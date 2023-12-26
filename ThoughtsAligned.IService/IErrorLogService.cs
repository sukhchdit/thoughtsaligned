using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Models.ViewModels;

namespace ThoughtsAligned.IService
{
    public interface IErrorLogService
    {
        Task<ErrorLogDto?> CreateErrorLogAsync(ErrorLogDto errorLogDto);
        Task<string> DeleteErrorLogs(string startDate, string endDate);
        IEnumerable<ErrorLogDto>? GetErrorLogs(string startDate, string endDate);
        List<ErrorLogFile>? GetAllErrorFiles(string startDate, string endDate);
        FileStream? DownloadErrorLogFile(string fileName);
    }
}
