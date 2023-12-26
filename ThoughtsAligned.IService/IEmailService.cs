using ThoughtsAligned.Models.Dto;

namespace ThoughtsAligned.IService
{
    public interface IEmailService
    {
        Task NewUserEmail(UserDto model);
    }
}
