namespace BacktestStudio.Web.Models.Chart;

/// <summary>
/// ApexCharts K線圖資料序列格式
/// </summary>
public class ApexCandlestickSeries
{
    /// <summary>
    /// 序列名稱，用於圖表圖例顯示
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 圖表資料陣列，包含K線資料點
    /// </summary>
    public object[] Data { get; set; } = Array.Empty<object>();
}