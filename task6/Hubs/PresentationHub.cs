using Microsoft.AspNetCore.SignalR;
using task6.Hubs.IHubs;
using task6.Services.IServices;

namespace task6.Hubs
{
    public class PresentationHub : Hub, IPresentationHub
    {
        private readonly IPresentationService presentationService;
        private readonly ISlideService slideService;
        private readonly IActiveUserService activeUserService;

        public PresentationHub(IPresentationService presentationService, ISlideService slideService, IActiveUserService activeUserService)
        {
            this.presentationService = presentationService;
            this.slideService = slideService;
            this.activeUserService = activeUserService;
        }

        public async Task JoinPresentation(Guid presentationId, string nickname)
        {
            var presentation = await presentationService.GetPresentationByIdAsync(presentationId);
            await Groups.AddToGroupAsync(Context.ConnectionId, presentationId.ToString());
            var user = activeUserService.AddUser(
                Context.ConnectionId,
                nickname,
                presentationId,
                presentation.CreatorNickname
            );
            await Clients.Group(presentationId.ToString()).SendAsync("UserJoined", user);
            var users = activeUserService.GetUsersInPresentation(presentationId);
            await Clients.Caller.SendAsync("UsersInPresentation", users);
        }

        public async Task LeavePresentation(Guid presentationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, presentationId.ToString());
            var user = activeUserService.GetUser(Context.ConnectionId);
            activeUserService.RemoveUser(Context.ConnectionId);
            await Clients.Group(presentationId.ToString()).SendAsync("UserLeft", user.ConnectionId);
        }

        public async Task ChangeUserRole(string connectionId, string newRole)
        {
            var callerUser = activeUserService.GetUser(Context.ConnectionId);
            var targetUser = activeUserService.GetUser(connectionId);
            if (callerUser.Role != "Creator" || callerUser.PresentationId != targetUser.PresentationId) 
                throw new Exception("Only creator can change user role");
            activeUserService.UpdateUserRole(connectionId, newRole);
            await Clients.Group(callerUser.PresentationId.ToString())
                .SendAsync("UserRoleChanged", connectionId, newRole);
        }

        public async Task UpdateSlideContent(Guid slideId, string content)
        {
            var user = activeUserService.GetUser(Context.ConnectionId);
            var slide = presentationService.GetPresentationByIdAsync(user.PresentationId)
                .Result.Slides.FirstOrDefault(s => s.Id == slideId);
            if (user.Role != "Creator" || user.Role != "Editor")
                throw new Exception("Only creator or editor can update slide content");
            await slideService.UpdateSlideContentAsync(slideId, content);
            await Clients.Group(user.PresentationId.ToString())
                .SendAsync("SlideContentUpdated", slideId, content);
        }

        public async Task AddNewSlide(Guid presentationId)
        {
            var user = activeUserService.GetUser(Context.ConnectionId);
            if (user.Role != "Creator")
                throw new Exception("Only creator can add new slide");
            var presentation = await presentationService.GetPresentationByIdAsync(presentationId);
            int newOrder = presentation.Slides.Count;
            var newSlide = await slideService.AddSlideAsync(presentationId, newOrder);
            await Clients.Group(presentationId.ToString())
                .SendAsync("NewSlideAdded", newSlide);
        }

        public async Task DeleteSlide(Guid slideId)
        {
            var user = activeUserService.GetUser(Context.ConnectionId);
            if (user.Role != "Creator")
                throw new Exception("Only creator can delete slide");
            await slideService.DeleteSlideAsync(slideId);
            await Clients.Group(user.PresentationId.ToString())
                .SendAsync("SlideDeleted", slideId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = activeUserService.GetUser(Context.ConnectionId);
            activeUserService.RemoveUser(Context.ConnectionId);
            await Clients.Group(user.PresentationId.ToString())
                .SendAsync("UserLeft", user.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
