using BacktestStudio.Web.Models.DTOs;

namespace BacktestStudio.Web.Models.Chart;

/// <summary>
/// K線圖資料模型
/// </summary>
public class CandlestickData
{
    /// <summary>
    /// 交易日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 開盤價
    /// </summary>
    public decimal Open { get; set; }

    /// <summary>
    /// 最高價
    /// </summary>
    public decimal High { get; set; }

    /// <summary>
    /// 最低價
    /// </summary>
    public decimal Low { get; set; }

    /// <summary>
    /// 收盤價
    /// </summary>
    public decimal Close { get; set; }

    /// <summary>
    /// 成交量
    /// </summary>
    public long Volume { get; set; }
}