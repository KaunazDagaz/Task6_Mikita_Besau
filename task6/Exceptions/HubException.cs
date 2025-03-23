namespace task6.Exceptions
{
    public class HubException : AppException
    {
        public HubException(string message, int statusCode = 400) : base(message) { }
    }

    public class HubAccessForbidden : HubException
    {
        public HubAccessForbidden(string message) : base(message, 403) { }
    }
}
