# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BacktestStudio.Web is a Blazor Server application for stock market strategy backtesting. The application allows users to test different investment strategies, analyze their profitability, and visualize results through interactive charts.

### Technology Stack
- **Framework**: ASP.NET Core 8.0 with Blazor Server
- **UI**: Bootstrap CSS framework
- **Target Database**: SQLite with Entity Framework Core (planned)
- **Charts**: ApexCharts.NET (planned)
- **Container**: Docker support included

## Development Commands

### Build and Run
```bash
# Build the application
dotnet build

# Run in development mode
dotnet run

# Run with HTTPS (default profile)
dotnet run --launch-profile https

# Run with HTTP only
dotnet run --launch-profile http
```

### Docker Commands
```bash
# Build Docker image
docker build -t backteststudio-web .

# Run in container
docker run -p 8080:8080 backteststudio-web
```

### Testing
```bash
# Run tests (when test projects are added)
dotnet test
```

## Architecture Overview

### Current Structure
The application follows a standard Blazor Server architecture:

- **Program.cs**: Application entry point with service registration
- **Components/**: Blazor components organized by type
  - `App.razor`: Root application component
  - `Routes.razor`: Router configuration  
  - `Layout/`: Layout components (MainLayout, NavMenu)
  - `Pages/`: Page components (Home, Counter, Weather, Error)
  - `_Imports.razor`: Global using statements

### Planned Architecture (from README.md PRD)
The application will implement a comprehensive backtesting system with:

1. **Data Layer**: MarketData, Strategy, Trade, TechnicalIndicator, StrategyResult entities
2. **Service Layer**: Market data processing, strategy execution, technical indicators, backtest engine
3. **API Layer**: Controllers for market data, strategies, trades, backtesting
4. **UI Layer**: Blazor components for dashboard, strategy management, charts, reports

### Key Planned Components
- **TradingDashboard**: Main dashboard with strategy panel and performance metrics
- **ChartComponent**: Candlestick charts with technical indicators and trade markers
- **StrategyPanel**: Strategy creation and management interface
- **PerformancePanel**: Analysis and reporting interface

## Development Patterns

### Blazor Components
- Use `@page` directive for routable components
- Place shared components in `Components/` directory
- Use `@rendermode InteractiveServer` for server-side interactivity
- Follow naming convention: PascalCase for component files

### CSS and Styling
- Bootstrap classes are available globally
- Component-specific styles use `.razor.css` files
- Global styles in `wwwroot/app.css`

### Configuration
- Development settings in `appsettings.Development.json`
- Production settings in `appsettings.json`
- Launch profiles configured for HTTP/HTTPS in `Properties/launchSettings.json`

## File Organization

### Current Structure
```
Components/
├── Layout/           # Layout components
├── Pages/           # Page components  
├── App.razor        # Root component
├── Routes.razor     # Router configuration
└── _Imports.razor   # Global imports

wwwroot/            # Static files
├── app.css         # Global styles
├── bootstrap/      # Bootstrap CSS
└── favicon.png     # Site icon
```

### Planned Expansion (based on PRD)
```
Controllers/        # API controllers (planned)
Services/          # Business logic services (planned)
Models/           # Data models (planned)
Data/            # Entity Framework context (planned)
```

## Key Development Notes

- **Language Support**: README.md is written in Traditional Chinese, indicating the target market
- **Database**: SQLite with Entity Framework Core is planned for data persistence
- **Charts**: ApexCharts.NET is planned for financial data visualization
- **Docker**: Multi-stage Dockerfile included for containerized deployment
- **Target Framework**: .NET 8.0 with C# nullable reference types enabled

## URLs and Ports
- **Development HTTP**: http://localhost:5182
- **Development HTTPS**: https://localhost:7186
- **IIS Express**: http://localhost:10069 (HTTPS: 44314)
- **Docker**: Port 8080 (internal), 8081 (HTTPS)

## Future Development Areas

Based on the PRD, upcoming development will focus on:
1. Database setup with Entity Framework Core
2. Market data import and management
3. Technical indicator calculations (Moving Averages)
4. Strategy CRUD operations  
5. Backtesting engine implementation
6. Interactive charting with trade markers
7. Performance analysis and reporting

## Task Master AI Instructions
**Import Task Master's development workflow commands and guidelines, treat as if import is in the main CLAUDE.md file.**
@./.taskmaster/CLAUDE.md
