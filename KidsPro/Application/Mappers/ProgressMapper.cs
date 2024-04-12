using Application.Dtos.Response.StudentProgress;
using Domain.Entities;

namespace Application.Mappers;

public static class ProgressMapper
{
    public static List<SectionProgressResponse> StudentToProgressResponseList(List<StudentProgress> dto)
    {
        var result = new List<SectionProgressResponse>();
        // Nhóm các đối tượng StudentProgress theo courseId
        var groupedByCourse = dto.GroupBy(sp => sp.CourseId)
            .Select(group => group.ToList()).ToList();

        // Convert từng group 
        foreach (var courseGroup in groupedByCourse)
            result.Add(StudentToProgressResponse(courseGroup));
        
        return result;
    }

    public static SectionProgressResponse StudentToProgressResponse(List<StudentProgress> dto)
    {
        var response = new SectionProgressResponse()
        {
            StudentId = dto.FirstOrDefault()!.StudentId,
            StudentName = dto.FirstOrDefault()!.Student.Account.FullName,
            CourseName = dto.FirstOrDefault()!.Course.Name,
            TeacherName = dto.FirstOrDefault()!.Course.ModifiedBy!.FullName,
            CourseId = dto.FirstOrDefault()!.CourseId,
            CourseImage = dto.FirstOrDefault()!.Course.PictureUrl
        };

        foreach (var x in dto)
        {
            //float ratioSection = 0;
            float lessonCompletedRatio = 0;
            float quizRatio = 0;
            //check to see have any lessons been completed
            if (x.Section.Lessons.Any(s => s.StudentLessons?.Count > 0))
            {
                float lessonTotal = x.Section.Lessons.Count;

                float lessonRatio = 80 / lessonTotal;

                //Check to see how many lessons have been completed
                var completeLessons = x.Section.Lessons
                    .Select(l => l.StudentLessons?.Select(s => s.IsCompleted));

                quizRatio = x.Section.Quizzes.FirstOrDefault()?.StudentQuizzes.FirstOrDefault()?.IsPass == true
                    ? 20
                    : 0;

                lessonCompletedRatio = lessonRatio * completeLessons.ToList().Count;
            }

            var progress = new SectionProgress()
            {
                SectionId = x.SectionId,
                SectionName = x.Section.Name,
                Progress = quizRatio + lessonCompletedRatio,
            };
            response.SectionProgress.Add(progress);
        }

        response.CourseProgress = CalculateCourseProgress(response.SectionProgress);

        return response;
    }

    private static float CalculateCourseProgress(List<SectionProgress> sections)
    {
        float result = 0;
        float sectionRatio = 1 / (float)sections.Count;
        foreach (var x in sections)
        {
            result += sectionRatio * x.Progress;
        }

        return result;
    }
}