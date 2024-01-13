using Application.Dtos.Request.Teacher;
using Application.Dtos.Response.Teacher;
using Application.ErrorHandlers;
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
        readonly ITeacherService _teacher;
        readonly IMapper _map;

        public TeacherController(ITeacherContactService teacherContact, ITeacherResourceService teacherResource,
            ITeacherProfileService teacherProfile, ITeacherService teacher, IMapper map)
        {
            _teacherContact = teacherContact;
            _teacherResource = teacherResource;
            _teacherProfile = teacherProfile;
            _teacher = teacher;
            _map = map;
        }


        /// <summary>
        /// Get teacher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teacher = await _teacher.GetTeacherById(id);
            var result = _map.Map<TeacherResponse>(teacher);
            return Ok(result);
        }
        /// <summary>
        /// Update description Teacher
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("description")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> UpdateTeacher(TeacherRequest request)
        {
            var result= await _teacher.UpdateTeacher(request);
            return Ok(result);
        }

        /// <summary>
        /// Create or Update Teacher Contact Information 
        /// </summary>
        /// <param name="request">Có 3 type, PhoneNumber, Gmail, Facebook, Ghi đúng như vậy</param>
        /// <param name="type">1.Create, 2.Update</param>
        /// <returns></returns>
        [HttpPut("contact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> CreateOrUpdate(TeacherContactRequest request, TeacherRequestType type)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> CreateOrUpdate(TeacherProfileRequest request, TeacherRequestType type)
        {
            await _teacherProfile.CreateOrUpdate(type, request);
            return Ok();
        }
        /// <summary>
        /// Create or Update Teacher Resource
        /// </summary>
        /// <param name="request">Có 2 type, Avatar, Picture, Ghi đúng như vậy</param>
        /// <param name="type">1.Create, 2.Update</param>   
        /// <returns></returns>
        [HttpPut("resource")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> CreateOrUpdate(TeacherResourceRequest request, TeacherRequestType type)
        {
            await _teacherResource.CreateOrUpdate(type, request);
            return Ok();
        }
    }
}