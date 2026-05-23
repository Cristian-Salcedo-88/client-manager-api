namespace ClientManager.Domain.AggregateModel
{
    public class Client
    {
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public Client() { }

        public Client(string name, string identificationNumber, string phone, string address)
        {
            Name = name;
            IdentificationNumber = identificationNumber;
            Phone = phone;
            Address = address;
        }
    }
}
