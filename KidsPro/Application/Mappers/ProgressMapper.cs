using Application.Dtos.Response.StudentProgress;
using Domain.Entities;

namespace Application.Mappers;

public static class ProgressMapper
{
    public static SectionProgressResponse StudentToProgressResponse(List<StudentProgress> dto)
    {
        var response = new SectionProgressResponse()
        {
            StudentId = dto.FirstOrDefault()!.Student.Id,
            StudentName = dto.FirstOrDefault()!.Student.Account.FullName,
            CourseName = dto.FirstOrDefault()!.Course.Name
        };

        foreach (var x in dto)
        {
            float ratioSection = 0;
            float ratioLessonCompleted = 0;
            float ratioQuiz = 0;
            //check to see have any lessons been completed
            if (x.Section.Lessons.Any(s=>s.StudentLessons?.Count>0))
            {
                float totalLesson = x.Section.Lessons.Count;

                float ratioLesson = 80 / totalLesson;

                //Check to see how many lessons have been completed
                var completeLessons = x.Section.Lessons
                    .Select(l => l.StudentLessons?.Select(s => s.IsCompleted));

                ratioQuiz = x.Section.Quizzes.FirstOrDefault()?.StudentQuizzes.FirstOrDefault()?.IsPass == true
                    ? 20
                    : 0;

                ratioLessonCompleted = ratioLesson * completeLessons.ToList().Count;
            }

            var progress = new ProgressResponse()
            {
                SectionId = x.SectionId,
                SectionName = x.Section.Name,
                Progress = ratioSection - ratioQuiz - ratioLessonCompleted,
            };
            response.Progress.Add(progress);
        }

        return response;
    }
}