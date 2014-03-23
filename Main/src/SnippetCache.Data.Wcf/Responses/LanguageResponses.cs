using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class CreateNewLanguageResponse : ServiceResponseBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetAllLanguagesResponse : ServiceResponseBase
    {
        [DataMember(IsRequired = true)]
        public IEnumerable<LanguageQuickInfo> Languages { get; set; }
    }

    [DataContract]
    public class LanguageQuickInfo
    {
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetLanguageByIdResponse : ServiceResponseBase
    {
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetLanguageByNameResponse : ServiceResponseBase
    {
        [DataMember] public LanguageQuickInfo Language;
    }

    [DataContract]
    public class UpdateLanguageResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class DeleteLanguageResponse : ServiceResponseBase
    {
        // No op
    }
}