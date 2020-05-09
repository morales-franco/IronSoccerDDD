using System;

namespace IronSoccerDDD.Api.Dtos
{
    public class PlayerInListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Team { get; set; }
    }
}
