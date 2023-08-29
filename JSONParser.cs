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

//ParseJSON("   ");
ParseJSON("   {");
ParseJSON("   {}");

static Dictionary<string, dynamic> ParseJSON(string rawJSON)
{
    string errorMessage = "Input is not valid JSON.";
    Exception invalidJSON = new Exception(errorMessage);

    string trimmedJSON = rawJSON.Trim();

    if (trimmedJSON.Length == 0) throw invalidJSON;
    if (trimmedJSON[0] is not '{') throw invalidJSON;

    Dictionary<string, dynamic> parsedJSON = new Dictionary<string, dynamic>();

    Dictionary<char, int> charCounter = GetCharCounter();

    Console.WriteLine(DictionaryToString(charCounter));

    return parsedJSON;
}



static Dictionary<char, int> GetCharCounter()
{
    Dictionary<char, int> charCounter = new Dictionary<char, int>();

    string chars = "{}[]\"";
    foreach (char i in chars)
    {
        charCounter.Add(i, 0);
    }

    return charCounter;
}

static string StringifyJSON(Dictionary<string, dynamic> jsonTarget) { return new String(""); }

//static string DictionaryToString(Dictionary<dynamic, dynamic> dictionary)
//{
//    List<string> pairs = new List<string>();

//    foreach (KeyValuePair<dynamic, dynamic> keyValuePair in dictionary)
//    {
//        pairs.Add(string.Format("{0}: {1}", keyValuePair.Key, keyValuePair.Value));
//    }

//    return String.Join(",\n", pairs.ToArray());
//}

static string DictionaryToString(Dictionary<char, int> dictionary)
{
    List<string> pairs = new List<string>();

    foreach (KeyValuePair<char, int> keyValuePair in dictionary)
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