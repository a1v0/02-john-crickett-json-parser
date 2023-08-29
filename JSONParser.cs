/*
 * Wanted to make a test suite with these tests but couldn't get xUnit to work:
 * - returns empty object if string is "{}"
 * - returns empty object if string is "{           }"
 * - ignores white space and line breaks
 * - parses string key/value pairs
 * - parses numbers (incl. floats)
 * - parses boolean values
 * - parses arrays
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

ParseJSON("   ");
ParseJSON("   {");
ParseJSON("   {}");

static Dictionary<string, dynamic> ParseJSON(string rawJSON)
{
    string errorMessage = "Input is not valid JSON.";
    Exception invalidJSON = new Exception(errorMessage);
    try
    {
        string trimmedJSON = rawJSON.Trim();

        if (trimmedJSON.Length == 0) throw invalidJSON;
        if (trimmedJSON[0] is not '{') throw invalidJSON;

        Dictionary<string, dynamic> parsedJSON = new Dictionary<string, dynamic>();

        // loop through JSON
        // keep tally of all open curly brackets
        // keep tally of all open square brackets
        // keep tally of all open strings

        return parsedJSON;
    }
    catch (Exception exception)
    {
        Console.WriteLine(errorMessage);
    }
}

static string StringifyJSON(Dictionary<string, dynamic> jsonTarget) { return new String(""); }