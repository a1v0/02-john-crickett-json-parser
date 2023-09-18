// - instead of program states, just make a controller that takes the program through the key/value generation process in order
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
// 
















using System;
using System.Runtime.CompilerServices;

public class JSONParser
{
    public JSONParser(string jsonInput)
    {
        Input = jsonInput.Trim();
        CurrentCharIndex = 1;
        ParsedJSON = new Dictionary<string, dynamic>();
    }

    // PROPERTIES -----------------------------------------------------------------------------------------------
    private readonly string Input;
    private int CurrentCharIndex { get; set; }
    private Dictionary<string, dynamic> ParsedJSON { get; }

    // METHODS --------------------------------------------------------------------------------------------------
    public Dictionary<string, dynamic> Parse()
    {
        var invalidJSONException = GetInvalidJSONException();

        if (Input.Length == 0) throw invalidJSONException;
        if (Input[0] is not '{') throw invalidJSONException;

        var charCounter = GetCharCounter();

        ParseKeyValuePairs(charCounter);

        FinalBracketsCheck(charCounter);

        PrintParsedJSONAndCharCounter(charCounter);

        return ParsedJSON;
    }

    private void PrintParsedJSONAndCharCounter(Dictionary<char, short> charCounter)
    {
        Console.WriteLine("charCounter:");
        Console.WriteLine(CharCounterToString(charCounter));
        Console.WriteLine("Parsed JSON output:");
        Console.WriteLine(ParsedJSONObjectToString(ParsedJSON));
    }

    private static Exception GetInvalidJSONException()
    {
        string errorMessage = "Input is not valid JSON.";
        return new Exception(errorMessage);
    }

    private void ParseKeyValuePairs(Dictionary<char, short> charCounter)
    {
        while (CurrentCharIndex < Input.Length)
        {
            CurrentCharIndex = Input.Length; // DELETE THIS

            string key = RetrieveKey();
            //
            //
            // what if there's no key, just a closing }, e.g. in an empty object?
            //
            //
            //
            //
            //

            // check for colon
            // retrieve value
            // add pair to JSON
            // check whether there's another pair or whether to end the program
        }





        //
        //
        //
        // the below can be deleted later
        //
        //
        string closingCharacters = "]}";



        foreach (char c in this.Input)
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


    private string RetrieveKey()
    {
        return ParseString();
    }
    private void CheckForColon() { }
    private void RetrieveValue() { }
    private void CheckForCommaOrEnd()
    {
        // for nested objects to work, I reckon this one will need to know whether it's top-level or not. Should be doable using the CharCounter
    }

    private string ParseString()
    {
        // for loop through input
        // ignore spaces before "
        // expect "
        // find way to ignore escaped " characters
        // 
        return "";
    }

    private static void CloseBrackets(char closingCharacter, Dictionary<char, short> charCounter)
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

    private static void FinalBracketsCheck(Dictionary<char, short> charCounter)
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
}