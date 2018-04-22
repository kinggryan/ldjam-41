using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class LetterGenerator {

    public struct WeightedCommand
    {
        public readonly TwineTextPlayer.Command command;
        public readonly string name;
        public readonly float weight;

        public WeightedCommand(TwineTextPlayer.Command command, string name, float weight)
        {
            this.command = command;
            this.name = name;
            this.weight = weight;
        }
    }

    public static WeightedCommand[] weightedCommandsList = new WeightedCommand[] {
        new WeightedCommand( TwineTextPlayer.Command.North, "NORTH", 1),
        new WeightedCommand( TwineTextPlayer.Command.South, "SOUTH", 1),
        new WeightedCommand( TwineTextPlayer.Command.East, "EAST", 1),
        new WeightedCommand( TwineTextPlayer.Command.West, "WEST", 1),
        new WeightedCommand( TwineTextPlayer.Command.Open, "OPEN", 1),
        new WeightedCommand( TwineTextPlayer.Command.Hack, "HACK", 1),
        new WeightedCommand( TwineTextPlayer.Command.Look, "LOOK", 1)

        //North,
        //South,
        //East,
        //West,
        //Open,
        //Hack,
        //Look,
    };

    static float weightedCommandTotal = 0;

	public static char GetLetter()
    {
        if(weightedCommandTotal == 0)
        {
            weightedCommandTotal = GetWeightedCommandTotal(weightedCommandsList);
        }
        var command = GetRandomWeightCommand(weightedCommandsList, weightedCommandTotal);
        var letterIndex = Random.Range(0, command.name.Length);
        return command.name[letterIndex];
    }

    public static string GetSubstringFromCommands(int length)
    {
        if (weightedCommandTotal == 0)
        {
            weightedCommandTotal = GetWeightedCommandTotal(weightedCommandsList);
        }
        var command = GetRandomWeightCommand(weightedCommandsList, weightedCommandTotal);

        // We don't want to error out if we can't build a substring that long from the command, so just return somethign totally random as a fallback
        if(command.name.Length < length)
        {
            // just build a random string
            var str = "";
            for(var i = 0; i < length; i++)
            {
                str += GetLetter();
            }
            return str;
        }

        // Return the valid string
        var startIndex = Random.Range(0, command.name.Length - length + 1);
        var substr = command.name.Substring(startIndex, length);
        Debug.Log("returning " + substr);
        return substr;
    }

    public static string GetSubstringFromCommandsIncludingLetterAtStart(int length, char letter)
    {
        var validCommandList = GetWeightedCommandsIncludingLetterAtEnd(weightedCommandsList, length, letter);

        // If this is impossible, just return any substring
        if(validCommandList.Length == 0)
        {
            return GetSubstringFromCommands(length);
        }

        //Debug.Log("Made commands list of length " + validCommandList.Length);

        var validWeightedCommandTotal = GetWeightedCommandTotal(validCommandList);
        var command = GetRandomWeightCommand(validCommandList, validWeightedCommandTotal);
        var substr = GetSubstringWithCharacterAtStartOrEnd(command.name, length, letter);
        if(substr == "")
        {
            Debug.LogError("Something went wrong, couldn't find substring of length " + length + " with letter " + letter + " in " + command.name);
            return GetSubstringFromCommands(length);
        }

        Debug.Log("returning " + substr);

        // Reverse it if needed
        if(substr[0] != letter)
        {
            var charArray = new List<char>(substr.ToCharArray());
            charArray.Reverse();
            return new string(charArray.ToArray());
        }

        // Return the default string
        return substr;
    }

    static WeightedCommand[] GetWeightedCommandsIncludingLetterAtEnd(WeightedCommand[] commands, int substringLength, char letter)
    {
        var newCommands = new List<WeightedCommand>();
        foreach(var command in commands)
        {
            if (CanWordContainSubstringWithCharacterAtStartOrEnd(command.name, substringLength, letter))
                newCommands.Add(command);
        }
        return newCommands.ToArray();
    }

    static bool CanWordContainSubstringWithCharacterAtStartOrEnd(string word, int length, char letter)
    {
        var substr = GetSubstringWithCharacterAtStartOrEnd(word, length, letter);
        //Debug.Log("Found substring '" + substr + "'" + " for word " + word + " with length " + length + " with letter " + letter);
        return substr != "";
    }

    static float GetWeightedCommandTotal(WeightedCommand[] weightedCommandsList)
    {
        float total = 0;
        foreach(var command in weightedCommandsList)
        {
            total += command.weight;
        }
        return total;
    }

    static string GetSubstringWithCharacterAtStartOrEnd(string word, int length, char letter)
    {
        // TODO : Randomize search direction?
        for (int i = 0; i < word.Length - length + 1; i++)
        {
            if (word[i] == letter && (i >= length - 1 || i + length - 1 < word.Length))
            {
                if (i >= length - 1)
                {
                    return word.Substring(i - length + 1, length);
                } else if (i + length - 1 < word.Length)
                {
                    return word.Substring(i, length);
                }
            }
        }

        return "";
    }

    static WeightedCommand GetRandomWeightCommand(WeightedCommand[] weightedCommandsList, float weightedCommandTotal)
    {
        var randomValue = Random.Range(0, weightedCommandTotal);
        foreach(var command in weightedCommandsList)
        {
            if(randomValue < command.weight)
            {
                return command;
            }
            randomValue -= command.weight;
        }

        return weightedCommandsList[weightedCommandsList.Length-1];
    }
}
