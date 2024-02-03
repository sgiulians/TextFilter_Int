using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace TextFilter.Tests.IntegrationTests
{
    [TestFixture]
    public class AppSequentialNoBatchingTests
    {
        private ApplicationConfiguration _appConfig;
        private Mock<ILogger<AppSequentialNoBatching>> _loggerMock;
        private Mock<IOptions<ApplicationConfiguration>> _optionsAppConfigMock;
        private Mock<IFilterService> _filterServiceMock;
        private Mock<IFileReaderService> _fileReaderServiceMock;

        private string[] _baseLineExpectations;

        private string[] CreateBaselineExpectations(string filePath) =>
            File.ReadAllLines(filePath)
                .Where(i => !FilterMethods.MiddleVowelFilter(i))
                .Where(i => !FilterMethods.LessThanThreeCharactersFilter(i))
                .Where(i => !FilterMethods.WordContainingTFilter(i))
                .ToArray();

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AppSequentialNoBatching>>();
            _optionsAppConfigMock = new Mock<IOptions<ApplicationConfiguration>>();
            _filterServiceMock = new Mock<IFilterService>();
            _fileReaderServiceMock = new Mock<IFileReaderService>();


            // CREATE BASELINE EXPECTATIONS
            var testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, Path.Combine("IntegrationTests", "words.txt"));
            _baseLineExpectations = CreateBaselineExpectations(testFilePath);


            // INJECT FILE CONTENT INTO THE MOCK TO TEST AGAINST
            _fileReaderServiceMock.Setup(m => m.ReadAllText(It.IsAny<string>()))
                .Returns(File.ReadAllText(testFilePath));

            _optionsAppConfigMock.Setup(o => o.Value).Returns(
                _appConfig = new ApplicationConfiguration
                {
                    InputFilePath = testFilePath,
                    WordSeparator = new[] { " ", "\n", "\r" },
                    Filters = new[] { "MiddleVowelFilter",
                        "LessThanThreeCharactersFilter",
                        "WordContainingTFilter"
                    }
                });
               
            _filterServiceMock.Setup(f => f.Filters)
                .Returns(new List<Func<string, bool>>(ServiceCollectionExtension.LookUpFilters(_appConfig.Filters)));
        }

        [Test]
        public void RunExecutesWithExpectedBehavior()
        {
            // Arrange
            var app = new AppSequentialNoBatching(_loggerMock.Object,
                _optionsAppConfigMock.Object,
                _filterServiceMock.Object,
                _fileReaderServiceMock.Object);

            // Act
            var actual = app.Run();

            // Assert
            CollectionAssert.AreEquivalent(_baseLineExpectations, actual);
        }
    }
}