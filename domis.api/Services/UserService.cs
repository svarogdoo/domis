﻿using domis.api.DTOs.User;
using domis.api.Models;
using domis.api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace domis.api.Services;

public interface IUserService
{
    Task<UserProfileDto?> GetUserByIdAsync(string id);
    Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updateDto);
}

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserProfileDto?> GetUserByIdAsync(string id) 
        => await userRepository.GetUserByIdAsync(id);

    public async Task<bool> UpdateUserProfileAsync(string id, ProfileUpdateRequest updateDto)
        => await userRepository.UpdateUserProfileAsync(id, updateDto);
}