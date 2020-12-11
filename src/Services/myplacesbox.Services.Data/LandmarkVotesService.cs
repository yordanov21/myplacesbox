namespace MyPlacesBox.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class LandmarkVotesService : ILandmarkVotesService
    {
        private readonly IRepository<LandmarkVote> votesRepository;

        public LandmarkVotesService(IRepository<LandmarkVote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public double GetAverageVotes(int landmarkId)
        {
            return this.votesRepository.All()
               .Where(x => x.LandmarkId == landmarkId)
               .Average(x => x.Value);
        }

        public async Task SetVoteAsync(int landmarkId, string userId, byte value)
        {
            var vote = this.votesRepository.All()
               .FirstOrDefault(x => x.LandmarkId == landmarkId && x.UserId == userId);
            if (vote == null)
            {
                vote = new LandmarkVote
                {
                    LandmarkId = landmarkId,
                    UserId = userId,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = value;
            await this.votesRepository.SaveChangesAsync();
        }
    }
}
