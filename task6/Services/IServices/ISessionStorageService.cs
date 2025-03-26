namespace task6.Services.IServices
{
    public interface ISessionStorageService
    {
        Task SetNicknameAsync(string nickname);
        Task<string> GetNicknameAsync();
        Task RemoveNicknameAsync();
        Task SetUserIdAsync(Guid userId);
        Task<Guid> GetUserIdAsync();
        Task RemoveUserIdAsync();
    }
}
