using dentist_model;

namespace dentist_project.Serivce
{
    public interface IChatTraditional
    {
        Task<dentist_model.Chat> AddMessage(Chat mess);
        Task<List<UserDtos>> GetallMessage(string id);
        void UpdateUser(Chat mess);
    }
}
