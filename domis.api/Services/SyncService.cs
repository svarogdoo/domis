using CsvHelper;
using CsvHelper.Configuration;
using domis.api.Models;
using domis.api.Repositories;
using Serilog;
using System.Globalization;
using System.Net.Http;

namespace domis.api.Services;

public interface ISyncService
{
    //Task<bool> NivelacijaUpdate();
    Task<bool> NivelacijaUpdateBatch();
}

public class SyncService : ISyncService
{
    private readonly IProductRepository _repository;
    private readonly HttpClient _httpClient;

    public SyncService(IProductRepository repository, HttpClient httpClient)
    {
        _repository = repository;
        _httpClient = httpClient;
    }

    private readonly string _url = "https://domisenterijeri.com/domis/NIVELACIJA.csv";

    public async Task<bool> NivelacijaUpdateBatch()
    {
        var updates = new List<NivelacijaRecord>();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
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
                await _repository.NivelacijaUpdateProductBatch(updates);
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Error while updating products with nivelacija values: {Message}", ex.Message);
            return false;
        }
    }

    //public async Task<bool> NivelacijaUpdate()
    //{
    //    var result = false;

    //    try
    //    {
    //        var request = new HttpRequestMessage(HttpMethod.Get, _url);
    //        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

    //        var response = await _httpClient.SendAsync(request);
    //        response.EnsureSuccessStatusCode();

    //        var content = await response.Content.ReadAsStringAsync();

    //        var lines = content.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);

    //        foreach (var line in lines)
    //        {
    //            var splitLine = line.Split(';');

    //            if (splitLine.Length != 3 ||
    //                !int.TryParse(splitLine[0], out int id) ||
    //                !decimal.TryParse(splitLine[1], out decimal price) ||
    //                !decimal.TryParse(splitLine[2], out decimal stock))
    //            {
    //                continue;
    //            }

    //            var product = await _repository.GetById(id);

    //            if (product == null || (product.Price == price && product.Stock == stock))
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                await _repository.NivelacijaUpdateProduct(id, price, stock);
    //            }
    //        }

    //        //// USING CSVHELPER & NivelacijaRecord OBJECT
    //        //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    //        //{
    //        //    Delimiter = ";",
    //        //    HasHeaderRecord = false,
    //        //};

    //        //using var reader = new StringReader(content);
    //        //using var csv = new CsvReader(reader, config);

    //        ////TO-DO: maybe don't use object at all
    //        //var records = csv.GetRecords<NivelacijaRecord>().ToList();

    //        //foreach (var record in records)
    //        //{
    //        //    var product = await _repository.GetById(record.Id);

    //        //    if (product is null)
    //        //    {
    //        //        continue;
    //        //    }
    //        //    else if (record.Price != product.Price || record.Stock != product.Stock)
    //        //    {
    //        //        await _repository.NivelacijaUpdateProduct(record);
    //        //    }
    //        //}

    //        result = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error("Error while updating products with nivelacija values: {Message}", ex.Message);
    //    }

    //    return result;
    //}
}