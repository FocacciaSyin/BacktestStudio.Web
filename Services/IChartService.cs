using BacktestStudio.Web.Models.DTOs;
using BacktestStudio.Web.Models.ViewModels;

namespace BacktestStudio.Web.Services;

public interface IChartService
{
    Task<ChartData> PrepareChartDataAsync(DateRange range, int? strategyId = null);
    List<object> ConvertToApexChartData(List<MarketDataPoint> data);
    List<object> ConvertIndicatorsToApexData(List<TechnicalIndicatorData> indicators, TechnicalIndicatorType type);
    List<object> ConvertTradesToAnnotations(List<TradePoint> trades);
}

public class ChartService : IChartService
{
    private readonly IApiService _apiService;

    public ChartService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<ChartData> PrepareChartDataAsync(DateRange range, int? strategyId = null)
    {
        var chartData = new ChartData();

        // Get market data
        chartData.PriceData = await _apiService.GetMarketDataAsync(range);

        // Get technical indicators
        chartData.IndicatorData = await _apiService.GetTechnicalIndicatorsAsync(range);

        // Get trade markers if strategy is specified
        if (strategyId.HasValue)
        {
            chartData.TradeMarkers = await _apiService.GetTradePointsAsync(strategyId.Value, range);
        }

        return chartData;
    }

    public List<object> ConvertToApexChartData(List<MarketDataPoint> data)
    {
        return data.Select(item => new object[]
        {
            new DateTimeOffset(item.Date).ToUnixTimeMilliseconds(),
            new[] { item.Open, item.High, item.Low, item.Close }
        }).Cast<object>().ToList();
    }

    public List<object> ConvertIndicatorsToApexData(List<TechnicalIndicatorData> indicators, TechnicalIndicatorType type)
    {
        return indicators.Select(item =>
        {
            decimal? value = type switch
            {
                TechnicalIndicatorType.MA5 => item.MA5,
                TechnicalIndicatorType.MA10 => item.MA10,
                TechnicalIndicatorType.MA20 => item.MA20,
                TechnicalIndicatorType.MA50 => item.MA50,
                _ => null
            };

            return new object[]
            {
                new DateTimeOffset(item.Date).ToUnixTimeMilliseconds(),
                value
            };
        }).Cast<object>().ToList();
    }

    public List<object> ConvertTradesToAnnotations(List<TradePoint> trades)
    {
        return trades.Select(trade => new
        {
            x = new DateTimeOffset(trade.Date).ToUnixTimeMilliseconds(),
            y = trade.Price,
            marker = new
            {
                size = 6,
                fillColor = trade.TradeType == TradeType.Buy ? "#00E396" : "#FF4560",
                strokeColor = "#fff",
                strokeWidth = 2
            },
            label = new
            {
                text = trade.TradeType == TradeType.Buy ? "B" : "S",
                style = new
                {
                    color = "#fff",
                    fontSize = "10px"
                }
            }
        }).Cast<object>().ToList();
    }
}
