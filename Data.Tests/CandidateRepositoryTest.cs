using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Tests
{
    public class CandidateRepositoryTest : IDisposable
    {
        private readonly RadancyDbContext _context;
        private readonly CandidateRepository _repository;

        public CandidateRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<RadancyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RadancyDbContext(options);
            _repository = new CandidateRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var candidates = new List<CandidateEntity>
            {
                new CandidateEntity { CandidateId = 1, FullName = "Prabhu Deva", Email = "prabhudevasr@example.com" },
                new CandidateEntity { CandidateId = 2, FullName = "Prabhu Deva", Email = "Prabhudevajr@example.com" },
                new CandidateEntity { CandidateId = 3, FullName = "Prabhu", Email = "prabhu@example.com" },
                new CandidateEntity { CandidateId = 4, FullName = "Prabhu", Email = "vikramprabhu@example.com" },
                new CandidateEntity { CandidateId = 5, FullName = "Prabhu", Email = "praburaj@example.com" },
                new CandidateEntity { CandidateId = 6, FullName = "Deva", Email = "deva@example.com" }            };

            _context.Candidates.AddRange(candidates);
            _context.SaveChanges();
        }

        [Theory]
        [InlineData("Prabhu Deva", 2)]
        [InlineData("Prabhu", 3)]
        [InlineData("PrabhuD", 0)]
        public async Task SearchCandidatesAsync_ShouldReturnCandidate_WhenExactMatchFound(string fullName, int expectedCount)
        {
            // Arrange

            // Act
            var result = await _repository.SearchCandidatesAsync(fullName);

            // Assert
            Assert.Equal(result.Count, expectedCount);
        }

        [Fact]
        public async Task SearchCandidatesAsync_ShouldBeCaseInsensitive()
        {
            // Arrange
            var fullName = "Deva";

            // Act
            var result = await _repository.SearchCandidatesAsync(fullName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(fullName, result[0].FullName);
            Assert.Equal("deva@example.com", result[0].Email);
        }

        [Fact]
        public async Task SearchCandidatesAsync_ShouldReturnEmptyList_WhenNoMatchFound()
        {
            // Arrange
            var fullName = "NonExistent Person";

            // Act
            var result = await _repository.SearchCandidatesAsync(fullName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchCandidatesAsync_ShouldReturnEmptyList_WhenSearchStringIsEmpty()
        {
            // Arrange
            var fullName = "";

            // Act
            var result = await _repository.SearchCandidatesAsync(fullName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("Prabhu Deva", 2)]
        [InlineData("PRABHU DEVA", 2)]
        [InlineData("prabhu deva", 2)]
        public async Task SearchCandidatesAsync_ShouldHandleDifferentCases(string fullName, int expectedCount)
        {
            // Act
            var result = await _repository.SearchCandidatesAsync(fullName);

            // Assert
            Assert.Equal(expectedCount, result.Count);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}