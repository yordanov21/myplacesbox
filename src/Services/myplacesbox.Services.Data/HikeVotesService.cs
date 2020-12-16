namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class HikeVotesService : IHikeVotesService
    {
        private readonly IRepository<HikeVote> votesRepository;

        public HikeVotesService(IRepository<HikeVote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public double GetAverageVotes(int hikeId)
        {
            return this.votesRepository.All()
               .Where(x => x.HikeId == hikeId)
               .Average(x => x.Value);
        }

        public async Task SetVoteAsync(int hikeId, string userId, byte value)
        {
            var vote = this.votesRepository.All()
              .FirstOrDefault(x => x.HikeId == hikeId && x.UserId == userId);
            if (vote == null)
            {
                vote = new HikeVote
                {
                    HikeId = hikeId,
                    UserId = userId,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = value;
            await this.votesRepository.SaveChangesAsync();
        }
    }
}
