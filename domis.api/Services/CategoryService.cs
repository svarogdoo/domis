﻿using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>?> GetAll();

    //probably no need for this one
    Task<Category?> GetById(int id);
}

public class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public async Task<IEnumerable<Category>?> GetAll()
    {
        return await repository.GetAll();
    }

    //probably no need for this one
    public async Task<Category?> GetById(int id)
    {
        return await repository.GetById(id);
    }
}