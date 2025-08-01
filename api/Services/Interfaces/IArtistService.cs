﻿using api.Models;

namespace api.Services.Interfaces;

public interface IArtistService
{
    Task<IEnumerable<Artist>> GetAllArtistsAsync(int? page = null, int? pageSize = null);
    Task<Artist> GetArtistByIdAsync(int id);
    Task AddArtistAsync(Artist artist);
    Task UpdateArtistAsync(int id, Artist artist);
    Task DeleteArtistAsync(int id);
}