namespace RiskClassifier;

public enum Likelihood
{
    VeryUnlikely = 1,
    Unlikely = 2,
    Possible = 3,
    Likely = 4,
    VeryLikely = 5,
}

public enum Impact
{
    Negligible = 1,
    Minor = 2,
    Moderate = 3,
    Significant = 4,
    Severe = 5,
}

public enum RiskLevel
{
    Low = 1,
    LowMedium = 2,
    Medium = 3,
    MediumHigh = 4,
    High = 5,
}