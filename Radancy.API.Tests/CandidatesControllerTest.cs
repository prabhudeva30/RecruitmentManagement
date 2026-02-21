using Core;
using Core.CandidateService;
using Core.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Radancy.APi.Controllers;

namespace Radancy.API.Tests
{
    public class CandidatesControllerTest
    {
        private readonly Mock<ICandidateService> _mockCandidateService;
        private readonly Mock<ILogger<CandidatesController>> _mockLogger;
        private readonly CandidatesController _controller;
        private static readonly string VALIDATION_MESSAGE = "Full name can only contain letters (a-z, A-Z). No spaces, numbers, or special characters allowed.";

        public CandidatesControllerTest()
        {
            _mockCandidateService = new Mock<ICandidateService>();
            _mockLogger = new Mock<ILogger<CandidatesController>>();
            _controller = new CandidatesController(_mockLogger.Object, _mockCandidateService.Object);
        }

        [Fact]
        public async Task GetCandidates_WithValidFullName_ReturnsOkWithCandidates()
        {
            // Arrange
            var fullName = "Prabhu Deva";
            var expectedCandidates = new List<CandidateResponse>
            {
                new CandidateResponse { FullName = "Prabhu Deva", Email = "prabhudevasr@example.com" },
                new CandidateResponse { FullName = "Prabhu Deva", Email = "prabhudevajr@example.com" }
            };

            _mockCandidateService
                .Setup(s => s.SearchCandidatesAsync(fullName))
                .ReturnsAsync(expectedCandidates);

            // Act
            var result = await _controller.GetCandidates(fullName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCandidates = Assert.IsAssignableFrom<List<CandidateResponse>>(okResult.Value);
            Assert.Equal(2, actualCandidates.Count);
            Assert.Equal(expectedCandidates[0].Email, actualCandidates[0].Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("Prabhu123")]
        [InlineData("Prabhu@Deva")]
        public async Task GetCandidates_WithNumbersInFullName_ReturnsBadRequest(string fullName)
        {
            // Act
            var result = await _controller.GetCandidates(fullName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(VALIDATION_MESSAGE, badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task GetCandidates_WhenNoCandidatesFound_ReturnsOkWithEmptyList()
        {
            // Arrange
            var fullName = "Nonexistent Person";
            _mockCandidateService
                .Setup(s => s.SearchCandidatesAsync(fullName))
                .ReturnsAsync(new List<CandidateResponse>());

            // Act
            var result = await _controller.GetCandidates(fullName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCandidates = Assert.IsAssignableFrom<List<CandidateResponse>>(okResult.Value);
            Assert.Empty(actualCandidates);
        }
    }
}