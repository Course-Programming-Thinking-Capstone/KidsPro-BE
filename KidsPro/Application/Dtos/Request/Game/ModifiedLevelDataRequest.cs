namespace Application.Dtos.Request.Game
{
    public class ModifiedLevelDataRequest
    {
        public int LevelIndex { get; set; }
        public int CoinReward { get; set; }
        public int GemReward { get; set; }
        public int VStartPosition { get; set; }
        public int GameLevelTypeId { get; set; }
        public List<LevelDetailRequest> LevelDetail { get; set; } = new List<LevelDetailRequest>();
    }

    public class LevelDetailRequest
    {
        public int VPosition { get; set; }
        public int TypeId { get; set; }
    }
}