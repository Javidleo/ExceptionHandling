namespace CustomException.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(string message) : base(message)
        { }
    }
}
