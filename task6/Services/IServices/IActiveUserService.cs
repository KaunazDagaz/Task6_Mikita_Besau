using task6.Models.InMemoryModels;

namespace task6.Services.IServices
{
    public interface IActiveUserService
    {
        User AddUser(string connectionId, string nickname, Guid presentationId, string creatorNickname);
        User GetUser(string connectionId);
        List<User> GetUsersInPresentation(Guid presentationId);
        bool RemoveUser(string connectionId);
        bool UpdateUserRole(string connectionId, string newRole);
        bool IsUserCreator(string connectionId, Guid presentationId);
        bool IsUserEditor(string connectionId, Guid presentationId);
    }
}
