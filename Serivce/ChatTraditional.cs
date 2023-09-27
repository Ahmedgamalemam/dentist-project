using dentist_api.Dtos;
using dentist_model;
using System.Text.Json;
using System.Text;

namespace dentist_project.Serivce
{
    public class ChatTraditional : IChatTraditional
    {
        private readonly HttpClient http;

        public ChatTraditional(HttpClient http)
        {
            this.http = http;
        }

        public async Task<List<UserDtos>> GetallMessage(string id)
        {
            return await JsonSerializer.DeserializeAsync<List<UserDtos>>
                (await http.GetStreamAsync("/api/User" + id), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async void AddMessage(Chat mess)
        {
            var newuser = new StringContent(JsonSerializer.Serialize(mess), Encoding.UTF8, "application/json");
            var response = await http.PostAsync("/api/User", newuser);
            var stream = await response.Content.ReadAsStreamAsync();
            UserLoginDto t = await JsonSerializer.DeserializeAsync<UserLoginDto>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async void UpdateUser(Chat mess)
        {
            var newuser = new StringContent(JsonSerializer.Serialize(mess), Encoding.UTF8, "application/json");
            await http.PutAsync("/api/User", newuser);
        }
    }
}
