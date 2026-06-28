# Risk Classifier

Risk Classifier is a small C# console sample for exploring Microsoft's Agent
Framework. It talks with the user about a discovered system vulnerability,
estimates likelihood and impact scores, calls a local risk-matrix tool, and
prints a final risk assessment.

> [!WARNING]
> This is an experimental learning project and should not be considered
> production-ready.

The project was created to take first steps with Microsoft Agent Framework in a
plain local console application. It uses OpenAI GPT-4o directly and does not use
Azure, Azure OpenAI, or Agent Foundry.

## What It Does

The console app starts an agent-backed conversation about a vulnerability. The
agent asks direct follow-up questions until it can estimate two values:

- likelihood of exploitation, from 1 to 5
- impact if exploited, from 1 to 5

After choosing those scores, the agent calls the local `classify_risk` tool. The
tool delegates to the deterministic risk matrix in `Classifier.cs` and returns
one of these risk levels:

- Low risk
- Low-medium risk
- Medium risk
- Medium-high risk
- High risk

The final response includes the likelihood score, impact score, classified risk
level, and a short summary.

## Requirements

- .NET 8 SDK.
- PowerShell on Windows.
- An `OPENAI_API_KEY` environment variable for OpenAI model calls.

## Setup

Restore the NuGet packages from the repository root:

```powershell
dotnet restore
```

The project depends on:

- `Microsoft.Agents.AI`
- `Microsoft.Agents.AI.OpenAI`

## Running

Run the console app from the repository root:

```powershell
dotnet run
```

The app will ask you to describe a vulnerability. You can exit with:

```text
/quit
```

Example interaction:

```text
Welcome to the Risk Assessment Agent!
Type /quit to exit.

Describe the vulnerability.

You:
An expired authentication cookie still allows a customer to use the account portal.

Agent:
Does exploitation require access to the customer's already logged-in device or session?
```

When enough information has been gathered, the agent calls the local classifier
tool and prints a final risk assessment.

## Development Checks

Build the project with:

```powershell
dotnet build
```

There are currently no automated tests. The simplest smoke test is to run the
console app, describe a vulnerability, answer the agent's questions, and confirm
that a final risk assessment is printed.

## Project Structure

```text
Program.cs              Agent setup, console loop, OpenAI connection, and tool wiring
Classifier.cs           Deterministic likelihood/impact risk matrix
Models.cs               Likelihood, impact, and risk-level enums
RiskClassifier.csproj   .NET project file and package references
```

## Notes

This project is a first-use learning exercise for Microsoft Agent Framework, not
a general-purpose vulnerability assessment product. The risk matrix is local and
deterministic, but the questions, chosen scores, and final wording are
model-driven and can vary between runs.

The local tool has no side effects. It only converts the agent's likelihood and
impact scores into a risk level. OpenAI API calls may incur usage costs.

## Third-Party Notices

This project has direct runtime dependencies on Microsoft Agent Framework
packages and their transitive dependencies, including the OpenAI .NET client.
See each NuGet package's license metadata for full license and notice terms.

## License

GNU General Public License v3.0. See the `LICENSE` file for details.
