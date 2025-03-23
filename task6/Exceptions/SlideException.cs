namespace task6.Exceptions
{
    public class SlideException : AppException
    {

        public SlideException(string message, int statusCode = 400) : base(message, statusCode) { }
    }

    public class SlideNotFoundException : SlideException
    {
        public SlideNotFoundException(string message) : base(message, 404) { }
    }
}
