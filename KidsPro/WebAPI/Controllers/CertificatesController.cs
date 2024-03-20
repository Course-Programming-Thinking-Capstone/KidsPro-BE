using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/certificate")]
    public class CertificatesController : ControllerBase
    {
        ICertificateService _service;

        public CertificatesController(ICertificateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tạo certificate khi student hoàn thành course
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> AddCertificateAsync(CertificatesRequest dto)
        {
            await _service.AddCertificateAsync(dto);
            return Ok();
        }

    }
}
