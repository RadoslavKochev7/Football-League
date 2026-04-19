using FootballLeague.Core.Contracts;
using FootballLeague.Core.Entities;
using FootballLeague.Core.Services;
using FootballLeague.Shared.Constants;
using Moq;

namespace FootballLeague.Tests.Services
{
    [TestFixture]
    public class TeamStatisticsServiceTests
    {
        private Mock<ITeamRepository> _mockRepo;
        private TeamStatisticsService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ITeamRepository>();
            _service = new TeamStatisticsService(_mockRepo.Object);
        }

        [Test]
        public async Task AddStats_UpdatesTeamsCorrectly()
        {
            var home = new Team { Id = 1, Name = "Home" };
            var away = new Team { Id = 2, Name = "Away" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(home);
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(away);

            await _service.AddStats(1, 2, 2, 1);

            Assert.Multiple(() =>
            {
                Assert.That(home.MatchesPlayed, Is.EqualTo(1));
                Assert.That(away.MatchesPlayed, Is.EqualTo(1));
                Assert.That(home.Wins, Is.EqualTo(1));
                Assert.That(away.Losses, Is.EqualTo(1));
                Assert.That(home.Points, Is.EqualTo(GlobalConstants.WinPoints));
            });
        }

        [Test]
        public async Task RevertStats_RevertsTeamsCorrectly()
        {
            var home = new Team { Id = 1, Name = "Home", MatchesPlayed = 1, Wins = 1, Points = GlobalConstants.WinPoints, GoalsFor = 2, GoalsAgainst = 1 };
            var away = new Team { Id = 2, Name = "Away", MatchesPlayed = 1, Losses = 1, GoalsFor = 1, GoalsAgainst = 2 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(home);
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(away);

            await _service.RevertStats(1, 2, 2, 1);

            Assert.Multiple(() =>
            {
                Assert.That(home.MatchesPlayed, Is.EqualTo(0));
                Assert.That(away.MatchesPlayed, Is.EqualTo(0));
                Assert.That(home.Wins, Is.EqualTo(0));
                Assert.That(away.Losses, Is.EqualTo(0));
                Assert.That(home.Points, Is.EqualTo(0));
            });
        }
    }
}