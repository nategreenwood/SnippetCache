namespace SnippetCache.Data
{
    public interface INotificationType : IBaseEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}