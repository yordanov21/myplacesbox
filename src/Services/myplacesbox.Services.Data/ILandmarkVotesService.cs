namespace MyPlacesBox.Services.Data
{
    using System.Threading.Tasks;

    public interface ILandmarkVotesService
    {
        Task SetVoteAsync(int landmarkId, string userId, byte value);

        double GetAverageVotes(int landmarkId);
    }
}
