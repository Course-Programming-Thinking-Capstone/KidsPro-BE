namespace Application.Dtos.Response.Game
{
    public class CurrentLevelData
    {
        public int Mode { get; set; }
        public List<int>? PlayedLevel { get; set; }
    }
}