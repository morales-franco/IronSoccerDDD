using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using IronSoccerDDD.Api.Dtos;
using IronSoccerDDD.Core;
using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace IronSoccerDDD.Api.Controllers
{
    public class TeamsController : ApiBaseController
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TeamsController(ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            ICountryRepository countryRepository,
            IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> GetAll()
        {
            var players = await _teamRepository.GetListAsync(null);

            var dtos = players.Select(x => new TeamInListDto()
            {
                Id = x.Id,
                Country = x.Country.Name,
                Name = x.Name
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TeamCreateDto team)
        {
            var country = await _countryRepository.GetByIdAsync(team.CountryId);

            if (country == null)
                return BadRequest($"Invalid country Id { team.CountryId }");

            var newTeam = new Team(team.Name, country);

            await _teamRepository.AddAsync(newTeam);

            await _unitOfWork.Commit();
            return Ok();
        }

        [HttpPost]
        [Route("{id}/players")]
        public async Task<IActionResult> Join(int id, [FromBody] int playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            var team = await _teamRepository.GetByIdAsync(id);

            if (player == null)
                return BadRequest($"Invalid player Id { playerId }");

            if (team == null)
                return BadRequest($"Invalid team Id { id }");

            var joinPlayerResult = team.JoinPlayer(player);

            if (joinPlayerResult.IsFailure)
                return BadRequest(joinPlayerResult.Error);

            await _teamRepository.UpdateAsync(team);

            await _unitOfWork.Commit();
            return Ok();
        }

       

    }
}