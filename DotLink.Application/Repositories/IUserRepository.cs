﻿using DotLink.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DotLink.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}