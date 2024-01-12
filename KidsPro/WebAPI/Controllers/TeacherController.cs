using Application.Dtos.Request.Teacher;
using Application.Interfaces.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/teachers")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        readonly ITeacherContactService _teacherContact;
        readonly ITeacherResourceService _teacherResource;
        readonly ITeacherProfileService _teacherProfile;

        public TeacherController(ITeacherContactService teacherContact, ITeacherResourceService teacherResource, ITeacherProfileService teacherProfile)
        {
            _teacherContact = teacherContact;
            _teacherResource = teacherResource;
            _teacherProfile = teacherProfile;
        }

        /// <summary>
        /// Create or Update Teacher Contact Information 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type">1.Create, 2.Update</param>
        /// <returns></returns>
        [HttpPut("contact")]
        public async Task<IActionResult> CreateOrUpdate( TeacherContactRequest request, TeacherRequestType type)
        {
            await _teacherContact.CreateOrUpdate(type, request);
            return Ok();
        }
        /// <summary>
        /// Create or Update Teacher Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type">1.Create, 2.Update</param>
        /// <returns></returns>
        [HttpPut("profile")]
        public async Task<IActionResult> CreateOrUpdate(TeacherProfileRequest request, TeacherRequestType type)
        {
            await _teacherProfile.CreateOrUpdate(type, request);
            return Ok();
        }
        /// <summary>
        /// Create or Update Teacher Resource
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type">1.Create, 2.Update</param>
        /// <returns></returns>
        [HttpPut("resource")]
        public async Task<IActionResult> CreateOrUpdate(TeacherResourceRequest request, TeacherRequestType type)
        {
            await _teacherResource.CreateOrUpdate(type, request);
            return Ok();
        }
    }
}