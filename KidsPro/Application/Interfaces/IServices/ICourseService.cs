﻿using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> GetByIdAsync(int id);

    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);

    Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto);

    Task<string> UpdateCoursePictureAsync(int id, IFormFile file);
}