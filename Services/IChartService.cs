using BacktestStudio.Web.Models.Chart;
using BacktestStudio.Web.Models.DTOs;
using BacktestStudio.Web.Models.ViewModels;
using ChartData = BacktestStudio.Web.Models.DTOs.ChartData;
using DateRange = BacktestStudio.Web.Models.DTOs.DateRange;
using TechnicalIndicatorType = BacktestStudio.Web.Models.ViewModels.TechnicalIndicatorType;

namespace BacktestStudio.Web.Services;

/// <summary>
/// 图表服务接口
/// </summary>
public interface IChartService
{
    /// <summary>
    /// 将 K线数据转换为 ApexCharts 格式
    /// </summary>
    ApexCandlestickSeries ConvertToApexFormat(IEnumerable<CandlestickData> data);
    
    /// <summary>
    /// 获取默认图表配置
    /// </summary>
    ChartOptions GetDefaultChartOptions();
    
    /// <summary>
    /// 获取模拟交易数据
    /// </summary>
    Task<IEnumerable<CandlestickData>> GetMockTradingDataAsync();
    
    /// <summary>
    /// 计算技术指标数据
    /// </summary>
    object CalculateTechnicalIndicators(IEnumerable<CandlestickData> data, string indicatorType);
    
    /// <summary>
    /// 准备图表数据
    /// </summary>
    Task<ChartData> PrepareChartDataAsync(DateRange range, int? strategyId = null);
    
    /// <summary>
    /// 将市场数据转换为 ApexChart 格式
    /// </summary>
    List<object> ConvertToApexChartData(List<MarketDataPoint> data);
    
    /// <summary>
    /// 将技术指标转换为 ApexChart 格式
    /// </summary>
    List<object> ConvertIndicatorsToApexData(List<TechnicalIndicatorData> indicators, TechnicalIndicatorType type);
    
    /// <summary>
    /// 将交易点转换为注释格式
    /// </summary>
    List<object> ConvertTradesToAnnotations(List<TradePoint> trades);
}
