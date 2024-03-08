namespace Application.Dtos.Response.Game
{
    public class NumberOfLevelResponse
    {
        private List<LevelTypeCount> Values { get; set; }
    }

    public class LevelTypeCount
    {
        private int Mode { get; set; }
        private int Count { get; set; }
    }
}