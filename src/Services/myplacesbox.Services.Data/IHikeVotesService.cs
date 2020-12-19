namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHikeVotesService
    {
        Task SetVoteAsync(int hikeId, string userId, byte value);

        double GetAverageVotes(int hikeId);
    }
}
