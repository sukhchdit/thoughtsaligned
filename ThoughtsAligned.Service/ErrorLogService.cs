using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.IService;
using ThoughtsAligned.Repository;
using System.Text;
using ThoughtsAligned.Models.ViewModels;
using AutoMapper;
using ThoughtsAligned.Models.Dto;

namespace ThoughtsAligned.Service
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IRepository<ErrorLog> _errorLogRepository;
        private readonly IMapper _mapper;

        public ErrorLogService(IRepository<ErrorLog> errorLogRepository, IMapper mapper)
        {
            _errorLogRepository = errorLogRepository;
            _mapper = mapper;
        }
        public async Task<ErrorLogDto?> CreateErrorLogAsync(ErrorLogDto errorLogDto)
        {
            try
            {
                var errorLog=_mapper.Map<ErrorLog>(errorLogDto);
                _errorLogRepository.Add(errorLog);
                await _errorLogRepository.SaveChangesAsync();
                errorLogDto=_mapper.Map<ErrorLogDto>(errorLog);
                return errorLogDto;
            }
            catch (Exception ex)
            {
                CreateErrorLogFile(ex);
                return null;
            }
        }
        public async Task<string> DeleteErrorLogs(string startDate, string endDate)
        {
            try
            {
                var startDt = Convert.ToDateTime(startDate);
                var endDt = Convert.ToDateTime(endDate);
                var errorLogs = _errorLogRepository.GetAll(x => x.CreatedOn != null && x.CreatedOn.Value.Date >= startDt && x.CreatedOn.Value.Date <= endDt);
                
                foreach (ErrorLog errorLog in errorLogs)
                {
                    _errorLogRepository.Delete(errorLog);
                }
                
                await _errorLogRepository.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                CreateErrorLogFile(ex);
                return "Error";
            }
        }
        public IEnumerable<ErrorLogDto>? GetErrorLogs(string startDate, string endDate)
        {
            try
            {
                var startDt = Convert.ToDateTime(startDate);
                var endDt = Convert.ToDateTime(endDate);
                var logs = _errorLogRepository.GetAll(x => x.CreatedOn != null && x.CreatedOn.Value.Date >= startDt && x.CreatedOn.Value.Date <= endDt);
                var errorDtos = _mapper.Map<IEnumerable<ErrorLog>, IEnumerable<ErrorLogDto>>(logs);
                return errorDtos;
            }
            catch (Exception ex)
            {
                CreateErrorLogFile(ex);
                return null;
            }
        }

        public List<ErrorLogFile>? GetAllErrorFiles(string startDate, string endDate)
        {
            try
            {
                var fileList = new List<ErrorLogFile>();
                var startDt = Convert.ToDateTime(startDate);
                var endDt = Convert.ToDateTime(endDate);
                while (startDt <= endDt)
                {
                    var dirPath = Path.Combine(Directory.GetCurrentDirectory(), ("Logs/Log-" + long.Parse(startDt.ToString("yyyyMMdd"))));
                    if (Directory.Exists(dirPath))
                    {
                        var fileListArray = Directory.GetFiles(dirPath);
                        if (fileListArray != null)
                        {
                            for (int x = 0; x < fileListArray.Length; x++)
                            {
                                var file = fileListArray[x].Substring(fileListArray[x].LastIndexOf("\\") + 1);
                                var dt = file.Substring(4);
                                var fileDate = new DateTime(Convert.ToInt32(dt.Substring(0, 4)), Convert.ToInt32(dt.Substring(4, 2)), Convert.ToInt32(dt.Substring(6, 2)), Convert.ToInt32(dt.Substring(8, 2)), Convert.ToInt32(dt.Substring(10, 2)), Convert.ToInt32(dt.Substring(12, 2)));
                                fileList.Add(new ErrorLogFile { FileName = file, FileDate = fileDate });
                            }
                        }
                    }
                    startDt = startDt.AddDays(1);
                }
                return fileList;
            }
            catch (Exception ex)
            {
                CreateErrorLogFile(ex);
                return null;
            }
        }

        public FileStream? DownloadErrorLogFile(string fileName)
        {
            try
            {

                var pathToSaveFile = Path.Combine(Directory.GetCurrentDirectory(), "Logs/Log-" + long.Parse(DateTime.Now.ToString("yyyyMMdd")));
                if (!Directory.Exists(pathToSaveFile))
                    return null;
                var file = pathToSaveFile + "/" + fileName;

                if (System.IO.File.Exists(file))
                {
                    return new FileStream(file, FileMode.Open, FileAccess.Read);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                CreateErrorLogFile(ex);
                return null;
            }
        }

        private void CreateErrorLogFile(Exception ex)
        {
            string fileData = "ErrorMessage = " + ex.Message +
                                ",\\n InnerException = " + ex.InnerException?.Message +
                                ",\\n Source = " + ex.Source +
                                ",\\n StackTrace = " + ex.StackTrace;
            byte[] bytes = Encoding.ASCII.GetBytes(fileData);
            var dirName = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            var pathToSaveFile = Path.Combine(Directory.GetCurrentDirectory(), ("Logs/Log-" + long.Parse(DateTime.Now.ToString("yyyyMMdd"))));
            if (!Directory.Exists(pathToSaveFile))
                Directory.CreateDirectory(pathToSaveFile);


            File.WriteAllBytes(pathToSaveFile + "/Log-" + long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")) + ".txt", bytes);
        }

    }
}
