using Microsoft.AspNetCore.Mvc;
using RaidCoachingGroup1MatchScores.Requests;
using RaidCoachingGroup1MatchScores.Responses;
using RaidCoachingGroup1MatchScores.Services;

namespace RaidCoachingGroup1MatchScores.Controller;

[ApiController]
[Route("api/[controller]")]
public class MatchController(IMatchService matchService) : ControllerBase
{
    [HttpPost("UpdateMatchScores")]
    public async Task<UpdateMatchScoresResponse> UpdateMatchScores(UpdateMatchScoresRequest request)
    {
        // Logic to update match scores
        // This would typically involve calling a service that interacts with the repository
        // to update the scores in the database.
        return new UpdateMatchScoresResponse
        {
            DisplayResult = matchService.UpdateMatchScore(request.GetUpdateMatchScoresModel())
        };
    }
}