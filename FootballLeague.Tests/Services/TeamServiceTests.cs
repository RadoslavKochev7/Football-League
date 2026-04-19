using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Team;
using FootballLeague.Core.Entities;
using FootballLeague.Core.Services;
using Moq;
using System.Linq.Expressions;

namespace FootballLeague.Tests.Services
{
    [TestFixture]
    public class TeamServiceTests
    {
        private Mock<ITeamRepository> _mockRepo;
        private TeamService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ITeamRepository>();
            _service = new TeamService(_mockRepo.Object);
        }

        [Test]
        public async Task AddAsync_CallsRepository()
        {
            // Arrange
            var request = new TeamAddRequest("Chelsea");
            var teamDto = new TeamDto { Id = 1, Name = "Chelsea" };
            _mockRepo.Setup(r => r.AddAsync(request)).ReturnsAsync(teamDto);
            // Act
            var result = await _service.AddAsync(request);

            // Assert
            Assert.That(result.Name, Is.EqualTo("Chelsea"));
            _mockRepo.Verify(r => r.AddAsync(request), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_CallsRepository()
        {
            // Arrange - Act
            await _service.DeleteAsync(1);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task EditAsync_UpdatesTeam_WhenFound()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Old" };
            var update = new TeamUpdateRequest("NewName");

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);
            _mockRepo.Setup(r => r.UpdateAsync(team)).ReturnsAsync(new TeamDto { Id = 1, Name = "NewName" });

            // Act
            await _service.EditAsync(1, update);

            // Assert
            Assert.That(team.Name, Is.EqualTo("NewName"));
            _mockRepo.Verify(r => r.UpdateAsync(team), Times.Once);
        }

        [Test]
        public async Task EditAsync_DoesNothing_WhenNotFound()
        {
            // Arrange
            var update = new TeamUpdateRequest("Name");
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Team?)null);

            // Act
            await _service.EditAsync(2, update);

            // Assert
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Team>()), Times.Never);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsDto_WhenFound()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Chelsea", MatchesPlayed = 5, Wins = 3, Draws = 1, Losses = 1, GoalsFor = 10, GoalsAgainst = 5, Points = 10 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(team.Id));
                Assert.That(result.Name, Is.EqualTo(team.Name));
            });
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Team?)null);

            // Act
            var result = await _service.GetByIdAsync(2);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task TeamExists_ByName_CallsRepository()
        {
            // Arrange
            _mockRepo.Setup(r => r.Any(It.IsAny<Expression<Func<Team, bool>>>())).ReturnsAsync(true);

            // Act
            var exists = await _service.TeamExists("Chelsea");

            // Assert
            Assert.That(exists, Is.True);

            _mockRepo.Verify(r => r.Any(It.IsAny<Expression<Func<Team, bool>>>()), Times.Once);
        }

        [Test]
        public async Task TeamExists_ById_CallsRepository()
        {
            // Arrange
            _mockRepo.Setup(r => r.Any(It.IsAny<Expression<Func<Team, bool>>>())).ReturnsAsync(true);

            // Act
            var exists = await _service.TeamExists(1);

            // Assert
            Assert.That(exists, Is.True);
            _mockRepo.Verify(r => r.Any(It.IsAny<Expression<Func<Team, bool>>>()), Times.Once);
        }
    }
}