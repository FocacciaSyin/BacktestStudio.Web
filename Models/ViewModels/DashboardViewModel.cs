using BacktestStudio.Web.Models.DTOs;

namespace BacktestStudio.Web.Models.ViewModels;

public class DashboardViewModel
{
    public List<StrategyDto> Strategies { get; set; } = new();
    public StrategyDto? SelectedStrategy { get; set; }
    public ChartData ChartData { get; set; } = new();
    public PerformanceMetrics? Performance { get; set; }
    public DateRange TimeRange { get; set; } = new()
    {
        StartDate = DateTime.Now.AddYears(-1),
        EndDate = DateTime.Now
    };
    public LoadingState LoadingState { get; set; } = LoadingState.Idle;
    public string? ErrorMessage { get; set; }
    public List<TechnicalIndicatorType> EnabledIndicators { get; set; } = new();
}

public enum LoadingState
{
    Idle,
    Loading,
    Success,
    Error
}

public enum TechnicalIndicatorType
{
    MA5,
    MA10,
    MA20,
    MA50
}