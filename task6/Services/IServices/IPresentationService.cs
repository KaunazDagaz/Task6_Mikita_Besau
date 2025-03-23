using task6.Models;

namespace task6.Services.IServices
{
    public interface IPresentationService
    {
        Task<List<Presentation>> GetAllPresentationsAsync();
        Task<Presentation> GetPresentationByIdAsync(Guid id);
        Task<Presentation> CreatePresentationAsync(string title, string creatorNickname);
        Task<bool> DeletePresentationAsync(Guid id);
        Task<Slide> AddSlideAsync (Guid presentationId, int order);
        Task<bool> UpdateSlideContentAsync(Guid slideId, string content);
        Task<bool> DeleteSlideAsync(Guid slideId);
    }
}
