
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly List<Client> clientList;

        public ClientRepository()
        {
            clientList = [
                new Client(3, "A.J. Kwak", "user3@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA="),
                new Client(3, "Donald. Kwak", "user2@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA="),
                new Client(3, "Ernesto. Kwak", "user1@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")
            ];
        }

        public Client? Get(string email)
        {
            Client? client = clientList.FirstOrDefault(c => c.EmailAddress.Equals(email));
            return client;
        }

        public Client? Get(int id)
        {
            Client? client = clientList.FirstOrDefault(c => c.Id == id);
            return client;
        }

        public List<Client> GetAll()
        {
            return clientList;
        }
    }
}
