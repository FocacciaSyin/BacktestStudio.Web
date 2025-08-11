namespace BacktestStudio.Web.Models.DTOs;

public class TechnicalIndicatorData
{
    public DateTime Date { get; set; }
    public decimal? MA5 { get; set; }
    public decimal? MA10 { get; set; }
    public decimal? MA20 { get; set; }
    public decimal? MA50 { get; set; }
}

public class TradePoint
{
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public TradeType TradeType { get; set; }
    public PositionType PositionType { get; set; }
    public int Quantity { get; set; }
    public string StrategyName { get; set; } = string.Empty;
}

public enum TradeType
{
    Buy = 1,
    Sell = 2
}

public enum PositionType
{
    Long = 1,
    Short = 2
}