namespace Application.Dtos.Response.Game
{
    public class LevelInformationResponse
    {
        public int CoinReward { get; set; }
        public int GameReward { get; set; }
        public int VStartPosition { get; set; }
        public List<LevelPositionData> levelDetail { get; set; }
    }

    public class LevelPositionData
    {
        public int VPosition { get; set; }
        public string TypeName { get; set; } = null!;
    }
}