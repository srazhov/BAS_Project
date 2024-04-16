using BAS_Project.DTOs;
using BAS_Project.Helpers;
using BAS_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace BAS_Project.Controllers
{
    [ApiController]
    [Route("api/process")]
    public class FileManagementController : ControllerBase
    {
        private IFileProcessingService Service { get; set; }

        public FileManagementController(IFileProcessingService service)
        {
            Service = service;
        }

        [HttpPost]
        public IActionResult Post(RequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var messageId = Service.EnqueueMessage(request.Message);

            var message = Service.WriteMessage(messageId);

            return Ok(new ResponseDTO(
                DateHelpers.GetISO_8601String(message.StartTime),
                DateHelpers.GetISO_8601String(message.WriteTime),
                (message.WriteTime - message.StartTime).Milliseconds,
                message.Message));
        }
    }
}

