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

string divider = "----------------------------------------------------------------------------------------";

// PASS TESTS
Console.WriteLine(divider);
new JSONParser("{}").Parse(); // test case: PASS empty JSON object

Console.WriteLine(divider);
new JSONParser("   {          }    ").Parse(); // test case: PASS ignores whitespace between JSON content

Console.WriteLine(divider);
new JSONParser("{\"key\": \"value\"}").Parse(); // test case: PASS correctly parses a string key/value pair

Console.WriteLine(divider);
new JSONParser("{\"key1\": \"value\", \"key2\": \"value\"}").Parse(); // test case: PASS correctly parses multiple string key/value pairs

// FAIL TESTS
Console.WriteLine(divider);
new JSONParser("   {").Parse(); // test case: FAIL unclosed bracket

Console.WriteLine(divider);
new JSONParser("   ").Parse(); // test case: FAIL empty string


public class JSONParser
{
    private readonly string input;

    public JSONParser(string jsonInput)
    {
        input = jsonInput.Trim();
        CurrentProgramState = 0;
    }

    public Dictionary<string, dynamic> Parse()
    {
        var invalidJSON = GetInvalidJSONException();

        if (this.input.Length == 0) throw invalidJSON;
        if (this.input[0] is not '{') throw invalidJSON;

        var parsedJSON = new Dictionary<string, dynamic>();

        var charCounter = GetCharCounter();

        LoopThroughInput(parsedJSON, charCounter);

        FinalBracketsCheck(charCounter);

        PrintParsedJSONAndCharCounter(parsedJSON, charCounter);

        return parsedJSON;
    }

    private static void PrintParsedJSONAndCharCounter(Dictionary<string, dynamic> parsedJSON, Dictionary<char, short> charCounter)
    {
        Console.WriteLine("charCounter:");
        Console.WriteLine(CharCounterToString(charCounter));
        Console.WriteLine("Parsed JSON output:");
        Console.WriteLine(ParsedJSONObjectToString(parsedJSON));
    }

    private static Exception GetInvalidJSONException()
    {
        string errorMessage = "Input is not valid JSON.";
        return new Exception(errorMessage);
    }

    private void LoopThroughInput(Dictionary<string, dynamic> parsedJSON, Dictionary<char, short> charCounter)
    {
        string closingCharacters = "]}";

        Action[] programStates = { ExpectKey, ExpectColon, ExpectValue, ExpectCommaOrEnd }; // I know this variable should be a static property or similar but I can't get it to work. If you update the length of this, you need to update the CurrentProgramState property


        foreach (char c in this.input)
        {
            // create a property on the class to house the current char index we're going through
            // each method starts looping through from the latest index and updates the value on exit
            // - doesn't this violate the Do One Thing rule? See if you can avoid it






            // state explicitly what we are expecting (bracket, key (i.e. string), value, colon, comma etc.)
            // - this might make most sense as a string field, e.g. expecting = "comma"
            // - this way, if we're 'inside' a key, we can ignore any spaces, brackets etc.
            // use a switch statement to control the logic (e.g. if we're expectinng a key, go to method FindKey or something)
            // foreach might not be appropriate, because it might make more sense for the individual methods to update the character we're working on. As such, a conventional for loop, wherein we can update the position of c, might be more sensible
            // might also need some sort of recursion for nested objects.
            // - perhaps, therefore, I need to investigate using substrings, identifying where the closing bracket is before I parse it

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

    private void ExpectKey() { }
    private void ExpectColon() { }
    private void ExpectValue() { }
    private void ExpectCommaOrEnd() {
        // for nested objects to work, I reckon this one will need to know whether it's top-level or not. Should be doable using the CharCounter
        }

        private void CloseBrackets(char closingCharacter, Dictionary<char, short> charCounter)
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

    private void FinalBracketsCheck(Dictionary<char, short> charCounter)
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

    private static Dictionary<char, short> GetCharCounter()
    {
        var charCounter = new Dictionary<char, short>();

        string chars = "{[\"";
        foreach (char i in chars)
        {
            charCounter.Add(i, 0);
        }

        return charCounter;
    }

    private static string CharCounterToString(Dictionary<char, short> dictionary)
    {
        var pairs = new List<string>();

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
    private static string ParsedJSONObjectToString(Dictionary<string, dynamic> dictionary)
    {
        var pairs = new List<string>();

        foreach (KeyValuePair<string, dynamic> keyValuePair in dictionary)
        {
            string type = keyValuePair.Key.GetType().Name;
            string key = keyValuePair.Key.ToString();
            string value = keyValuePair.Value.ToString();

            string pair = string.Format("    {0} {1}: {2}", type, key, value);

            pairs.Add(pair);
        }

        if (pairs.Count == 0) return "{}";

        string result = String.Join(",\n", pairs.ToArray());

        return "{\n" + result + "\n}";
    }

    private int CurrentProgramState
    {
        get;
        set;
    }

    private void UpdateCurrentProgramState()
    {
        ++CurrentProgramState;
        if (CurrentProgramState > 4)
        {
            CurrentProgramState = 0;
        }
    }
}

/***************************************************************************************************************************/

public class JSONStringifier
{
    public Dictionary<string, dynamic> input;

    public JSONStringifier(Dictionary<string, dynamic> dataToBeStringified) { input = dataToBeStringified; }

    //public string Stringify() { }
}
