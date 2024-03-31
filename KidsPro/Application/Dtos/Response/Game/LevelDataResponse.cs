namespace Application.Dtos.Response.Game
{
    public class LevelDataResponse
    {
        public int Id { get; set; }
        public int LevelIndex { get; set; }
        public int CoinReward { get; set; }
        public int GemReward { get; set; }
        public int VStartPosition { get; set; }
        public int GameLevelTypeId { get; set; }
        public string GameLevelTypeName { get; set; }
        public List<LevelDetail> LevelDetail { get; set; } = new();
    }

    public class LevelDetail
    {
        public int VPosition { get; set; }
        public int TypeId { get; set; }
    }
}