namespace SnippetCache.Utils.Security
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}