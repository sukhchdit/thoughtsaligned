using AutoMapper;
using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.Models.Dto;

namespace ThoughtsAligned.Utility.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ErrorLog, ErrorLogDto>();
            CreateMap<ErrorLogDto, ErrorLog>();

            CreateMap<FileInformation, FileInformationDto>();
            CreateMap<FileInformationDto, FileInformation>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

        }
    }
}
