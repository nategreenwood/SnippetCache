using System;
using System.Collections.Generic;
using SnippetCache.Data.DTO;

namespace SnippetCache.Managers
{
    public interface ISnippetManager : IManager
    {
        IEnumerable<SnippetDTO> PerformSearch(string searchText, int userId, Guid userGuid);
    }
}