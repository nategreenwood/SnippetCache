using System.Collections.Generic;
using System.Linq;
using SnippetCache.Data;
using SnippetCache.Data.DTO;

namespace SnippetCache.Managers.Wcf
{
    public static class Extensions
    {
        public static IEnumerable<SnippetDTO> ToSnippetDTO(this IEnumerable<ISnippet> collection)
        {
            var returnCollection = new List<SnippetDTO>();
            ISnippet[] evalArray = collection.ToArray();

            if (evalArray.Any())
            {
                returnCollection.AddRange(evalArray.Select(snippet =>
                                                           new SnippetDTO
                                                               {
                                                                   Id = snippet.Id,
                                                                   Guid = snippet.Guid,
                                                                   Name = snippet.Name,
                                                                   Description = snippet.Description,
                                                                   PreviewData = snippet.PreviewData,
                                                                   Data = snippet.Data,
                                                                   LastModified = snippet.LastModified,
                                                                   Language_Id = snippet.Language_Id,
                                                                   User_Id = snippet.User_Id,
                                                                   User_FormsAuthId = snippet.User_FormsAuthId
                                                               }));
            }

            return returnCollection;
        }
    }
}