namespace DotLink.Application.Exceptions
{
    public class DotLinkNotFoundException : Exception
    {
        public DotLinkNotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.") { }
    }
}
