namespace BacktestStudio.Web.Models.DTOs;

public class PerformanceMetrics
{
    public decimal TotalProfit { get; set; }
    public decimal TotalReturn { get; set; }
    public int TotalTrades { get; set; }
    public decimal WinRate { get; set; }
    public decimal MaxDrawdown { get; set; }
    public decimal SharpeRatio { get; set; }
    public decimal AverageProfit { get; set; }
    public int WinningTrades { get; set; }
    public int LosingTrades { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class ChartData
{
    public List<MarketDataPoint> PriceData { get; set; } = new();
    public List<TechnicalIndicatorData> IndicatorData { get; set; } = new();
    public List<TradePoint> TradeMarkers { get; set; } = new();
}

public class DateRange
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}