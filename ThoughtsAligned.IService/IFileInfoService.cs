using ThoughtsAligned.Models.Dto;

namespace ThoughtsAligned.IService
{
    public interface IFileInfoService
    {
        Task<FileInformationDto> Create(FileInformationDto fileInfo);
        IEnumerable<FileInformationDto>? GetAll();
        FileInformationDto? GetFileInfoById(string id);
        bool DeleteFileInfo(string id);
    }
}
