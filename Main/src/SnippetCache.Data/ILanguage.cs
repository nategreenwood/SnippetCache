namespace SnippetCache.Data
{
    public interface ILanguage : IBaseEntity
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}