using System.Collections.Generic;
using System.Runtime.Serialization;
using SnippetCache.Data.DTO;

namespace SnippetCache.Data.Wcf.Responses
{
    [DataContract]
    public class GetFilesBySnippetIdResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<FileDTO> Files { get; set; }
    }

    [DataContract]
    public class GetFileByIdResponse : ServiceResponseBase
    {
        [DataMember]
        public FileDTO File { get; set; }
    }

    [DataContract]
    public class GetFilesByUserIdResponse : ServiceResponseBase
    {
        [DataMember]
        public IEnumerable<FileDTO> Files { get; set; }
    }

    [DataContract]
    public class CreateFileResponse : ServiceResponseBase
    {
        [DataMember]
        public int FileId { get; set; }
    }

    [DataContract]
    public class UpdateFileResponse : ServiceResponseBase
    {
        // No op
    }

    [DataContract]
    public class DeleteFileResponse : ServiceResponseBase
    {
        // No op
    }
}