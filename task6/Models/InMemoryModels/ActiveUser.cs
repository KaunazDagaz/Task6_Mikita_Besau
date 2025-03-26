namespace task6.Models.InMemoryModels
{
    public class ActiveUser
    {
        public Guid Id { get; set; }
        public required string ConnectionId { get; set; }
        public required string Nickname { get; set; }
        public required string Role { get; set; }
        public Guid PresentationId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
