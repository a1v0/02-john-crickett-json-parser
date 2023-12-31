﻿using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

public class JSONParser
{
    public JSONParser(string jsonInput)
    {
        Input = jsonInput.Trim();
        CurrentCharIndex = 1;
        ParsedJSON = new Dictionary<string, dynamic>();
        InvalidJSONException = GetInvalidJSONException();
        OpenObjects = 0;
        OpenArrays = 0;
    }

    // PROPERTIES -----------------------------------------------------------------------------------------------
    private readonly string Input;
    private int CurrentCharIndex { get; set; }
    private readonly Dictionary<string, dynamic> ParsedJSON;
    private readonly Exception InvalidJSONException;
    private int OpenObjects { get; set; }
    private int OpenArrays { get; set; }

    // METHODS --------------------------------------------------------------------------------------------------
    public Dictionary<string, dynamic> Parse()
    {
        if (Input.Length == 0) throw InvalidJSONException;
        if (Input[0] is not '{') throw InvalidJSONException;

        ParseKeyValuePairs(ParsedJSON);

        // PLACEHOLDER: we need a check here to ensure there's no additional text AFTER the final bracket has closed, e.g. "{ } blabla"

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
        const string errorMessage = "Input is not valid JSON.";
        return new Exception(errorMessage);
    }

    private void ParseKeyValuePairs(Dictionary<string, dynamic> JSONObject)
    {
        ++OpenObjects;

        int noOfOpenBracesAtStart = OpenObjects;

        CheckForEmptyObject();

        while (OpenObjects >= noOfOpenBracesAtStart)
        {
            string key = RetrieveKey();
            CheckForColon();
            dynamic value = RetrieveValue();
            JSONObject.Add(key, value);
            CheckForCommaOrCurlyBrace();
        }
    }

    private void CheckForEmptyObject()
    {
        SkipToNextNonSpaceChar();
        if (Input[CurrentCharIndex] is '}')
        {
            ++CurrentCharIndex;
            --OpenObjects;
        }
    }

    private void CheckForEmptyArray()
    {
        SkipToNextNonSpaceChar();
        if (Input[CurrentCharIndex] is ']')
        {
            ++CurrentCharIndex;
            --OpenArrays;
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

    private void CheckForCommaOrCurlyBrace()
    {
        SkipToNextNonSpaceChar();
        switch (Input[CurrentCharIndex])
        {
            case ',':
                break;
            case '}':
                --OpenObjects;
                break;
            default:
                throw InvalidJSONException;
        }
        ++CurrentCharIndex;
    }

    private void CheckForCommaOrSquareBracket()
    {
        SkipToNextNonSpaceChar();
        switch (Input[CurrentCharIndex])
        {
            case ',':
                break;
            case ']':
                --OpenArrays;
                break;
            default:
                throw InvalidJSONException;
        }
        ++CurrentCharIndex;
    }

    private bool ParseTrue()
    {
        const string trueValue = "true";
        string nextLetters = Input.Substring(CurrentCharIndex, trueValue.Length);
        if (nextLetters is trueValue)
        {
            CurrentCharIndex += trueValue.Length;
            return true;
        }
        throw InvalidJSONException;
    }

    private bool ParseFalse()
    {
        const string falseValue = "false";
        string nextLetters = Input.Substring(CurrentCharIndex, falseValue.Length);
        if (nextLetters is falseValue)
        {
            CurrentCharIndex += falseValue.Length;
            return false;
        }
        throw InvalidJSONException;
    }

    private bool? ParseNull()
    {
        const string nullValue = "null";
        string nextLetters = Input.Substring(CurrentCharIndex, nullValue.Length);
        if (nextLetters is nullValue)
        {
            CurrentCharIndex += nullValue.Length;
            return null;
        }
        throw InvalidJSONException;
    }

    private dynamic ParseNumber()
    {
        string extractedNumber = "";
        bool isFloat = false;

        const string validNumberCharacters = "0123456789-.";
        const string validEndCharacters = " ]},";

        for (int i = CurrentCharIndex; i < Input.Length; ++i)
        {
            CurrentCharIndex = i;

            char c = Input[i];
            if (validEndCharacters.Contains(c)) break;
            if (!validNumberCharacters.Contains(c)) throw InvalidJSONException;
            if (c is '.') isFloat = true;

            extractedNumber += c;
        }

        if (isFloat) return ParseFloatingPoint(extractedNumber);
        return ParseInteger(extractedNumber);
    }

    private long ParseInteger(string number)
    {
        bool successfulConversion = long.TryParse(number, out long result);

        if (!successfulConversion) throw InvalidJSONException;

        return result;
    }

    private double ParseFloatingPoint(string number)
    {
        bool successfulConversion = double.TryParse(number, out double result);

        if (!successfulConversion) throw InvalidJSONException;

        return result;
    }

    private List<dynamic> ParseArray()
    {
        ++CurrentCharIndex;

        ++OpenArrays;

        int noOfOpenArraysAtStart = OpenArrays;

        var parsedArray = new List<dynamic>();

        CheckForEmptyArray();

        while (OpenArrays >= noOfOpenArraysAtStart)
        {
            dynamic value = RetrieveValue();
            parsedArray.Add(value);
            CheckForCommaOrSquareBracket();
        }

        return parsedArray;
    }

    private Dictionary<string, dynamic> ParseObject()
    {
        ++CurrentCharIndex;
        var JSONObject = new Dictionary<string, dynamic>();
        ParseKeyValuePairs(JSONObject);
        return JSONObject;
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
        const string spaces = " \n\r";

        for (int i = CurrentCharIndex; i < Input.Length; ++i)
        {
            CurrentCharIndex = i;
            char c = Input[i];
            if (!spaces.Contains(c)) break;
        }
    }

    private static string ParsedJSONObjectToString(Dictionary<string, dynamic> dictionary, int levelOfNesting = 1)
    {
        var pairs = new List<string>();
        int depthOfIndentation = 4;
        string partialIndentation = new string(' ', depthOfIndentation * (levelOfNesting - 1));
        string fullIndentation = new string(' ', depthOfIndentation * levelOfNesting);

        foreach (KeyValuePair<string, dynamic> keyValuePair in dictionary)
        {
            string key = keyValuePair.Key.ToString();
            string value = keyValuePair.Value.ToString();
            string valueType = keyValuePair.Value.GetType().Name;

            if (valueType == "Dictionary`2")
            {
                value = ParsedJSONObjectToString(keyValuePair.Value, levelOfNesting + 1);
            }
            else if (valueType == "List`1")
            {
                value = ParsedJSONArrayToString(keyValuePair.Value);
            }

            string pair = string.Format(fullIndentation + "{0}: {1} {2}", key, valueType, value);

            pairs.Add(pair);
        }

        if (pairs.Count == 0) return "{}";

        string result = String.Join(",\n", pairs.ToArray());

        return "{\n" + result + "\n" + partialIndentation + "}";
    }

    private static string ParsedJSONArrayToString(List<dynamic> jsonArray)
    {
        var values = new List<string>();

        foreach (dynamic jsonValue in jsonArray)
        {
            string value = jsonValue.ToString();
            string valueType = jsonValue.GetType().Name;

            if (valueType == "Dictionary`2")
            {
                value = ParsedJSONObjectToString(jsonValue);
            }
            else if (valueType == "List`1")
            {
                value = ParsedJSONArrayToString(jsonValue);
            }

            string valueAndType = string.Format("{0} {1}", valueType, value);

            values.Add(valueAndType);
        }

        if (values.Count == 0) return "[]";

        string result = String.Join(", ", values.ToArray());

        return "[ " + result + " ]";
    }
}