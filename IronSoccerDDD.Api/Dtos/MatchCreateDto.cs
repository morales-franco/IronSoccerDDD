using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IronSoccerDDD.Api.Dtos
{
    public class MatchCreateDto
    {
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public DateTime MatchDate { get; set; }
    }
}
