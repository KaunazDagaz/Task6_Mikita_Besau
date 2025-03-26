namespace task6.Hubs.IHubs
{
    public interface IPresentationHub
    {
        Task JoinPresentation(Guid presentationId, string nickname, Guid userId);
        Task LeavePresentation(Guid presentationId);
        Task ChangeUserRole(string connectionId, string newRole);
        Task UpdateSlideContent(Guid slideId, string content);
        Task AddNewSlide(Guid presentationId);
        Task DeleteSlide(Guid slideId);
    }
}
