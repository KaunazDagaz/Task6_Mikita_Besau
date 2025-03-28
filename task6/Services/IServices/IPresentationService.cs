﻿using task6.Models;

namespace task6.Services.IServices
{
    public interface IPresentationService
    {
        Task<List<Presentation>> GetAllPresentationsAsync();
        Task<Presentation> GetPresentationByIdAsync(Guid id);
        Task<Presentation> CreatePresentationAsync(string title, string creatorNickname, Guid creatorId);
        Task<bool> DeletePresentationAsync(Guid id);
    }
}
