public class JSONParser
{
    public JSONParser(string jsonInput)
    {
        Input = jsonInput.Trim();
        CurrentCharIndex = 1;
        ParsedJSON = new Dictionary<string, dynamic>();
        InvalidJSONException = GetInvalidJSONException();
        OpenBraces = 0;
    }

    // PROPERTIES -----------------------------------------------------------------------------------------------
    private readonly string Input;
    private int CurrentCharIndex { get; set; }
    private Dictionary<string, dynamic> ParsedJSON;
    private readonly Exception InvalidJSONException;
    private int OpenBraces { get; set; }

    // METHODS --------------------------------------------------------------------------------------------------
    public Dictionary<string, dynamic> Parse()
    {
        if (Input.Length == 0) throw InvalidJSONException;
        if (Input[0] is not '{') throw InvalidJSONException;

        ParseKeyValuePairs();

        PrintParsedJSON();

        return ParsedJSON;
    }

    private void PrintParsedJSON()
    {
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
        ++OpenBraces;

        int noOfOpenBracesAtStart = OpenBraces;

        CheckForEmptyObject();

        while (OpenBraces >= noOfOpenBracesAtStart)
        {
            // potential refactor: instead of looping until the Input is over, run this loop using the CharCounter and then, once the loop is complete, check to ensure that there are no further characters at the end of the Input

            string key = RetrieveKey();
            CheckForColon();
            dynamic value = RetrieveValue();
            ParsedJSON.Add(key, value);
            CheckForCommaOrEnd();
        }
    }

    private void CheckForEmptyObject()
    {
        SkipToNextNonSpaceChar();
        if (Input[CurrentCharIndex] is '}') --OpenBraces;
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
                --OpenBraces;
                break;
            default:
                throw InvalidJSONException;
        }
        ++CurrentCharIndex;
    }

    private bool ParseTrue()
    {
        string nextFourLetters = Input.Substring(CurrentCharIndex, 4);
        if (nextFourLetters is "true") return true;
        throw InvalidJSONException;
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

        ++CurrentCharIndex;
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