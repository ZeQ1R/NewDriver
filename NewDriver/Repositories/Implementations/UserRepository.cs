using NewDriver.Data;
using NewDriver.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace NewDriver.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }


        public string AddUser(User user)

        {

            user.Submitted = null;
            user.Provimi = null;

            _context.Users.Add(user);
            _context.SaveChanges();
            return "User added successsfully";
        }

        public void UpdateUser(User user)
        {
            
            var trackedEntity = _context.Users.Local.FirstOrDefault(u => u.Id == user.Id);

            if (trackedEntity != null)
            {
                
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();

        }

        public void SubmitUser(string email,string provimi, string submitted)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                if (user.Submitted == null || !user.Submitted.Contains("Pending"))
                {
                    user.Provimi = provimi;
                    user.Submitted = submitted + "_Pending";
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("You have already submitted");
                }
            }
        }

        public void AcceptUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            if (!string.IsNullOrEmpty(user.Submitted))
            {
                user.Submitted = user.Submitted.Replace("Pending", "Accepted");
                _context.SaveChanges();
            }
        }

        public void DenyUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null && !string.IsNullOrEmpty(user.Submitted))
            {
                user.Submitted = user.Submitted.Replace("Pending", "Denied");
                _context.SaveChanges();
            }
        }

        public User GetUserByEmail(string email)
        {
            
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int id)
        {
            //return _context.Users.Find(id);
            return _context.Users
                   .AsNoTracking()
                   .FirstOrDefault(u => u.Id == id);
        }
    }
}



