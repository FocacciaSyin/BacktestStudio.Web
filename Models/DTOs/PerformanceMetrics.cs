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

/// <summary>
/// 日期範圍
/// </summary>
public class DateRange
{
    /// <summary>
    /// 開始日期
    /// </summary>
    public DateTime Start { get; set; }
    
    /// <summary>
    /// 結束日期
    /// </summary>
    public DateTime End { get; set; }
}