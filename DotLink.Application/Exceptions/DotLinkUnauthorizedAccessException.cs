namespace DotLink.Application.Exceptions
{
    public class DotLinkUnauthorizedAccessException : Exception
    {
        public DotLinkUnauthorizedAccessException(string message)
            : base(message) { }
    }
}
