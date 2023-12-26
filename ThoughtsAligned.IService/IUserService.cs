using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Models.ViewModels;

namespace ThoughtsAligned.IService
{
    public interface IUserService
    {
        UserDto? GetById(string id);

        UserDto? GetByEmail(string email);

        UserDto? GetByEmailWithoutStatus(string email);

        IEnumerable<UserDto> GetAll();

        UserDto Authenticate(UserClaimModel model);

        string GetToken(UserClaimModel user, UserDto userDto);

        Task<UserDto> Create(UserClaimModel model);

        Task UpdatePassword(string newpassword, UserDto user);

        Task UpdatePassword(UserDto obj, string password);

        Task Delete(UserDto obj);

        Task CreateSuperAdmin();

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        Task UpdateUserStatus(string userId, byte userStatus);

        Task DeleteUser(string id);

        Task UpdateUserRole(string userId, byte userRole);

        IEnumerable<UserDto> GetAllPendingUsers();
    }
}
