
using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace RiskClassifier;

internal static class Program
{
	private static async Task<int> Main(string[] args)
	{
		string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
			Console.WriteLine("The OPENAI_API_KEY environment variable is missing.");
			return 1;
        }

		ChatClient chatClient = new ("gpt-4o", apiKey);

        string instructions = """
            You are a risk assessment agent.

            Your job is to estimate exactly two scores for a discovered system vulnerability:
            Likelihood and Impact.

            Ask the fewest practical questions needed. Prefer direct questions.

            For Likelihood, focus only on:
            - whether exploitation is technically possible
            - whether the attacker needs special access, timing, credentials, or user interaction
            - whether the vulnerability is internet-facing or internal-only
            - whether there is evidence of active or likely exploitation

            For Impact, focus only on:
            - what an attacker could see, change, delete, or trigger
            - whether sensitive data, payments, account changes, or service availability are affected
            - how many users or systems could be affected
            - whether the impact is limited to one session/account or broader

            Do not ask about remediation.
            Do not ask about root causes.
            Do not ask about general mitigations unless they directly change the Likelihood or Impact score.

            If the user's first description already gives enough information, classify the risk.
            Otherwise, ask one direct question at a time.
            Ask no more than four follow-up questions before making your best assessment.

            Use these scores:

            Likelihood:
            1 = Very Unlikely
            2 = Unlikely
            3 = Possible
            4 = Likely
            5 = Very Likely

            Impact:
            1 = Negligible
            2 = Minor
            3 = Moderate
            4 = Significant
            5 = Severe

            When you have enough information:
            1. Choose the Likelihood score.
            2. Choose the Impact score.
            3. Call the classify_risk tool with those two scores.
            4. Use the tool result as the final risk level.

            Do not invent the final risk level yourself. Use the tool result.

            After calling the tool, respond in exactly this format:

            FINAL RISK ASSESSMENT

            Likelihood: <number> - <label>
            Impact: <number> - <label>
            Risk level: <tool result>

            Summary:
            <brief explanation>
            """;

        AITool classifyRiskTool = AIFunctionFactory.Create(
            (Func<int, int, string>)ClassifyRisk,
            name: "classify_risk",
            description: "Classifies the risk level based on Likelihood and Impact scores.");

        ChatClientAgent agent = chatClient.AsAIAgent(
			name: "Risk Assessment Agent",
			instructions: instructions,
            tools: [classifyRiskTool]);

        AgentSession session = await agent.CreateSessionAsync();

        Console.WriteLine("Welcome to the Risk Assessment Agent!");
        Console.WriteLine("Type /quit to exit.");
        Console.WriteLine();
        Console.WriteLine("Describe the vulnerability.");
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("You: ");
            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                continue;
            }

            if (userInput.Trim().Equals("/quit", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            AgentResponse response = await agent.RunAsync(userInput, session);

            Console.WriteLine();
            Console.WriteLine("Agent:");
            Console.WriteLine(response.Text);
            Console.WriteLine();

            if (response.Text.Contains("FINAL ASSESSMENT", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
        }
    }

    private static string ClassifyRisk(
        [Description("Likelihood score from 1 to 5. 1 is Very Unlikely, 5 is Very Likely.")]
        int likelihood,

        [Description("Impact score from 1 to 5. 1 is Negligible, 5 is Severe.")]
        int impact)
    {
        //Console.WriteLine($"Classifying risk with Likelihood={likelihood}, Impact={impact}...");
        RiskLevel riskLevel = Classifier.ClassifyRisk(likelihood, impact);

        return riskLevel switch
        {
            RiskLevel.Low => "Low risk",
            RiskLevel.LowMedium => "Low-medium risk",
            RiskLevel.Medium => "Medium risk",
            RiskLevel.MediumHigh => "Medium-high risk",
            RiskLevel.High => "High risk",
            _ => riskLevel.ToString()
        };
    }
}
