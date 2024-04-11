using ModelLayer.Registration;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IRegistration
    {
        public Task<bool> RegisterUser(UserRegistrationModel userRegistrationModel);
        public Task<string> UserLogin(UserLoginModel userLogin);
        public Task<int> UpdatePassword(string mailid, string password);

        public Task<Registration> GetByEmailAsync(string Email);
        public Task<IEnumerable<Registration>> GetUserDetails();
        public Task DeleteUser(string firstname);
        public Task UpdateUser(string firstname, string lastname, string email, string password);
    }
}
