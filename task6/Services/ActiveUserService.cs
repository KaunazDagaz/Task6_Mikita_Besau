using System.Collections.Concurrent;
using task6.Exceptions;
using task6.Models.InMemoryModels;
using task6.Services.IServices;

namespace task6.Services
{
    public class ActiveUserService : IActiveUserService
    {
        private readonly ConcurrentDictionary<string, ActiveUser> users = new ConcurrentDictionary<string, ActiveUser>();

        public ActiveUser AddUser(string connectionId, string nickname, Guid userId, Guid presentationId, string role)
        {
            var user = new ActiveUser
            {
                Id = userId,
                ConnectionId = connectionId,
                Nickname = nickname,
                Role = role,
                PresentationId = presentationId,
                JoinedAt = DateTime.UtcNow
            };

            bool isAdded = users.TryAdd(connectionId, user);
            if (!isAdded) throw new UserAlreadyExistsException("User already exists");
            return user;
        }

        public ActiveUser GetUser(string connectionId)
        {
            bool isGot = users.TryGetValue(connectionId, out var user);
            if (!isGot || user == null) throw new UserNotFoundException("User not found");
            return user;
        }

        public List<ActiveUser> GetUsersInPresentation(Guid presentationId)
        {
            return users.Values
                .Where(u => u.PresentationId == presentationId)
                .ToList();
        }

        public bool RemoveUser(string connectionId)
        {
            bool isRemoved = users.TryRemove(connectionId, out _);
            if (!isRemoved) throw new UserNotFoundException("User not found");
            return true;
        }

        public bool UpdateUserRole(string connectionId, string newRole)
        {
            bool isGot = users.TryGetValue(connectionId, out var user);
            if (!isGot || user == null) throw new UserNotFoundException("User not found");
            user.Role = newRole;
            return true;
        }

        public bool IsUserCreator(string connectionId, Guid presentationId)
        {
            bool isGot = users.TryGetValue(connectionId, out var user);
            if (!isGot || user == null) throw new UserNotFoundException("User not found");
            return user.Role == "Creator" && user.PresentationId == presentationId;
        }

        public bool IsUserEditor(string connectionId, Guid presentationId)
        {
            bool isGot = users.TryGetValue(connectionId, out var user);
            if (!isGot || user == null) throw new UserNotFoundException("User not found");
            return user.PresentationId == presentationId &&
                (user.Role == "Creator" || user.Role == "Editor");
        }
    }
}
