using domis.api.Models;
using domis.api.Repositories;
using Serilog;
using System.Globalization;
using System.Text;
using System.Xml;

namespace domis.api.Services;

public interface ISyncService
{
    Task<bool> NivelacijaUpdateBatch();
    Task<bool> UpdateExchangeRate();
}

public class SyncService : ISyncService
{
    private readonly string _nbsUsername;
    private readonly string _nbsPassword;
    private readonly string _nbsLicenseId;
    private readonly int _nbsExchangeRateListTypeId;
    private readonly string _nbsUrl;

    private readonly string _nivelacijaUrl = "https://domisenterijeri.com/domis/NIVELACIJA.csv";

    private readonly IProductRepository _productRepository;
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SyncService(IProductRepository productRepo, IExchangeRateRepository exchangeRateRepo, IConfiguration configuration, HttpClient httpClient)
    {
        _productRepository = productRepo;
        _exchangeRateRepository = exchangeRateRepo;
        _httpClient = httpClient;
        _configuration = configuration;

        _nbsUsername = _configuration["NBSSettings:Username"] ?? string.Empty;
        _nbsPassword = _configuration["NBSSettings:Password"] ?? string.Empty;
        _nbsLicenseId = _configuration["NBSSettings:LicenseId"] ?? string.Empty;
        _nbsExchangeRateListTypeId = int.TryParse(_configuration["NBSSettings:ExchangeRateListTypeId"], out var exchangeRateListTypeId)
            ? exchangeRateListTypeId
            : 3; 
        _nbsUrl = _configuration["NBSSettings:Url"] ?? "";
    }


    public async Task<bool> UpdateExchangeRate()
    {
        var nbsSoapEnvelope = $@"
            <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://communicationoffice.nbs.rs'>
               <soapenv:Header>
                  <web:AuthenticationHeader>
                     <web:UserName>{_nbsUsername}</web:UserName>
                     <web:Password>{_nbsPassword}</web:Password>
                     <web:LicenceID>{_nbsLicenseId}</web:LicenceID>
                  </web:AuthenticationHeader>
               </soapenv:Header>
               <soapenv:Body>
                  <web:GetCurrentExchangeRate>
                     <web:exchangeRateListTypeID>{_nbsExchangeRateListTypeId}</web:exchangeRateListTypeID>
                  </web:GetCurrentExchangeRate>
               </soapenv:Body>
            </soapenv:Envelope>";

        try
        {
            var httpContent = new StringContent(nbsSoapEnvelope, Encoding.UTF8, "text/xml");

            var response = await _httpClient.PostAsync(_nbsUrl, httpContent);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var doc = new XmlDocument();
            doc.LoadXml(responseBody);

            var namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("web", "http://communicationoffice.nbs.rs");

            var resultNode = doc.SelectSingleNode("//soap:Body/web:GetCurrentExchangeRateResponse/web:GetCurrentExchangeRateResult", namespaceManager);
            if (resultNode != null)
            {
                var innerXml = resultNode.InnerText;

                var exchangeRateDoc = new XmlDocument();
                exchangeRateDoc.LoadXml(innerXml);

                var middleRateNode = exchangeRateDoc.SelectSingleNode("//ExchangeRate/MiddleRate");

                if (middleRateNode != null)
                {
                    var middleRate = middleRateNode.InnerText;
                    var result = decimal.Parse(middleRate, CultureInfo.InvariantCulture);

                    return await _exchangeRateRepository.UpdateExchangeRate(GetBelgradeCurrentTime(), result);
                }
                else
                {
                    Log.Error("Failed to retrieve the middle rate.");
                    return false;
                }
            }
            else
            {
                Log.Error("No result node found in the response.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error while updating exchange rate from NBS: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> NivelacijaUpdateBatch()
    {
        var updates = new List<NivelacijaRecord>();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _nivelacijaUrl);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var lines = content.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var splitLine = line.Split(';');

                if (splitLine.Length != 3 ||
                    !int.TryParse(splitLine[0], out int sku) ||
                    !decimal.TryParse(splitLine[1], out decimal price) ||
                    !decimal.TryParse(splitLine[2], out decimal stock))
                {
                    continue;
                }

                updates.Add(new NivelacijaRecord(sku, price, stock));
            }

            if (updates.Count > 0)
            {
                await _productRepository.NivelacijaUpdateProductBatch(updates);
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Error while updating products with nivelacija values: {Message}", ex.Message);
            return false;
        }
    }

    private static DateTime GetBelgradeCurrentTime()
    {
        var belgradeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var belgradeDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, belgradeTimeZone);
        var dateToUse = belgradeDateTime.Date;
        return dateToUse;
    }
}