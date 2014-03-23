using System.Runtime.Serialization;

namespace SnippetCache.Data.DTO
{
    public class LanguageDTO : ILanguage
    {
        #region ILanguage Members

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Id { get; set; }

        #endregion
    }
}