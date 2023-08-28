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
 * - throws errors:
 *   - no quotes on key or string value (or single quotes)
 *   - incorrectly escaped strings
 *   - unrecognised values (including incorrectly cased NULL/TRUE etc.)
 *   - missing comma
 *   - missing closing bracket
 */

ParseJSON("   {");

static object ParseJSON(string rawJSON)
{
    Exception invalidJSON = new Exception("Input is not valid JSON.");
    // is there a way to create a more specific return type than just "object"? Something like an interface that doesn't require defining property names
    string trimmedJSON = rawJSON.Trim();

    Console.WriteLine(new Object());

if (trimmedJSON.Length == 0) throw invalidJSON;
if (trimmedJSON[0] is not '{') throw invalidJSON;

return new Object();
}

static string StringifyJSON(object jsonTarget) { return new String(""); }