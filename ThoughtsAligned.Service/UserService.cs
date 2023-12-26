using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.IService;
using ThoughtsAligned.Repository;
using ThoughtsAligned.Utility.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThoughtsAligned.Models.ViewModels;
using ThoughtsAligned.Utilties.Constants;
using AutoMapper;

namespace ThoughtsAligned.Service
{
    public enum UserStatus { InActive, Active, Pending, Suspended, Deleted }
    public enum UserRole { User, Admin, SuperAdmin }
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly AppSetting _appSettings;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IOptions<AppSetting> appSettings, IEmailService emailService, IMapper mapper)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task CreateSuperAdmin()
        {
            var obj = new UserClaimModel
            {
                preferred_username = HopperConstants.SuperAdminEmail,
                name = "Super Admin"
            };
            await Create(obj);
        }

        public async Task Delete(UserDto userDto)
        {
            var user = GetUserByIdWithoutStatus(userDto.Id.ToString());
            if (user == null)
                throw new AppException("User not found");
            user.Status = Convert.ToByte(UserStatus.Deleted);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserStatus(string userId, byte userStatus)
        {
            try
            {
                var user = GetUserByIdWithoutStatus(userId);
                if (user == null)
                    throw new AppException("User not found");
                user.Status = Convert.ToByte(userStatus);
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException("Could not update status.");
            }
        }

        public async Task UpdateUserRole(string userId, byte userRole)
        {
            try
            {
                var user = GetUserByIdWithoutStatus(userId);
                if (user == null)
                    throw new AppException("User not found");
                user.UserRole = userRole;
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AppException("Could not update status.");
            }
        }

        public User? GetUserByIdWithoutStatus(string id)
        {
            var userId = Guid.Parse(id);
            var user = _userRepository.Get(e => e.Id == userId);
            return user;
        }

        public UserDto? GetById(string id)
        {
            var userGuid = Guid.Parse(id);
            var user = _userRepository.Get(e => e.Id == userGuid);
            if (user != null)
                return _mapper.Map<UserDto>(user);
            else return null;
        }

        public UserDto GetByEmail(string email)
        {
            var user = _userRepository.Get(e => e.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public UserDto? GetByEmailWithoutStatus(string email)
        {
            var user= _userRepository.Get(e => e.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public IEnumerable<UserDto> GetAll()
        {
            var userList = new List<UserDto>();
            var users = _userRepository.GetAll(_ => true);
            foreach (User user in users)
            {
                userList.Add(new UserDto
                {
                    CreatedBy = user.CreatedBy,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    Id = user.Id,
                    Name = user.Name,
                    Status = user.Status,
                    UpdatedBy = user.UpdatedBy,
                    UpdateOn = user.UpdateOn,
                    UserRole = user.UserRole
                });
            }

            return userList;
        }
        public IEnumerable<UserDto> GetAllPendingUsers()
        {
            var userList = new List<UserDto>();
            var users = _userRepository.GetAll(x => x.Status == 2);
            foreach (User user in users)
            {
                userList.Add(new UserDto
                {
                    CreatedBy = user.CreatedBy,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    Id = user.Id,
                    Name = user.Name,
                    Status = user.Status,
                    UpdatedBy = user.UpdatedBy,
                    UpdateOn = user.UpdateOn,
                    UserRole = user.Status
                });
            }

            return userList;
        }

        public UserDto Authenticate(UserClaimModel model)
        {
            if (string.IsNullOrEmpty(model.preferred_username))
                return null;
            //var user = _rep.Get<User>(x => x.Email == model.Email && x.UserRole == model.UserRole);
            var user = _userRepository.GetAll(x => x.Email == model.preferred_username).FirstOrDefault();

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            //if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            //    return null;

            // authentication successful
            var userDto=_mapper.Map<UserDto>(user);
            return userDto;
        }

        public string GetToken(UserClaimModel user, UserDto userDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userDto.Id.ToString()),
                    new Claim(ClaimTypes.Thumbprint, user.aio),
                    new Claim(ClaimTypes.Upn, user.aud),
                    new Claim(ClaimTypes.Expiration, user.exp),
                    new Claim(ClaimTypes.Dns, user.iat),
                    new Claim(ClaimTypes.Dsa, user.iss),
                    new Claim(ClaimTypes.GivenName, user.name),
                    new Claim(ClaimTypes.Expired, user.nbf),
                    new Claim(ClaimTypes.Gender, userDto.UserRole.ToString()),
                    new Claim(ClaimTypes.GroupSid, userDto.Status.ToString()),
                    new Claim(ClaimTypes.Email, user.preferred_username),
                    new Claim(ClaimTypes.Uri, user.rh),
                    new Claim(ClaimTypes.Hash, user.sub),
                    new Claim(ClaimTypes.HomePhone, user.tid),
                    new Claim(ClaimTypes.Locality, user.uti),
                    new Claim(ClaimTypes.Version, user.ver)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDto> Create(UserClaimModel model)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash("randpass", out passwordHash, out passwordSalt);
            var user = new User();
            user.Id = Guid.NewGuid();
            user.Name = model.name;
            user.Email = model.preferred_username;
            user.CreatedOn = DateTime.Now;
            user.UpdateOn = DateTime.Now;
            user.Status = Convert.ToByte(UserStatus.Pending);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Status = Convert.ToByte(UserStatus.Pending);
            user.UserRole = Convert.ToByte(UserRole.User);
            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            var userDto=_mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task UpdatePassword(UserDto obj, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var model = _userRepository.Get(x => x.Id == obj.Id);
            if (model != null)
            {
                model.PasswordHash = passwordHash;
                model.PasswordSalt = passwordSalt;
                await _userRepository.SaveChangesAsync();
            }
        }

        public async Task UpdatePassword(string newpassword, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(newpassword))
                throw new AppException("Password is required");

            var user = _userRepository.Get(x => x.Id == userDto.Id);
            if (user != null)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(newpassword, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _userRepository.SaveChangesAsync();
            }
        }


        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public async Task DeleteUser(string id)
        {
            var user = _userRepository.Get(x => x.Id == Guid.Parse(id));
            if (user == null)
                throw new AppException("User not found");

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

        }

    }
}
