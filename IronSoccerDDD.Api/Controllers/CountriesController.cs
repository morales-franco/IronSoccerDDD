using System.Linq;
using System.Threading.Tasks;
using IronSoccerDDD.Api.Dtos;
using IronSoccerDDD.Core;
using IronSoccerDDD.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace IronSoccerDDD.Api.Controllers
{
    public class CountriesController : ApiBaseController
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IActionResult> GetAll()
        {
            var countries = await _countryRepository.GetListAsync(null);

            var dtos = countries.Select(x => new CountryInListDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(dtos);
        }
    }
}