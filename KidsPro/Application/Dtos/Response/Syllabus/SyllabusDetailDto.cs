namespace Application.Dtos.Response.Syllabus;

public class SyllabusDetailDto
{
    public int Id { get; set; }

    public string Name { get; init; } = null!;

    public string? Target { get; init; } = null!;

    public int TotalSlot { get; init; }

    public int SlotTime { get; init; }

    public int? MinQuizScoreRatio { get; init; }

    public List<SyllabusSectionDto>? Sections { get; init; }

    public int? TeacherId { get; init; }
}