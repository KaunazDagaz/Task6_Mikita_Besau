using Microsoft.JSInterop;
using task6.Services.IServices;

namespace task6.Services
{
    public class SessionStorageService : ISessionStorageService
    {
        private readonly IJSRuntime jsRuntime;

        public SessionStorageService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task SetNicknameAsync(string nickname)
        {
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "UserNickname", nickname);
        }
        public async Task<string> GetNicknameAsync()
        {
            return await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "UserNickname");
        }

        public async Task RemoveNicknameAsync()
        {
            await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "UserNickname");
        }

        public async Task SetUserIdAsync(Guid userId)
        {
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "userId", userId.ToString());
        }

        public async Task<Guid> GetUserIdAsync()
        {
            var userId = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "userId");
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var result))
            {
                return Guid.Empty;
            }
            return result;
        }

        public async Task RemoveUserIdAsync()
        {
            await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "userId");
        }
    }
}
