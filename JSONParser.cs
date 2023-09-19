using System;
using System.Runtime.CompilerServices;

public class JSONParser
{
    public JSONParser(string jsonInput)
    {
        Input = jsonInput.Trim();
        CurrentCharIndex = 1;
        ParsedJSON = new Dictionary<string, dynamic>();
        InvalidJSONException = GetInvalidJSONException();
        CharCounter = GetCharCounter();
    }

    // PROPERTIES -----------------------------------------------------------------------------------------------
    private readonly string Input;
    private int CurrentCharIndex { get; set; }
    private Dictionary<string, dynamic> ParsedJSON;
    private readonly Exception InvalidJSONException;
    private readonly Dictionary<char, short> CharCounter;

    // METHODS --------------------------------------------------------------------------------------------------
    public Dictionary<string, dynamic> Parse()
    {
        if (Input.Length == 0) throw InvalidJSONException;
        if (Input[0] is not '{') throw InvalidJSONException;

        ParseKeyValuePairs();

        FinalBracketsCheck();

        PrintParsedJSONAndCharCounter();

        return ParsedJSON;
    }

    private void PrintParsedJSONAndCharCounter()
    {
        Console.WriteLine("CharCounter:");
        Console.WriteLine(CharCounterToString());
        Console.WriteLine("Parsed JSON output:");
        Console.WriteLine(ParsedJSONObjectToString(ParsedJSON));
    }

    private static Exception GetInvalidJSONException()
    {
        string errorMessage = "Input is not valid JSON.";
        return new Exception(errorMessage);
    }

    private void ParseKeyValuePairs()
    {
        while (CurrentCharIndex < Input.Length)
        {
            // potential refactor: instead of looping until the Input is over, run this loop using the CharCounter and then, once the loop is complete, check to ensure that there are no further characters at the end of the Input
            CurrentCharIndex = Input.Length; // DELETE THIS

            //
            //
            // what if there's no key, just a closing }, e.g. in an empty object?
            //
            //
            //
            //
            //
            string key = RetrieveKey();
            CheckForColon();
            dynamic value = RetrieveValue();
            ParsedJSON.Add(key, value);
            CheckForCommaOrEnd();
        }





        //
        //
        //
        // the below can be deleted later
        //
        //
        string closingCharacters = "]}";



        foreach (char c in Input)
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

            if (CharCounter.ContainsKey(c))
            {
                // this will need refactoring/expanding for " characters, where the opening and closing char looks the same
                ++CharCounter[c];
            }

            if (closingCharacters.Contains(c))
            {
                CloseBrackets(c);
            }
        }
    }


    private string RetrieveKey()
    {
        string key = ParseString();
        if (key is "") throw InvalidJSONException;
        return key;
    }
    private void CheckForColon()
    {
        SkipToNextNonSpaceChar();
        if (Input[CurrentCharIndex] is not ':') throw InvalidJSONException;
        ++CurrentCharIndex;
    }
    private dynamic RetrieveValue()
    {
        SkipToNextNonSpaceChar();
        switch (Input[CurrentCharIndex])
        {
            case '"':
                return ParseString();
            case 't':
                return ParseTrue();
            case 'f':
                return ParseFalse();
            case 'n':
                return ParseNull();
            case '{':
                return ParseObject();
            case '[':
                return ParseArray();
            default:
                return ParseNumber();
        }
    }
    private void CheckForCommaOrEnd()
    {
        SkipToNextNonSpaceChar();
        switch (Input[CurrentCharIndex])
        {
            case ',':
                break;
            case '}':
                // update CharCounter here
                break;
            default:
                throw InvalidJSONException;
        }
        ++CurrentCharIndex;
    }

    private bool ParseTrue()
    {
        return true;
    }

    private bool ParseFalse()
    {
        return false;
    }

    private bool? ParseNull()
    {
        return null;
    }

    private dynamic ParseNumber()
    {
        // check for negative number and multiply by -1 before returning
        return 0;
    }

    private int ParseInteger()
    {
        // should I allow for the maximum number size?
        return 0;
    }

    private float ParseFloatingPoint()
    {
        // should I allow for the maximum number size?
        return 0;
    }

    private List<dynamic> ParseArray()
    {
        return new List<dynamic>();
    }

    private Dictionary<string, dynamic> ParseObject()
    {
        // this one should take a CharCounter and have a while loop that loops until the number of open brackets is back to what it was before the loop began, but should otherwise follow the same process as the global while loop that loops until the text is exhausted
        return new Dictionary<string, dynamic>();
    }

    private string ParseString()
    {
        SkipToNextNonSpaceChar();

        if (Input[CurrentCharIndex] is not '"') throw InvalidJSONException;
        ++CurrentCharIndex;

        string value = "";

        for (int i = CurrentCharIndex; i < Input.Length; ++i)
        {
            CurrentCharIndex = i;

            char c = Input[i];
            if (c is '"') break;

            value += c;

            if (c is '\\')
            {
                value += Input[i + 1]; // POTENTIAL EDGE CASE: this line could through a range error when given invalid JSON. Would be better to throw an invalid JSON exception
                ++i;
            }
        }

        return value;
    }

    private void SkipToNextNonSpaceChar()
    {
        string spaces = " \n\r";

        for (int i = CurrentCharIndex; i < Input.Length; ++i)
        {
            CurrentCharIndex = i;
            char c = Input[i];
            if (!spaces.Contains(c)) break;
        }
    }

    private void CloseBrackets(char closingCharacter)
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
        short newValue = --CharCounter[(char)openingCharacter];
        if (newValue >= 0) return;

        string exceptionMessage = String.Format("Paired Character Exception: could not parse input because of either a missing or superfluous '{0}' character.", openingCharacter);
        throw new Exception(exceptionMessage);
    }

    private void FinalBracketsCheck()
    {
        foreach (char c in CharCounter.Keys)
        {
            if (CharCounter[c] != 0)
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

    private string CharCounterToString()
    {
        var pairs = new List<string>();

        foreach (KeyValuePair<char, short> keyValuePair in CharCounter)
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
}