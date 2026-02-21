using Data.Candidate;
using Data.Entities;
using Moq;

namespace Core.Tests
{
    public class CandidateServiceTest
    {
        private readonly Mock<ICandidateRepository> _mockCandidateRepository;
        private readonly CandidateService.CandidateService _candidateService;

        public CandidateServiceTest()
        {
            _mockCandidateRepository = new Mock<ICandidateRepository>();
            _candidateService = new CandidateService.CandidateService(_mockCandidateRepository.Object);
        }

        [Fact]
        public async Task SearchCandidatesAsync_ShouldReturnCandidates_WhenValidNameProvided()
        {
            // Arrange
            string fullName = "Prabhu Deva";
            var mockEntities = new List<CandidateEntity>
            {
                new CandidateEntity { CandidateId = 1, FullName = "Prabhu Deva", Email = "prabhudevasr@example.com" },
                new CandidateEntity { CandidateId = 2, FullName = "Prabhu Deva", Email = "Prabhudevajr@example.com" }
            };

            _mockCandidateRepository
                .Setup(repo => repo.SearchCandidatesAsync(fullName))
                .ReturnsAsync(mockEntities);

            // Act
            var result = await _candidateService.SearchCandidatesAsync(fullName);

            // Assert
            Assert.Equal(result.Count, mockEntities.Count);

            _mockCandidateRepository.Verify(repo => repo.SearchCandidatesAsync(fullName), Times.Once);
        }

        [Fact]
        public async Task SearchCandidatesAsync_ShouldReturnEmptyList_WhenNoCandidatesFound()
        {
            // Arrange
            var fullName = "NonExistent Person";
            _mockCandidateRepository
                .Setup(repo => repo.SearchCandidatesAsync(fullName))
                .ReturnsAsync(new List<CandidateEntity>());

            // Act
            var result = await _candidateService.SearchCandidatesAsync(fullName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}