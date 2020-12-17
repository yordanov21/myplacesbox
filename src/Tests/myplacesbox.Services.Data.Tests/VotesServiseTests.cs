namespace MyPlacesBox.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Moq;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using Xunit;

    public class VotesServiseTests
    {
        // Test Landmark Votes
        [Fact]
        public async Task WhenUserVotesForLandmark2TimesOnly1VoteShouldBeCounted()
        {
            var list = new List<LandmarkVote>();
            var mockRepo = new Mock<IRepository<LandmarkVote>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<LandmarkVote>())).Callback(
                (LandmarkVote vote) => list.Add(vote));
            var service = new LandmarkVotesService(mockRepo.Object);

            await service.SetVoteAsync(1, "1", 1);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);

            Assert.Single(list);
            Assert.Equal(5, list.First().Value);
        }

        [Fact]
        public async Task When2UsersVoteLandmarkForTheSameRecipeTheAverageVoteShouldBeCorrect()
        {
            var list = new List<LandmarkVote>();
            var mockRepo = new Mock<IRepository<LandmarkVote>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<LandmarkVote>())).Callback(
                (LandmarkVote vote) => list.Add(vote));
            var service = new LandmarkVotesService(mockRepo.Object);

            await service.SetVoteAsync(2, "Niki", 5);
            await service.SetVoteAsync(2, "Pesho", 1);
            await service.SetVoteAsync(2, "Niki", 2);

            mockRepo.Verify(x => x.AddAsync(It.IsAny<LandmarkVote>()), Times.Exactly(2));

            Assert.Equal(2, list.Count);
            Assert.Equal(1.5, service.GetAverageVotes(2));
        }

        // Test Hike Votes
        [Fact]
        public async Task WhenUserVotesForLHike2TimesOnly1VoteShouldBeCounted()
        {
            var list = new List<HikeVote>();
            var mockRepo = new Mock<IRepository<HikeVote>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<HikeVote>())).Callback(
                (HikeVote vote) => list.Add(vote));
            var service = new HikeVotesService(mockRepo.Object);

            await service.SetVoteAsync(1, "1", 1);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);

            Assert.Single(list);
            Assert.Equal(5, list.First().Value);
        }

        [Fact]
        public async Task When2UsersVoteHikeForTheSameRecipeTheAverageVoteShouldBeCorrect()
        {
            var list = new List<HikeVote>();
            var mockRepo = new Mock<IRepository<HikeVote>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<HikeVote>())).Callback(
                (HikeVote vote) => list.Add(vote));
            var service = new HikeVotesService(mockRepo.Object);

            await service.SetVoteAsync(2, "Niki", 5);
            await service.SetVoteAsync(2, "Pesho", 1);
            await service.SetVoteAsync(2, "Niki", 2);

            mockRepo.Verify(x => x.AddAsync(It.IsAny<HikeVote>()), Times.Exactly(2));

            Assert.Equal(2, list.Count);
            Assert.Equal(1.5, service.GetAverageVotes(2));
        }
    }
}
