using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class LetterGenerator {

    public struct WeightedCommand
    {
        public readonly TwineTextPlayer.Command command;
        public readonly string name;
        public readonly float weight;
        public readonly string[] aliases;

        public WeightedCommand(TwineTextPlayer.Command command, string name, float weight, string[] aliases)
        {
            this.command = command;
            this.name = name;
            this.weight = weight;
            this.aliases = aliases;
        }
    }

    // SHOULD BE CONST
    public static WeightedCommand[] fullWeightedCommandsList = new WeightedCommand[] {
        new WeightedCommand( TwineTextPlayer.Command.North, "NORTH", 1, new string[]{ "UP", "OPEN" }),
        new WeightedCommand( TwineTextPlayer.Command.South, "SOUTH", 1, new string[]{ "DOWN" }),
        new WeightedCommand( TwineTextPlayer.Command.East, "EAST", 1, new string[]{  }),
        new WeightedCommand( TwineTextPlayer.Command.West, "WEST", 1, new string[]{ "LEFT" }),
        new WeightedCommand( TwineTextPlayer.Command.Open, "OPEN", 1, new string[]{ }),
        new WeightedCommand( TwineTextPlayer.Command.Hack, "HACK", 1, new string[]{ }),
        new WeightedCommand( TwineTextPlayer.Command.Look, "LOOK", 1, new string[]{ "SEARCH", "PEER", "GAPE", "OGLE", "SCAN", "PROBE", "SEEK", "PEEK" }),
        new WeightedCommand( TwineTextPlayer.Command.Talk, "TALK", 1, new string[]{ "SPEAK", "HELLO", "CHAT", "GAB", "CONFAB"}),

        // And all the item commands
        new WeightedCommand( TwineTextPlayer.Command.UseGun, "USEGUN", 1, new string[]{"GUN", "ARM", "GAT" }),
        new WeightedCommand( TwineTextPlayer.Command.UseFob, "USEFOB", 1, new string[]{"FOB" }),
		new WeightedCommand( TwineTextPlayer.Command.UseCoat, "USECOAT", 1, new string[]{"COAT", "CLOTH", "GARB" }),
		new WeightedCommand( TwineTextPlayer.Command.UseCure, "USECURE", 1, new string[]{"CURE", "HEAL" }),
      
        new WeightedCommand( TwineTextPlayer.Command.GetFob, "GETFOB", 1, new string[]{"FOB" }),
       
        new WeightedCommand( TwineTextPlayer.Command.GetCoat, "GETCOAT", 1, new string[]{ "COAT", "CLOTH", "GARB"}),
        new WeightedCommand( TwineTextPlayer.Command.End, "END", 1, new string[]{ })

        //North,
        //South,
        //East,
        //West,
        //Open,
        //Hack,
        //Look,
    };

    public static WeightedCommand[] weightedCommandsList = new WeightedCommand[] { };

    static float weightedCommandTotal = 0;

    public static void UpdateWithPossibleCommands(TwineTextPlayer.Command[] commands)
    {
        // HACK: we should jsut prevent this from happening at the start of the story but whatever
        if(commands.Length == 0)
        {
            weightedCommandsList = new WeightedCommand[] { new WeightedCommand(TwineTextPlayer.Command.Hack, "HACK", 1, new string[] { }) };
            weightedCommandTotal = 0;
            return;
        }

        // Generate the list from the default list
        var newWeightedList = new List<WeightedCommand>();
        foreach(var command in commands)
        {
            foreach(var weightedCommand in fullWeightedCommandsList)
            {
                if(weightedCommand.command == command)
                {
                    newWeightedList.Add(weightedCommand);
                    // Debug.Log("Adding " + weightedCommand.name + " to commands list.");
                }
            }
        }
        weightedCommandsList = newWeightedList.ToArray();
        weightedCommandTotal = 0;
    }

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
        //Debug.Log("returning " + substr);
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

        //Debug.Log("returning " + substr);

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
