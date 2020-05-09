using System.Linq;
using System.Threading.Tasks;
using IronSoccerDDD.Api.Dtos;
using IronSoccerDDD.Core;
using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace IronSoccerDDD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ApiBaseController
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MatchesController(IMatchRepository matchRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> GetAll()
        {
            var matches = await _matchRepository.GetListAsync(null);

            var dtos = matches.Select(x => new MatchInListDto()
            {
                Id = x.Id,
                BestPlayer = x.BestPlayer == null ? null : x.BestPlayer.CompleteName.ToString(),
                MatchDate = x.MatchDate,
                TeamA = x.TeamA.Name,
                TeamB = x.TeamB.Name,
                Winner = x.Winner == null ? null : x.Winner.Name

            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);

            if (match == null)
                return BadRequest($"Invalid match id : { id }");

            var dto = MatchDto.FromEntity(match);
    
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MatchCreateDto model)
        {
            var teamA = await _teamRepository.GetByIdAsync(model.TeamAId);
            if (teamA == null)
                return BadRequest($"Invalid Team A Id { model.TeamAId }");

            var teamB = await _teamRepository.GetByIdAsync(model.TeamBId);
            if (teamB == null)
                return BadRequest($"Invalid Team B Id { model.TeamBId }");

            var match = new Match(teamA, teamB, model.MatchDate);

            await _matchRepository.AddAsync(match);
            await _unitOfWork.Commit();

            return CreatedAtAction(nameof(GetById), new { id = match.Id }, MatchDto.FromEntity(match));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return BadRequest($"Invalid match id : { id }");

            await _matchRepository.RemoveAsync(match);
            await _unitOfWork.Commit();

            return NoContent();
        }

        [HttpPut("{id}/reschudule")]
        public async Task<IActionResult> ReschuduleMatchDate(int id, [FromBody] MatchUpdateDto model)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return BadRequest($"Invalid match id : { id }");

            var reschuduleResult = match.ReschuduleMachDate(model.MatchDate);

            if (reschuduleResult.IsFailure)
                return BadRequest(reschuduleResult.Error);

            await _matchRepository.UpdateAsync(match);
            await _unitOfWork.Commit();

            return NoContent();
        }

        [HttpPut("{id}/finish")]
        public async Task<IActionResult> SetEndResult(int id, [FromBody] FinishedMatchDto model)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                return BadRequest($"Invalid match id : { id }");

            var winner = await _teamRepository.GetByIdAsync(model.WinnerId);
            if (winner == null)
                return BadRequest($"Invalid winner Id { model.WinnerId }");

            var bestplayer = await _playerRepository.GetByIdAsync(model.BestPlayerId);
            if (bestplayer == null)
                return BadRequest($"Invalid  best player Id { model.BestPlayerId }");

            match.SetEndResult(winner, bestplayer);

            await _matchRepository.UpdateAsync(match);
            await _unitOfWork.Commit();

            return NoContent();
        }
    }
}