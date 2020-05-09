using System.Linq;
using System.Threading.Tasks;
using IronSoccerDDD.Api.Dtos;
using IronSoccerDDD.Core;
using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace IronSoccerDDD.Api.Controllers
{
    public class PlayersController : ApiBaseController
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayersController(IPlayerRepository playerRepository,
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> GetAll()
        {
            var players = await _playerRepository.GetListAsync(null);

            var dtos = players.Select(x => new PlayerInListDto()
            {
                Id = x.Id,
                FirstName = x.CompleteName.FirstName,
                LastName = x.CompleteName.LastName,
                Email = x.Email,
                Phone = x.Phone,
                Team = x.Team.Name,
                BirthDate = x.BirthDate
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]PlayerCreateDto player)
        {
            var team = await _teamRepository.GetByIdAsync(player.TeamId);
            if (team == null)
                return BadRequest($"Invalid country Id { player.TeamId }");

            if (await _playerRepository.ExistAsync(p => p.Email == player.Email))
                return BadRequest($"Invalid email - It is already registered in the system");

            var completeNameResult = CompleteName.Create(player.FirstName, player.LastName);
            if (completeNameResult.IsFailure)
                return BadRequest(completeNameResult.Error);

            var emailResult = Email.Create(player.Email);
            if (emailResult.IsFailure)
                return BadRequest(emailResult.Error);

            var newPlayer = new Player(completeNameResult.Value, player.BirthDate, emailResult.Value, player.Phone, team);

            await _playerRepository.AddAsync(newPlayer);

            await _unitOfWork.Commit();
            return Ok();
        }

    }
}