using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IronSoccerDDD.Api.Dtos
{
    public class TeamInListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
