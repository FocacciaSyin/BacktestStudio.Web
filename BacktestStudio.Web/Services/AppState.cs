using BacktestStudio.Web.Models.DTOs;
using BacktestStudio.Web.Models.ViewModels;

namespace BacktestStudio.Web.Services;

public class AppState
{
    public StrategyDto? SelectedStrategy { get; private set; }
    public DateRange TimeRange { get; private set; } = new()
    {
        Start = DateTime.Now.AddYears(-1),
        End = DateTime.Now
    };
    public List<TechnicalIndicatorType> EnabledIndicators { get; private set; } = new();
    public LoadingState LoadingState { get; private set; } = LoadingState.Idle;

    public event Action? OnChange;

    public void SetSelectedStrategy(StrategyDto strategy)
    {
        SelectedStrategy = strategy;
        NotifyStateChanged();
    }

    public void SetTimeRange(DateRange range)
    {
        TimeRange = range;
        NotifyStateChanged();
    }

    public void SetEnabledIndicators(List<TechnicalIndicatorType> indicators)
    {
        EnabledIndicators = indicators;
        NotifyStateChanged();
    }

    public void SetLoadingState(LoadingState state)
    {
        LoadingState = state;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}