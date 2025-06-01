namespace TravelMate.Core.Entities
{
    // 天气数据类（不存储到数据库，仅用于API响应）
    public class Weather
    {
        public float MaxTemperature { get; set; }
        public float MinTemperature { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Wind { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
