using System.Runtime.Serialization;

namespace SnippetCache.Data.Wcf.Requests
{
    [DataContract]
    public class CreateNewLanguageRequest
    {
        public CreateNewLanguageRequest(string name, string description)
        {
            Name = name;
            Description = description;
        }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = false)]
        public string Description { get; set; }
    }

    [DataContract]
    public class GetAllLanguagesRequest
    {
    }

    [DataContract]
    public class GetLanguageByIdRequest
    {
        public GetLanguageByIdRequest(int id)
        {
            Id = id;
        }

        [DataMember]
        public int Id { get; set; }
    }

    [DataContract]
    public class GetLanguageByNameRequest
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }

    [DataContract]
    public class UpdateLanguageRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class DeleteLanguageRequest
    {
        [DataMember]
        public int Id { get; set; }
    }
}