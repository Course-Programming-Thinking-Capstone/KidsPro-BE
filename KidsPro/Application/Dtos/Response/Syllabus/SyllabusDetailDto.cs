namespace Application.Dtos.Response.Syllabus;

public class SyllabusDetailDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Target { get; set; } = null!;

    public int TotalSlot { get; set; }

    public int SlotPerWeek { get; set; }
    public int SlotTime { get; set; }

    public int? MinQuizScoreRatio { get; set; }

    public List<SyllabusSectionDto>? Sections { get; set; }

    public int? TeacherId { get; set; }
    public int? CourseId { get; set; }
}