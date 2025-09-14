namespace RestWithASPNET10Erudio.Data.DTO.V2
{
    public class PersonDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool Enabled { get; set; }
    }
}
