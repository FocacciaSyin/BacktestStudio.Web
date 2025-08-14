namespace BacktestStudio.Web.Models.DTOs;

public class StrategyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public StrategyType StrategyType { get; set; }
    public decimal EntryPrice { get; set; }
    public decimal StopLoss { get; set; }
    public decimal TakeProfit { get; set; }
    public decimal PositionSize { get; set; }
}

public enum StrategyType
{
    Long = 1,
    Short = 2
}

public class StrategyCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public StrategyType StrategyType { get; set; }
    public decimal EntryPrice { get; set; }
    public decimal StopLoss { get; set; }
    public decimal TakeProfit { get; set; }
    public decimal PositionSize { get; set; }
}

public class StrategyUpdateDto : StrategyCreateDto
{
    public bool IsActive { get; set; }
}