namespace task6.Exceptions
{
    public class PresentationException : AppException
    {
        public PresentationException(string message, int statusCode = 400) : base(message, statusCode) { }
    }

    public class PresentationNotFoundException : PresentationException
    {
        public PresentationNotFoundException(string message) : base(message, 404) { }
    }
}
