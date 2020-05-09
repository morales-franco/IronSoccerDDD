using IronSoccerDDD.Core.Entities;

namespace IronSoccerDDD.Api.Dtos
{
    public class MatchDto : MatchInListDto
    {
        internal static MatchDto FromEntity(Match match)
        {
            return new MatchDto
            {
                Id = match.Id,
                BestPlayer = match.BestPlayer == null ? null : match.BestPlayer.CompleteName.ToString(),
                MatchDate = match.MatchDate,
                TeamA = match.TeamA.Name,
                TeamB = match.TeamB.Name,
                Winner = match.Winner == null ? null : match.Winner.Name
            };
        }
    }
}
