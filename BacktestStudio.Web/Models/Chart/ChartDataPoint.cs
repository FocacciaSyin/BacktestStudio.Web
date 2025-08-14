namespace BacktestStudio.Web.Models.Chart;

/// <summary>
/// 統一的圖表數據點模型
/// </summary>
public class ChartDataPoint
{
    /// <summary>
    /// X軸數值（通常為時間戳）
    /// </summary>
    public decimal X { get; set; }

    /// <summary>
    /// Y軸數值（價格或指標值）
    /// </summary>
    public decimal Y { get; set; }
}