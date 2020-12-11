using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPlacesBox.Services.Data;
using MyPlacesBox.Web.ViewModels.Votes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyPlacesBox.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandmarkVotesController : BaseController
    {
        private readonly ILandmarkVotesService votesService;

        public LandmarkVotesController(ILandmarkVotesService votesService)
        {
            this.votesService = votesService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostLandmarkVoteResponseModel>> Post(PostLandmarkVoteInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.votesService.SetVoteAsync(input.LandmarkId, userId, input.Value);
            var averageVotes = this.votesService.GetAverageVotes(input.LandmarkId);
            return new PostLandmarkVoteResponseModel { AverageVote = averageVotes };
        }
    }
}
