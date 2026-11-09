using PodHrebenem.Shared.Services;
using Xunit;

namespace PodHrebenem.Tests;

public class PriceCalculatorTests
{
    private readonly PriceCalculator _calc = new();

    [Fact]
    public void Missing_dates_give_empty_estimate()
    {
        Assert.Equal(EstimateStatus.Empty, _calc.Calculate(null, null).Status);
        Assert.Equal(EstimateStatus.Empty, _calc.Calculate(new DateOnly(2026, 5, 1), null).Status);
        Assert.Equal(EstimateStatus.Empty, _calc.Calculate(new DateOnly(2026, 5, 1), new DateOnly(2026, 5, 1)).Status);
    }

    [Fact]
    public void Departure_before_arrival_is_invalid()
    {
        var r = _calc.Calculate(new DateOnly(2026, 5, 10), new DateOnly(2026, 5, 8));
        Assert.Equal(EstimateStatus.InvalidRange, r.Status);
    }

    [Fact]
    public void One_night_is_below_minimum()
    {
        var r = _calc.Calculate(new DateOnly(2026, 5, 10), new DateOnly(2026, 5, 11));
        Assert.Equal(EstimateStatus.BelowMinimum, r.Status);
    }

    [Fact]
    public void Off_season_stay_uses_low_rate_plus_cleaning()
    {
        // 2 noci v květnu: 2 × 4 500 + 1 500 úklid
        var r = _calc.Calculate(new DateOnly(2026, 5, 10), new DateOnly(2026, 5, 12));
        Assert.Equal(EstimateStatus.Ok, r.Status);
        Assert.Equal(2, r.Nights);
        Assert.Equal(10_500m, r.Total);
    }

    [Fact]
    public void High_season_stay_uses_high_rate()
    {
        // 3 noci v lednu: 3 × 6 500 + 1 500
        var r = _calc.Calculate(new DateOnly(2027, 1, 10), new DateOnly(2027, 1, 13));
        Assert.Equal(21_000m, r.Total);
    }

    [Fact]
    public void Stay_across_season_boundary_prices_each_night_by_its_own_month()
    {
        // noc 31.3. patří do vysoké sezóny (březen), noc 1.4. do nízké (duben)
        var r = _calc.Calculate(new DateOnly(2026, 3, 31), new DateOnly(2026, 4, 2));
        Assert.Equal(6_500m + 4_500m + 1_500m, r.Total);
    }

    [Fact]
    public void Stay_across_year_end_works()
    {
        // 30.12. a 31.12. a 1.1.: všechno vysoká sezóna
        var r = _calc.Calculate(new DateOnly(2026, 12, 30), new DateOnly(2027, 1, 2));
        Assert.Equal(3, r.Nights);
        Assert.Equal(3 * 6_500m + 1_500m, r.Total);
    }

    [Fact]
    public void Custom_options_are_respected()
    {
        var calc = new PriceCalculator(new PriceOptions { LowRate = 1000, HighRate = 2000, Cleaning = 0, MinNights = 3, HighMonths = new[] { 7 } });
        Assert.Equal(EstimateStatus.BelowMinimum, calc.Calculate(new DateOnly(2026, 7, 1), new DateOnly(2026, 7, 3)).Status);
        Assert.Equal(6_000m, calc.Calculate(new DateOnly(2026, 7, 1), new DateOnly(2026, 7, 4)).Total);
    }
}
