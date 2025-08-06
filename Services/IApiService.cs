using BacktestStudio.Web.Models.DTOs;

namespace BacktestStudio.Web.Services;

public interface IApiService
{
    Task<List<StrategyDto>> GetStrategiesAsync();
    Task<StrategyDto> CreateStrategyAsync(StrategyCreateDto dto);
    Task<StrategyDto> UpdateStrategyAsync(int id, StrategyUpdateDto dto);
    Task DeleteStrategyAsync(int id);
    Task<List<MarketDataPoint>> GetMarketDataAsync(DateRange range);
    Task<List<TechnicalIndicatorData>> GetTechnicalIndicatorsAsync(DateRange range);
    Task<List<TradePoint>> GetTradePointsAsync(int strategyId, DateRange range);
    Task<PerformanceMetrics> GetPerformanceMetricsAsync(int strategyId);
}

public class MockApiService : IApiService
{
    public async Task<List<StrategyDto>> GetStrategiesAsync()
    {
        // Mock data for development
        await Task.Delay(500); // Simulate API delay
        return new List<StrategyDto>
        {
            new() { Id = 1, Name = "長線策略", Description = "基於移動平均線的長線投資策略", IsActive = true, CreatedDate = DateTime.Now.AddDays(-7), StrategyType = StrategyType.Long },
            new() { Id = 2, Name = "短線策略", Description = "快速進出的短線交易策略", IsActive = false, CreatedDate = DateTime.Now.AddDays(-3), StrategyType = StrategyType.Short },
            new() { Id = 3, Name = "均值回歸策略", Description = "基於價格均值回歸的策略", IsActive = true, CreatedDate = DateTime.Now.AddDays(-1), StrategyType = StrategyType.Long }
        };
    }

    public async Task<StrategyDto> CreateStrategyAsync(StrategyCreateDto dto)
    {
        await Task.Delay(300);
        return new StrategyDto
        {
            Id = Random.Shared.Next(100, 999),
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true,
            CreatedDate = DateTime.Now,
            StrategyType = dto.StrategyType,
            EntryPrice = dto.EntryPrice,
            StopLoss = dto.StopLoss,
            TakeProfit = dto.TakeProfit,
            PositionSize = dto.PositionSize
        };
    }

    public async Task<StrategyDto> UpdateStrategyAsync(int id, StrategyUpdateDto dto)
    {
        await Task.Delay(300);
        return new StrategyDto
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.Now.AddDays(-5),
            StrategyType = dto.StrategyType,
            EntryPrice = dto.EntryPrice,
            StopLoss = dto.StopLoss,
            TakeProfit = dto.TakeProfit,
            PositionSize = dto.PositionSize
        };
    }

    public async Task DeleteStrategyAsync(int id)
    {
        await Task.Delay(200);
    }

    public async Task<List<MarketDataPoint>> GetMarketDataAsync(DateRange range)
    {
        await Task.Delay(800);
        var data = new List<MarketDataPoint>();
        var random = new Random();
        var currentPrice = 15000m;
        
        for (var date = range.StartDate; date <= range.EndDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                continue;
                
            var change = (decimal)(random.NextDouble() - 0.5) * 200;
            var open = currentPrice;
            var close = open + change;
            var high = Math.Max(open, close) + (decimal)random.NextDouble() * 100;
            var low = Math.Min(open, close) - (decimal)random.NextDouble() * 100;
            
            data.Add(new MarketDataPoint
            {
                Date = date,
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = random.Next(1000000, 5000000)
            });
            
            currentPrice = close;
        }
        
        return data;
    }

    public async Task<List<TechnicalIndicatorData>> GetTechnicalIndicatorsAsync(DateRange range)
    {
        await Task.Delay(400);
        var marketData = await GetMarketDataAsync(range);
        var indicators = new List<TechnicalIndicatorData>();
        
        for (int i = 0; i < marketData.Count; i++)
        {
            var indicator = new TechnicalIndicatorData
            {
                Date = marketData[i].Date
            };
            
            // Calculate MA5
            if (i >= 4)
            {
                indicator.MA5 = marketData.Skip(i - 4).Take(5).Average(x => x.Close);
            }
            
            // Calculate MA10
            if (i >= 9)
            {
                indicator.MA10 = marketData.Skip(i - 9).Take(10).Average(x => x.Close);
            }
            
            // Calculate MA20
            if (i >= 19)
            {
                indicator.MA20 = marketData.Skip(i - 19).Take(20).Average(x => x.Close);
            }
            
            // Calculate MA50
            if (i >= 49)
            {
                indicator.MA50 = marketData.Skip(i - 49).Take(50).Average(x => x.Close);
            }
            
            indicators.Add(indicator);
        }
        
        return indicators;
    }

    public async Task<List<TradePoint>> GetTradePointsAsync(int strategyId, DateRange range)
    {
        await Task.Delay(300);
        var random = new Random();
        var trades = new List<TradePoint>();
        var strategies = await GetStrategiesAsync();
        var strategy = strategies.FirstOrDefault(s => s.Id == strategyId);
        
        if (strategy == null) return trades;
        
        var currentDate = range.StartDate.AddDays(random.Next(10, 50));
        
        while (currentDate <= range.EndDate.AddDays(-10))
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                trades.Add(new TradePoint
                {
                    Date = currentDate,
                    Price = 14000 + (decimal)random.NextDouble() * 2000,
                    TradeType = TradeType.Buy,
                    PositionType = strategy.StrategyType == StrategyType.Long ? PositionType.Long : PositionType.Short,
                    Quantity = random.Next(100, 1000),
                    StrategyName = strategy.Name
                });
                
                // Add corresponding sell trade
                var sellDate = currentDate.AddDays(random.Next(1, 15));
                if (sellDate <= range.EndDate)
                {
                    trades.Add(new TradePoint
                    {
                        Date = sellDate,
                        Price = trades.Last().Price + (decimal)(random.NextDouble() - 0.5) * 500,
                        TradeType = TradeType.Sell,
                        PositionType = strategy.StrategyType == StrategyType.Long ? PositionType.Long : PositionType.Short,
                        Quantity = trades.Last().Quantity,
                        StrategyName = strategy.Name
                    });
                }
            }
            
            currentDate = currentDate.AddDays(random.Next(20, 60));
        }
        
        return trades;
    }

    public async Task<PerformanceMetrics> GetPerformanceMetricsAsync(int strategyId)
    {
        await Task.Delay(400);
        var random = new Random();
        
        return new PerformanceMetrics
        {
            TotalProfit = (decimal)(random.NextDouble() - 0.3) * 100000,
            TotalReturn = (decimal)(random.NextDouble() - 0.2) * 50,
            TotalTrades = random.Next(10, 100),
            WinRate = (decimal)random.NextDouble() * 100,
            MaxDrawdown = (decimal)random.NextDouble() * 20,
            SharpeRatio = (decimal)(random.NextDouble() * 2),
            AverageProfit = (decimal)(random.NextDouble() - 0.3) * 5000,
            WinningTrades = random.Next(5, 60),
            LosingTrades = random.Next(5, 40),
            LastUpdated = DateTime.Now
        };
    }
}