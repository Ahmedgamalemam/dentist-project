using dentist_model;
using dentist_api.Dtos;

namespace dentist_project.Serivce
{
    public interface IUserservice
    {
        Task<HttpResponseMessage> AddUser(UserDtos user);
        void DeleteUser(UserDtos user);
        Task<List<UserDtos>> getallusers();
        Task<UserDtos> getbyid(string id);
        void UpdateUser(UserDtos user);
        Task<UserLoginDto> Login(LoginDtos user);
    }
}
