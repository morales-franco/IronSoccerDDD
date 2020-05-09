using System;

namespace IronSoccerDDD.Api.Dtos
{
    public class MatchInListDto
    {
        public int Id { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public DateTime MatchDate { get; set; }
        public string Winner { get; set; }
        public string BestPlayer { get; set; }
    }
}
