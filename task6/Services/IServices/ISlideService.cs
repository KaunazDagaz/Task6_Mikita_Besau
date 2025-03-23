using task6.Models;

namespace task6.Services.IServices
{
    public interface ISlideService
    {
        Task<Slide> AddSlideAsync(Guid presentationId, int order);
        Task<bool> UpdateSlideContentAsync(Guid slideId, string content);
        Task<bool> DeleteSlideAsync(Guid slideId);
    }
}
