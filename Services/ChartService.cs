using BacktestStudio.Web.Models.Chart;

namespace BacktestStudio.Web.Services;

/// <summary>
/// 图表服务实现
/// </summary>
public class ChartService : IChartService
{
    private readonly IApiService _apiService;

    public ChartService(IApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// 将 K线数据转换为 ApexCharts 格式
    /// </summary>
    public ApexCandlestickSeries ConvertToApexFormat(IEnumerable<CandlestickData> data)
    {
        var apexData = data.Select(d => new ApexCandlestickDataPoint
        {
            X = new DateTimeOffset(d.Date).ToUnixTimeMilliseconds(),
            Y = new decimal[] { d.Open, d.High, d.Low, d.Close }
        }).ToArray();

        return new ApexCandlestickSeries
        {
            Data = apexData.Cast<object>().ToArray()
        };
    }
    
    /// <summary>
    /// 获取默认图表配置
    /// </summary>
    public ChartOptions GetDefaultChartOptions()
    {
        return new ChartOptions
        {
            Theme = "light",
            ShowToolbar = true,
            ShowGrid = true,
            Colors = new[] { "#00E396", "#FF4560" } // 涨绿跌红
        };
    }
    
    /// <summary>
    /// 获取模拟交易数据
    /// </summary>
    public async Task<IEnumerable<CandlestickData>> GetMockTradingDataAsync()
    {
        // 生成模拟 K线数据
        var mockData = new List<CandlestickData>();
        var random = new Random();
        var basePrice = 100m;
        var currentDate = DateTime.Now.AddDays(-30);

        for (int i = 0; i < 30; i++)
        {
            var open = basePrice + (decimal)(random.NextDouble() - 0.5) * 2;
            var close = open + (decimal)(random.NextDouble() - 0.5) * 5;
            var high = Math.Max(open, close) + (decimal)random.NextDouble() * 2;
            var low = Math.Min(open, close) - (decimal)random.NextDouble() * 2;

            mockData.Add(new CandlestickData
            {
                Date = currentDate.AddDays(i),
                Open = Math.Round(open, 2),
                High = Math.Round(high, 2),
                Low = Math.Round(low, 2),
                Close = Math.Round(close, 2),
                Volume = random.Next(10000, 100000)
            });

            basePrice = close; // 下一个交易日的基础价格
        }

        await Task.Delay(100); // 模拟异步操作
        return mockData;
    }
    
    /// <summary>
    /// 计算技术指标数据
    /// </summary>
    public object CalculateTechnicalIndicators(IEnumerable<CandlestickData> data, string indicatorType)
    {
        var dataList = data.ToList();
        
        return indicatorType.ToUpper() switch
        {
            "MA5" => CalculateMovingAverage(dataList, 5),
            "MA10" => CalculateMovingAverage(dataList, 10),
            "MA20" => CalculateMovingAverage(dataList, 20),
            "RSI" => CalculateRSI(dataList, 14),
            _ => null
        };
    }

    private List<object> CalculateMovingAverage(List<CandlestickData> data, int period)
    {
        var result = new List<object>();
        
        for (int i = period - 1; i < data.Count; i++)
        {
            var sum = 0m;
            for (int j = i - period + 1; j <= i; j++)
            {
                sum += data[j].Close;
            }
            
            var average = sum / period;
            result.Add(new
            {
                x = new DateTimeOffset(data[i].Date).ToUnixTimeMilliseconds(),
                y = Math.Round(average, 2)
            });
        }
        
        return result;
    }

    private List<object> CalculateRSI(List<CandlestickData> data, int period)
    {
        var result = new List<object>();
        
        if (data.Count < period + 1) return result;

        var gains = new List<decimal>();
        var losses = new List<decimal>();

        // 计算价格变化
        for (int i = 1; i < data.Count; i++)
        {
            var change = data[i].Close - data[i - 1].Close;
            gains.Add(change > 0 ? change : 0);
            losses.Add(change < 0 ? Math.Abs(change) : 0);
        }

        // 计算 RSI
        for (int i = period - 1; i < gains.Count; i++)
        {
            var avgGain = gains.Skip(i - period + 1).Take(period).Average();
            var avgLoss = losses.Skip(i - period + 1).Take(period).Average();
            
            var rs = avgLoss == 0 ? 100 : avgGain / avgLoss;
            var rsi = 100 - (100 / (1 + rs));
            
            result.Add(new
            {
                x = new DateTimeOffset(data[i + 1].Date).ToUnixTimeMilliseconds(),
                y = Math.Round(rsi, 2)
            });
        }
        
        return result;
    }
}
