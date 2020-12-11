using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyPlacesBox.Services.Data
{
    public interface ILandmarkVotesService
    {

        Task SetVoteAsync(int landmarkId, string userId, byte value);

        double GetAverageVotes(int landmarkId);
    }
}
