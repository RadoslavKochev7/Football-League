using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Core.Entities;
using FootballLeague.Core.Services;
using Moq;
using System.Linq.Expressions;
using Match = FootballLeague.Core.Entities.Match;

namespace FootballLeague.Tests.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IMatchRepository> _mockRepo;
        private MatchService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IMatchRepository>();
            _service = new MatchService(_mockRepo.Object);
        }

        [Test]
        public async Task AddAsync_CallsRepository()
        {
            var req = new MatchCreateRequest(1, 2, 3, 1, DateTime.Now);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Match>())).ReturnsAsync(new MatchDto());

            await _service.AddAsync(req);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Match>(m =>
                m.HomeTeamId == req.HomeTeamId &&
                m.AwayTeamId == req.AwayTeamId &&
                m.HomeTeamGoals == req.HomeTeamGoals &&
                m.AwayTeamGoals == req.AwayTeamGoals)));
        }

        [Test]
        public async Task DeleteAsync_CallsRepository()
        {
            await _service.DeleteAsync(1);
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task EditAsync_UpdatesMatch_WhenFound()
        {
            var match = new Match { Id = 1, HomeTeamGoals = 0, AwayTeamGoals = 0, PlayedOn = DateTime.UtcNow };
            var update = new MatchUpdateRequest(2, 2, DateTime.UtcNow);
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(match);
            _mockRepo.Setup(r => r.UpdateAsync(match)).ReturnsAsync(new MatchDto());

            await _service.EditAsync(1, update);

            Assert.Multiple(() =>
            {
                Assert.That(match.HomeTeamGoals, Is.EqualTo(2));
                Assert.That(match.AwayTeamGoals, Is.EqualTo(2));
                Assert.That(match.PlayedOn, Is.EqualTo(update.PlayedOn));
            });

            _mockRepo.Verify(r => r.UpdateAsync(match), Times.Once);
        }

        [Test]
        public async Task EditAsync_DoesNothing_WhenNotFound()
        {
            var update = new MatchUpdateRequest(2, 2, DateTime.Now);
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Match?)null);

            await _service.EditAsync(2, update);

            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Match>()), Times.Never);
        }

        [Test]
        public async Task GetAllPlayedMatchesAsync_ReturnsDtos()
        {
            var matches = new List<Match>
            {
                new Match { Id = 1, HomeTeam = new Team { Name = "A" }, AwayTeam = new Team { Name = "B" }, HomeTeamGoals = 2, AwayTeamGoals = 1, PlayedOn = System.DateTime.Now },
                new Match { Id = 2, HomeTeam = new Team { Name = "C" }, AwayTeam = new Team { Name = "D" }, HomeTeamGoals = 0, AwayTeamGoals = 0, PlayedOn = System.DateTime.Now }
            };
            _mockRepo.Setup(r => r.GetAllReadonlyAsync()).ReturnsAsync(matches);

            var result = await _service.GetAllPlayedMatchesAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().HomeTeamName, Is.EqualTo("A"));
        }

        [Test]
        public async Task GetAllPlayedMatchesAsync_ReturnsEmpty_WhenNoMatches()
        {
            _mockRepo.Setup(r => r.GetAllReadonlyAsync()).ReturnsAsync(new List<Match>());

            var result = await _service.GetAllPlayedMatchesAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsDto_WhenFound()
        {
            var match = new Match
            {
                Id = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamGoals = 2,
                AwayTeamGoals = 1,
                PlayedOn = System.DateTime.Now,
                HomeTeam = new Team { Name = "A" },
                AwayTeam = new Team { Name = "B" }
            };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(match);

            var result = await _service.GetByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(match.Id));
                Assert.That(result.HomeTeamName, Is.EqualTo("A"));
                Assert.That(result.AwayTeamName, Is.EqualTo("B"));
            });
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Match?)null);

            var result = await _service.GetByIdAsync(2);

            Assert.That(result, Is.Null);
        }
    }
}