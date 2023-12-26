using ThoughtsAligned.IService;
using ThoughtsAligned.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ThoughtsAligned.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    [Authorize]
    public class FileInfoController : ControllerBase
    {
        private IFileInfoService _fileInfoService;

        public FileInfoController(IFileInfoService fileInfoService)
        {
            _fileInfoService = fileInfoService;
        }

        [HttpPost]
        [Route("CreateFilePath")]
        public async Task<IActionResult> CreateFileInfo(FileInformationDto fileInfo)
        {
            var model = await _fileInfoService.Create(fileInfo);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetAllFilePaths")]
        public IActionResult GetAllFilePaths()
        {
            var list = _fileInfoService.GetAll();
            return Ok(list);
        }

        [HttpGet]
        [Route("GetFilePathById")]
        public IActionResult GetFilePathById(string id)
        {
            var list = _fileInfoService.GetFileInfoById(id);
            return Ok(list);
        }

        [HttpDelete]
        [Route("DeleteFilePath")]
        public IActionResult DeleteFilePath(string id)
        {
            var model = _fileInfoService.DeleteFileInfo(id);
            return Ok(model);
        }
    }
}
