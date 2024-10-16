﻿using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public interface IWalkRepository {

        Task<List<Walk>> GetAllAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int limit = 100
        );

        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> CreateAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
