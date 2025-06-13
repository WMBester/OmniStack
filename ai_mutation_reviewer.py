import os
import json
import openai
import argparse
from pathlib import Path

# --- Argument Parsing ---
parser = argparse.ArgumentParser(description="AI Mutation Reviewer")
parser.add_argument("--json", required=True, help="Path to mutation-report.json")
parser.add_argument("--output", required=True, help="Path to output .md file")
parser.add_argument("--test_dir", default="WMB.Api.Tests", help="Directory where test classes are located")
args = parser.parse_args()

# --- Load API Key from ENV ---
api_key = os.getenv("OPENAI_API_KEY")
if not api_key:
    raise ValueError("OPENAI_API_KEY environment variable not set")
openai.api_key = api_key

# --- Load Mutation Report ---
with open(args.json, encoding="utf-8") as f:
    report = json.load(f)

# --- Collect all survived mutants ---
survived_mutants = []
for file_path, file_data in report["files"].items():
    for mutant in file_data["mutants"]:
        if mutant["status"] == "Survived":
            survived_mutants.append({
                "sourceFilePath": file_path,
                "mutatorName": mutant["mutatorName"],
                "replacement": mutant["replacement"],
                "originalLines": mutant["originalLines"],
                "location": mutant["location"]
            })

# --- Load all test files into memory ---
test_files = {}
for test_path in Path(args.test_dir).rglob("*.cs"):
    with open(test_path, encoding="utf-8") as tf:
        test_files[str(test_path)] = tf.read()

# --- AI Prompt Template ---
def build_prompt(mutant, test_code_snippets):
    return f"""
A mutation survived in this C# codebase.

🔹 Mutation Type: {mutant['mutatorName']}
🔹 Original Code:
{''.join(mutant['originalLines'])}
🔹 Replaced With:
{mutant['replacement']}

🔍 Context: These are the current test classes:

{test_code_snippets}

🧠 Task:
Analyze why the mutation was not killed.
- Suggest how to improve the unit test to catch this mutant.
- If necessary, suggest a production code improvement to eliminate false positives.
- If possible, return a sample NUnit test method.

Respond in **Markdown** with section headers.
"""

# --- Run AI Review ---
def run_review():
    report_lines = ["# 🧠 AI Mutation Review Report\n"]
    for idx, mutant in enumerate(survived_mutants):
        print(f"🔍 Reviewing mutant {idx + 1} / {len(survived_mutants)}")

        # Concatenate a few test files (limit for prompt size)
        test_code_combined = "\n\n".join(list(test_files.values())[:3])

        prompt = build_prompt(mutant, test_code_combined)

        response = openai.ChatCompletion.create(
            model="gpt-4",
            messages=[
                {"role": "system", "content": "You are a senior C# test engineer."},
                {"role": "user", "content": prompt}
            ],
            temperature=0.4,
            max_tokens=800
        )

        suggestion = response["choices"][0]["message"]["content"]

        # Print review to pipeline logs
        print("\n🧠 AI Suggestion for Mutant:")
        print(f"📄 File: {mutant['sourceFilePath']}")
        print(f"🔄 Mutation: {mutant['mutatorName']}")
        print(f"🧬 Original:\n{''.join(mutant['originalLines'])}")
        print(f"💥 Replacement:\n{mutant['replacement']}")
        print("✍️ Suggestion:\n")
        print(suggestion)

        # Save to markdown report
        report_lines.append(f"## 📄 {mutant['sourceFilePath']} - {mutant['mutatorName']}")
        report_lines.append(f"**Original Code:**\n```csharp\n{''.join(mutant['originalLines'])}\n```\n")
        report_lines.append(f"**Replacement:**\n```csharp\n{mutant['replacement']}\n```\n")
        report_lines.append(suggestion)
        report_lines.append("\n---\n")

    with open(args.output, "w", encoding="utf-8") as f:
        f.write("\n".join(report_lines))
    print(f"✅ Report written to {args.output}")

# --- Execute ---
if __name__ == "__main__":
    run_review()
