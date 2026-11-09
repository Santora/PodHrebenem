namespace PodHrebenem.Shared.Services;

/// <summary>Ukázkové ceny. Nahradit skutečnými, shodnými s ceníkem na webu.</summary>
public sealed class PriceOptions
{
    public decimal LowRate { get; init; } = 4500m;
    public decimal HighRate { get; init; } = 6500m;
    public decimal Cleaning { get; init; } = 1500m;
    public int MinNights { get; init; } = 2;

    /// <summary>Měsíce vysoké sezóny (1 až 12).</summary>
    public IReadOnlyList<int> HighMonths { get; init; } = new[] { 12, 1, 2, 3, 7, 8 };
}

public enum EstimateStatus
{
    Empty,
    InvalidRange,
    BelowMinimum,
    Ok
}

public sealed record PriceEstimate(EstimateStatus Status, int Nights, decimal Total);

/// <summary>Orientační kalkulace ceny pobytu, odpovídá výpočtu v main.js na webu.</summary>
public sealed class PriceCalculator
{
    public PriceCalculator(PriceOptions? options = null) => Options = options ?? new PriceOptions();

    public PriceOptions Options { get; }

    public PriceEstimate Calculate(DateOnly? from, DateOnly? to)
    {
        if (from is null || to is null)
            return new PriceEstimate(EstimateStatus.Empty, 0, 0);

        var nights = to.Value.DayNumber - from.Value.DayNumber;
        if (nights == 0) return new PriceEstimate(EstimateStatus.Empty, 0, 0);
        if (nights < 0) return new PriceEstimate(EstimateStatus.InvalidRange, nights, 0);
        if (nights < Options.MinNights) return new PriceEstimate(EstimateStatus.BelowMinimum, nights, 0);

        decimal total = 0;
        var day = from.Value;
        for (var i = 0; i < nights; i++)
        {
            total += Options.HighMonths.Contains(day.Month) ? Options.HighRate : Options.LowRate;
            day = day.AddDays(1);
        }

        return new PriceEstimate(EstimateStatus.Ok, nights, total + Options.Cleaning);
    }
}
