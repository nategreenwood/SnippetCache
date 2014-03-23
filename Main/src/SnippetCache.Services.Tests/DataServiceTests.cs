using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnippetCache.Services.Tests.SnippetDataService;

namespace SnippetCache.Services.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        private SnippetCacheDataServiceClient _dataClient;

        [TestInitialize]
        public void Init()
        {
            _dataClient = new SnippetCacheDataServiceClient("BasicHttpBinding_ISnippetCacheDataService");
        }

        #region Language Tests

        [TestMethod]
        public void GetAllLanguagesTest()
        {
            var request = new GetAllLanguagesRequest();

            GetAllLanguagesResponse results = _dataClient.GetAllLanguages(request);

            Assert.IsNotNull(results);
        }

        #endregion
    }
}