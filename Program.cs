/*
 * Wanted to make a test suite with these tests but couldn't get xUnit to work:
 * - parses arrays
 *   - what should I parse this as? Always as an untyped collection or rather do I validate that it could work as a typed array and conditionally create an array on that basis?
 * - parses objects
 * - parses nested objects
 * - parses nested arrays
 * - ignores quotation marks and brackets when escaped and in a string
 * - throws errors:
 *   - no quotes on key or string value (or single quotes)
 *   - incorrectly escaped strings (key and value)
 *   - incorrectly cased bools or null
 *   - numbers with multiple . or - characters
 *   - missing comma
 *   - missing closing bracket
 */

string divider = "----------------------------------------------------------------------------------------";

// PASS TESTS
Console.WriteLine(divider);
PrintHeadingForPassTest("empty JSON object");
new JSONParser("{}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("ignores whitespace between JSON content");
new JSONParser("   {          }    ").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses a string key/value pair");
new JSONParser("{\"key\": \"value\"}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses multiple string key/value pairs");
new JSONParser("{\"key1\": \"value\", \"key2\": \"value\"}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses multiple string key/value pairs");
new JSONParser("{\n\"key1\": \n\"value\", \r\"key2\":\n\r \"value\"}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses Boolean values");
new JSONParser("{\"key1\": true, \"key2\": false}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses positive integers");
new JSONParser("{\"key1\": 1,       \"key2\"    :   123456789, \"key3\": 0    }").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses negative integers");
new JSONParser("{\"key1\": -0, \"key2\": -123456789}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses positive floats");
new JSONParser("{\"key1\": 1.000000,       \"key2\"    :   123456789, \"key3\": 120.456789510001    }").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses negative floats");
new JSONParser("{\"key1\": -0.0001, \"key2\": -12345.6789}").Parse();

Console.WriteLine(divider);
PrintHeadingForPassTest("correctly parses null values");
new JSONParser("{\"key1\": null, \"key2\": false  }").Parse();

// --------------------------------------------------------------------------------------------------------------------
// FAIL TESTS
Console.WriteLine(divider);
PrintHeadingForFailTest("recognises unclosed brace");
new JSONParser("   {").Parse();

Console.WriteLine(divider);
PrintHeadingForFailTest("recognises empty string");
new JSONParser("   ").Parse();

static void PrintHeadingForPassTest(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("PASS: " + message);
    Console.ForegroundColor = ConsoleColor.White;
}

static void PrintHeadingForFailTest(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("FAIL: " + message);
    Console.ForegroundColor = ConsoleColor.White;
}