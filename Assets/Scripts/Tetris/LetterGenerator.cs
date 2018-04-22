using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LetterGenerator {

    struct WeightedCommand
    {
        public readonly string command;
        public readonly float weight;

        public WeightedCommand(string command, float weight)
        {
            this.command = command;
            this.weight = weight;
        }
    }

    static WeightedCommand[] weightedCommandsList = new WeightedCommand[] {
        new WeightedCommand( "NONE", 1 )
    };

    static float weightedCommandTotal = 0;

	public static char GetLetter()
    {
        if(weightedCommandTotal == 0)
        {
            weightedCommandTotal = GetWeightedCommandTotal();
        }
        var command = GetRandomWeightCommand();
        var letterIndex = Random.Range(0, command.command.Length);
        return command.command[letterIndex];
    }

    static float GetWeightedCommandTotal()
    {
        float total = 0;
        foreach(var command in weightedCommandsList)
        {
            total += command.weight;
        }
        return total;
    }

    static WeightedCommand GetRandomWeightCommand()
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
