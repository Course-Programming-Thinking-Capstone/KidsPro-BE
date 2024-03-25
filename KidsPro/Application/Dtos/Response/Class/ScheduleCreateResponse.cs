﻿namespace Application.Dtos.Response;

public class ScheduleCreateResponse
{
    public List<int> Days { get; set; } = new List<int>();
    public int ClassId { get; set; }
    public int Slot { get; set; }
    public TimeSpan Open { get; set; }
    public TimeSpan Close { get; set; }
    public string? Link { get; set; }
}