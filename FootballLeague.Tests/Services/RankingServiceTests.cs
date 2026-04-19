using FootballLeague.Core.Contracts;
using FootballLeague.Core.Entities;
using FootballLeague.Core.Services;
using Moq;

namespace FootballLeague.Tests.Services
{
    [TestFixture]
    public class RankingServiceTests
    {
        [Test]
        public async Task GetRankingAsync_ReturnsTeamsOrderedByPointsAndGoals()
        {
            var teams = new List<Team>
            {
                new() { Id = 1, Name = "A", Points = 10, GoalsFor = 15, GoalsAgainst = 5 },
                new() { Id = 2, Name = "B", Points = 10, GoalsFor = 12, GoalsAgainst = 6 },
                new() { Id = 3, Name = "C", Points = 8, GoalsFor = 20, GoalsAgainst = 10 }
            };

            var mockRepo = new Mock<ITeamRepository>();
            mockRepo.Setup(r => r.GetAllReadonlyAsync()).ReturnsAsync(teams);

            var service = new RankingService(mockRepo.Object);

            var ranking = (await service.GetRankingAsync()).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(ranking[0].TeamName, Is.EqualTo("A"));
                Assert.That(ranking[1].TeamName, Is.EqualTo("B"));
                Assert.That(ranking[2].TeamName, Is.EqualTo("C"));
            });
        }
    }
}