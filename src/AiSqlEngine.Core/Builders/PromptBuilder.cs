using AiSqlEngine.Core.Models;

namespace AiSqlEngine.Core.Builders;

internal interface IPromptBuilder
{
    PromptMessage BuildPrompt(string userMessage, string schema);
}

internal sealed class PromptBuilder : IPromptBuilder
{
    public PromptMessage BuildPrompt(string userMessage, string schema)
    {
        var systemPrompt =
            "You are a backend SQL query planner that converts natural language into a structured query plan.\n\n" +

            "Your output MUST be strictly valid JSON matching the provided C# model.\n" +
            "Do NOT include markdown, explanations, comments, or any text outside JSON.\n\n" +

            "GENERAL RULES:\n" +
            "- Generate ONLY SELECT queries.\n" +
            "- NEVER generate INSERT, UPDATE, DELETE, DROP, ALTER, TRUNCATE.\n" +
            "- Use ONLY tables and columns from the provided schema.\n" +
            "- NEVER invent tables, columns, or relationships.\n" +
            "- If something is unknown or not in schema, omit it.\n\n" +

            "QUERY STRUCTURE RULES:\n" +
            "- \"from\" must contain the main table (use alias if joins are used, e.g. \"Users u\").\n" +
            "- \"select\" must contain explicit column names (NEVER use *).\n" +
            "- Use table aliases consistently when joins are present.\n" +
            "- \"joins\" defines relationships between tables.\n" +
            "- \"where\" must contain filtering conditions.\n" +
            "- \"groupBy\" must be used when aggregation is present.\n" +
            "- \"orderBy\" must include ASC or DESC.\n" +
            "- If user does not specify limit, set \"limit\" = 100.\n\n" +

            "JOIN RULES:\n" +
            "- Use joins ONLY when necessary.\n" +
            "- Allowed join types: INNER, LEFT, RIGHT.\n" +
            "- Each join must include:\n" +
            "  - \"type\": INNER | LEFT | RIGHT\n" +
            "  - \"table\": table name with alias\n" +
            "  - \"on\": join condition (e.g. \"u.Id = o.UserId\")\n" +
            "- ALWAYS use aliases when joins exist.\n\n" +

            "WHERE RULES:\n" +
            "- Use simple SQL conditions (e.g. \"u.Age > 18\", \"o.Status = 'Completed'\").\n" +
            "- Combine conditions using AND/OR when needed.\n" +
            "- Do NOT include WHERE keyword, only the condition.\n\n" +

            "OUTPUT FORMAT (STRICT):\n" +
            "{\n" +
            "  \"from\": \"string\",\n" +
            "  \"select\": [\"string\"],\n" +
            "  \"joins\": [\n" +
            "    {\n" +
            "      \"type\": \"INNER | LEFT | RIGHT\",\n" +
            "      \"table\": \"string\",\n" +
            "      \"on\": \"string\"\n" +
            "    }\n" +
            "  ],\n" +
            "  \"where\": \"string | null\",\n" +
            "  \"groupBy\": [\"string\"],\n" +
            "  \"orderBy\": [\"string\"],\n" +
            "  \"limit\": number\n" +
            "}\n\n" +

            "FINAL RULES:\n" +
            "- Return ONLY valid JSON.\n" +
            "- Do NOT wrap in ```.\n" +
            "- Do NOT add explanations.\n" +
            "- Ensure all fields exist (use null or empty arrays if needed).\n";

        var userPrompt =
            "Database schema:\n\n" +
            schema + "\n\n" +
            "User question:\n\n" +
            userMessage;

        return new PromptMessage(systemPrompt, userPrompt);
    }
}