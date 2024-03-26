﻿using Application.Dtos.Response.StudentSchedule;

namespace Application.Dtos.Response;

public class ClassResponse
{
    public int ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? CourseName { get; set; }
    public int TotalStudent { get; set; }
    public string? TeacherName { get; set; }
    public string? OpenClass { get; set; }
    public string? CloseClass { get; set; }
    public int Duration { get; set; }
    public int SlotTime { get; set; }
    public List<int> StudyDay = new List<int>();
    public int SlotNumber { get; set; }
    public TimeSpan StartSlot { get; set; }
    public TimeSpan EndSlot { get; set; }
    public int TotalSlot { get; set; }
    public string? RoomUrl { get; set; }
    public List<StudentClassResponse> Students = new List<StudentClassResponse>();

}