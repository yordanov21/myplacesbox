namespace MyPlacesBox.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels.Votes;

    public class HikeVotesController : BaseController
    {
        private readonly IHikeVotesService votesService;

        public HikeVotesController(IHikeVotesService votesService)
        {
            this.votesService = votesService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostHikeVoteResponseModel>> Post(PostHikeVoteInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.votesService.SetVoteAsync(input.HikeId, userId, input.Value);
            var averageVotes = this.votesService.GetAverageVotes(input.HikeId);
            return new PostHikeVoteResponseModel { AverageVote = averageVotes };
        }
    }
}
