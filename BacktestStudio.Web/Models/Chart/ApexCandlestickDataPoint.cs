namespace BacktestStudio.Web.Models.Chart;

/// <summary>
/// ApexCharts K線資料點模型
/// </summary>
public class ApexCandlestickDataPoint
{
    /// <summary>
    /// X軸時間戳（Unix毫秒時間戳）
    /// </summary>
    public long X { get; set; }

    /// <summary>
    /// Y軸OHLC價格陣列，順序為 [開盤價, 最高價, 最低價, 收盤價]
    /// </summary>
    public decimal[] Y { get; set; } = new decimal[4];
}