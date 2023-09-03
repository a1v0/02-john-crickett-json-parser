﻿/*
 * Wanted to make a test suite with these tests but couldn't get xUnit to work:
 * - returns empty object if string is "{}"
 * - returns empty object if string is "{           }"
 * - ignores white space and line breaks
 * - parses string key/value pairs
 *   - since JSON gets sent as a string, any quotation marks inside will be escaped. Therefore, how do I differentiate between the quotation marks that enclose a key and any ones escaped within that key?
 *   - check what it looks like when JSON arrives
 * - parses numbers (incl. floats)
 * - parses boolean values
 * - parses arrays
 *   - what should I parse this as? Always as an untyped collection or rather do I validate that it could work as a typed array and conditionally create an array on that basis?
 * - parses nested objects/numbers/strings/arrays/booleans
 * - parses objects
 * - parses null type
 * - ignores quotation marks and brackets when escaped and in a string
 * - throws errors:
 *   - no quotes on key or string value (or single quotes)
 *   - incorrectly escaped strings
 *   - unrecognised values (including incorrectly cased NULL/TRUE etc.)
 *   - missing comma
 *   - missing closing bracket
 */

// ParseJSON("   ");
// ParseJSON("   {");
ParseJSON("   {}");

static Dictionary<string, dynamic> ParseJSON(string rawJSON)
{
    string errorMessage = "Input is not valid JSON.";
    Exception invalidJSON = new Exception(errorMessage);

    string trimmedJSON = rawJSON.Trim();

    if (trimmedJSON.Length == 0) throw invalidJSON;
    if (trimmedJSON[0] is not '{') throw invalidJSON;

    Dictionary<string, dynamic> parsedJSON = new Dictionary<string, dynamic>();

    Dictionary<char, short> charCounter = GetCharCounter();

    LoopThroughInput(rawJSON, parsedJSON, charCounter);

    FinalBracketsCheck(charCounter);
    Console.WriteLine(CharCounterToString(charCounter));
    Console.WriteLine(ParsedJSONObjectToString(parsedJSON));

    return parsedJSON;
}

static void LoopThroughInput(string rawJSON, Dictionary<string, dynamic> parsedJSON, Dictionary<char, short> charCounter)
{
    string closingCharacters = "]}";

    foreach (char c in rawJSON)
    {
        if (charCounter.ContainsKey(c))
        {
            // this will need refactoring/expanding for " characters, where the opening and closing char looks the same
            ++charCounter[c];
        }

        if (closingCharacters.Contains(c))
        {
            CloseBrackets(c, charCounter);
        }
    }
}

static void CloseBrackets(char closingCharacter, Dictionary<char, short> charCounter)
{
    char? openingCharacter = null;

    switch (closingCharacter)
    {
        case '}':
            openingCharacter = '{';
            break;
        case ']':
            openingCharacter = '[';
            break;
        default:
            break;
    }

    if (!openingCharacter.HasValue) throw new Exception();

    // so long as the number of open brackets doesn't go below 0, all is well
    short newValue = --charCounter[(char)openingCharacter];
    if (newValue >= 0) return;


    string exceptionMessage = String.Format("Paired Character Exception: could not parse input because of either a missing or superfluous '{0}' character.", openingCharacter);
    throw new Exception(exceptionMessage);
}

static void FinalBracketsCheck(Dictionary<char, short> charCounter)
{
    foreach (char c in charCounter.Keys)
    {
        if (charCounter[c] != 0)
        {
            string exceptionMessage = String.Format("Paired Character Exception: could not parse input because of missing partner to an unclosed '{0}' character.", c);
            throw new Exception(exceptionMessage);
        }
    }
}

static Dictionary<char, short> GetCharCounter()
{
    Dictionary<char, short> charCounter = new Dictionary<char, short>();

    string chars = "{[\"";
    foreach (char i in chars)
    {
        charCounter.Add(i, 0);
    }

    return charCounter;
}

static string StringifyJSON(Dictionary<string, dynamic> jsonTarget) { return new String(""); }

static string CharCounterToString(Dictionary<char, short> dictionary)
{
    List<string> pairs = new List<string>();

    foreach (KeyValuePair<char, short> keyValuePair in dictionary)
    {
        string type = keyValuePair.Key.GetType().Name;
        string key = keyValuePair.Key.ToString();
        string value = keyValuePair.Value.ToString();

        string pair = string.Format("    {0} {1}: {2}", type, key, value);

        pairs.Add(pair);
    }

    string result = String.Join(",\n", pairs.ToArray());

    return "{\n" + result + "\n}";
}

// this needs a better name to make it clearly distinct from the Stringify method
static string ParsedJSONObjectToString(Dictionary<string, dynamic> dictionary)
{
    List<string> pairs = new List<string>();

    foreach (KeyValuePair<string, dynamic> keyValuePair in dictionary)
    {
        string type = keyValuePair.Key.GetType().Name;
        string key = keyValuePair.Key.ToString();
        string value = keyValuePair.Value.ToString();

        string pair = string.Format("    {0} {1}: {2}", type, key, value);

        pairs.Add(pair);
    }

    string result = String.Join(",\n", pairs.ToArray());

    return "{\n" + result + "\n}";
}