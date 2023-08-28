﻿/*
 * Wanted to make a test suite with these tests but couldn't get xUnit to work:
* - returns empty object if string is "{}"
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
 * 
 */


static object ParseJSON(string rawJSON)
{
// trim input
// early return if first character isn't "{"
// create test suite (in no particular order):
    return new Object();
}

static string StringifyJSON(object jsonTarget) { return new String(); }