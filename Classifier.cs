namespace RiskClassifier;

public static class Classifier
{
    // Risk assessment matrix:
    //
    // | Likelihood \ Impact | Negligible | Minor     | Moderate  | Significant | Severe     |
    // |---------------------|------------|-----------|-----------|-------------|------------|
    // | Very Likely         | LowMedium  | Medium    | MediumHigh| High        | High       |
    // | Likely              | Low        | LowMedium | Medium    | MediumHigh  | High       |
    // | Possible            | Low        | LowMedium | Medium    | MediumHigh  | MediumHigh |
    // | Unlikely            | Low        | LowMedium | LowMedium | Medium      | MediumHigh |
    // | Very Unlikely       | Low        | Low       | LowMedium | Medium      | Medium     |
    public static RiskLevel ClassifyRisk(Likelihood likelihood, Impact impact)
    {
        return (likelihood, impact) switch
        {
            // Very Unlikely
            (Likelihood.VeryUnlikely, Impact.Negligible) => RiskLevel.Low,
            (Likelihood.VeryUnlikely, Impact.Minor) => RiskLevel.Low,
            (Likelihood.VeryUnlikely, Impact.Moderate) => RiskLevel.LowMedium,
            (Likelihood.VeryUnlikely, Impact.Significant) => RiskLevel.Medium,
            (Likelihood.VeryUnlikely, Impact.Severe) => RiskLevel.Medium,

            // Unlikely
            (Likelihood.Unlikely, Impact.Negligible) => RiskLevel.Low,
            (Likelihood.Unlikely, Impact.Minor) => RiskLevel.LowMedium,
            (Likelihood.Unlikely, Impact.Moderate) => RiskLevel.LowMedium,
            (Likelihood.Unlikely, Impact.Significant) => RiskLevel.Medium,
            (Likelihood.Unlikely, Impact.Severe) => RiskLevel.MediumHigh,

            // Possible
            (Likelihood.Possible, Impact.Negligible) => RiskLevel.Low,
            (Likelihood.Possible, Impact.Minor) => RiskLevel.LowMedium,
            (Likelihood.Possible, Impact.Moderate) => RiskLevel.Medium,
            (Likelihood.Possible, Impact.Significant) => RiskLevel.MediumHigh,
            (Likelihood.Possible, Impact.Severe) => RiskLevel.MediumHigh,

            // Likely
            (Likelihood.Likely, Impact.Negligible) => RiskLevel.Low,
            (Likelihood.Likely, Impact.Minor) => RiskLevel.LowMedium,
            (Likelihood.Likely, Impact.Moderate) => RiskLevel.Medium,
            (Likelihood.Likely, Impact.Significant) => RiskLevel.MediumHigh,
            (Likelihood.Likely, Impact.Severe) => RiskLevel.High,

            // Very Likely
            (Likelihood.VeryLikely, Impact.Negligible) => RiskLevel.LowMedium,
            (Likelihood.VeryLikely, Impact.Minor) => RiskLevel.Medium,
            (Likelihood.VeryLikely, Impact.Moderate) => RiskLevel.MediumHigh,
            (Likelihood.VeryLikely, Impact.Significant) => RiskLevel.High,
            (Likelihood.VeryLikely, Impact.Severe) => RiskLevel.High,

            _ => throw new ArgumentOutOfRangeException(
                $"Unsupported likelihood/impact combination: {likelihood}, {impact}")
        };
    }

    public static RiskLevel ClassifyRisk(int likelihoodValue, int impactValue)
    {
        if (!Enum.IsDefined(typeof(Likelihood), likelihoodValue))
        {
            throw new ArgumentOutOfRangeException(nameof(likelihoodValue));
        }
        if (!Enum.IsDefined(typeof(Impact), impactValue))
        {
            throw new ArgumentOutOfRangeException(nameof(impactValue));
        }
        Likelihood likelihood = (Likelihood)likelihoodValue;
        Impact impact = (Impact)impactValue;
        return ClassifyRisk(likelihood, impact);
    }
}