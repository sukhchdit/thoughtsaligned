using AutoMapper;
using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.IService;
using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Repository;

namespace ThoughtsAligned.Service
{
    public class FileInfoService : IFileInfoService
    {
        private readonly IRepository<FileInformation> _fileInformationRepository;
        private readonly IMapper _mapper;

        public FileInfoService(IRepository<FileInformation> fileInformationRepository, IMapper mapper)
        {
            _fileInformationRepository = fileInformationRepository;
            _mapper = mapper;
        }

        public async Task<FileInformationDto> Create(FileInformationDto fileInfoDto)
        {
            try
            {
                var fileInfo = _mapper.Map<FileInformation>(fileInfoDto);
                if (fileInfo.Id != Guid.Empty)
                {
                    fileInfo = _fileInformationRepository.Get(x => x.Id == fileInfoDto.Id);
                    if(fileInfo != null) 
                    _fileInformationRepository.Update(fileInfo);
                }
                else
                {
                    
                    fileInfo.Id = Guid.NewGuid();
                    fileInfo.Status = true;
                    _fileInformationRepository.Add(fileInfo);
                }
                await _fileInformationRepository.SaveChangesAsync();

                return _mapper.Map<FileInformationDto>(fileInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<FileInformationDto>? GetAll()
        {
            try
            {
                var fileInfos = _fileInformationRepository.GetAll(x => x.Status == true);
                return _mapper.Map<IEnumerable<FileInformationDto>>(fileInfos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileInformationDto? GetFileInfoById(string id)
        {
            try
            {
                var fileInfo= _fileInformationRepository.Get(x => x.Id == Guid.Parse(id));
                return _mapper.Map<FileInformationDto>(fileInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteFileInfo(string id)
        {
            try
            {
                var model = _fileInformationRepository.Get(x => x.Id == (new Guid(id)));
                if (model != null)
                {
                    model.Status = false;
                    _fileInformationRepository.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
