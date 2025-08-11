namespace BacktestStudio.Web.Models.Chart;

/// <summary>
/// 圖表配置選項模型
/// </summary>
public class ChartOptions
{
    /// <summary>
    /// 圖表主題，支援 "light" 或 "dark"
    /// </summary>
    public string Theme { get; set; } = "light";

    /// <summary>
    /// 是否顯示圖表工具列
    /// </summary>
    public bool ShowToolbar { get; set; } = true;

    /// <summary>
    /// 是否顯示圖表網格線
    /// </summary>
    public bool ShowGrid { get; set; } = true;

    /// <summary>
    /// 圖表顏色配置陣列，預設為綠色（上漲）和紅色（下跌）
    /// </summary>
    public string[] Colors { get; set; } = { "#00E396", "#FF4560" };
}