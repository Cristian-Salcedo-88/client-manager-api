namespace ClientManager.Api.Application.DTO
{
    public class CreateClientDto
    {
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
