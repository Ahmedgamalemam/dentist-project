using System.Text.Json;
using System.Text;
using dentist_model;
using dentist_api.Dtos;

namespace dentist_project.Serivce
{
    public class Userservice : IUserservice
    {
        private readonly HttpClient http;
        private readonly object JsonUtil;

        public Userservice(HttpClient http)
        {
            this.http = http;
        }

        public async Task<List<UserDtos>> getallusers()
        {
            return await JsonSerializer.DeserializeAsync<List<UserDtos>>
                (await http.GetStreamAsync("/api/User"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<UserDtos> getbyid(string id)
        {
            return await JsonSerializer.DeserializeAsync<UserDtos>
                (await http.GetStreamAsync("/api/User" + id), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<HttpResponseMessage> AddUser(UserDtos user)
        {
            var newuser = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await http.PostAsync("/api/User", newuser);
            return response;
        }

        public async Task<UserLoginDto> Login(LoginDtos user)
        {
            var login = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await http.PostAsync("/api/User/password,Email", login);
            var stream = await response.Content.ReadAsStreamAsync();
            UserLoginDto t = await JsonSerializer.DeserializeAsync<UserLoginDto>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return t;
        }

        public async void UpdateUser(UserDtos user)
        {
            var newuser = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            await http.PutAsync("/api/User" + user.Id, newuser);
        }

        public async void DeleteUser(UserDtos user)
        {
            //var newuser = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            //await http.DeleteAsync("/api/User" + user.Id, newuser);
        }
    }
}
