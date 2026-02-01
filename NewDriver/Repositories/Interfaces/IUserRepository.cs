using NewDriver.Data;

namespace NewDriver.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUserByEmail(string email);

        string AddUser(User user);

        void DeleteUser(int id);
        void UpdateUser(User user);

        void SubmitUser(string email, string provimi, string submitted);


        void AcceptUser(int id);

        void DenyUser(int id);
        User GetUserById(int id);
    }
}


