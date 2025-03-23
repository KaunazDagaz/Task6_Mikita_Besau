namespace task6.Exceptions
{
    public class UserException : AppException
    {
        public UserException(string message, int statusCode = 400) : base(message) { }
    }

    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(string message) : base(message, 404) { }
    }

    public class UserAlreadyExistsException : UserException
    {
        public UserAlreadyExistsException(string message) : base(message, 409) { }
    }
}
