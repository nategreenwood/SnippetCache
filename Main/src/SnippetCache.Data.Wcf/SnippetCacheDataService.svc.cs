using SnippetCache.Data.EF4;

namespace SnippetCache.Data.Wcf
{
    public partial class SnippetCacheDataService : ISnippetCacheDataService
    {
        private readonly DataUnitOfWork _unitOfWork;

        public SnippetCacheDataService(DataUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        /*
         *  Implementations are broken out into seperate partial classes. 
         *  Ex. Language.svc.cs - Language implementations
         * 
         */
    }
}