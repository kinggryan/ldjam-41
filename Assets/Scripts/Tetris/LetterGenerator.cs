using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LetterGenerator {

    public struct WeightedCommand
    {
        public readonly Command command;
        public readonly string name;
        public readonly float weight;

        public WeightedCommand(Command command, string name, float weight)
        {
            this.command = command;
            this.name = name;
            this.weight = weight;
        }
    }

    public static WeightedCommand[] weightedCommandsList = new WeightedCommand[] {
        new WeightedCommand( Command.Up, "UP", 1)
    };

    static float weightedCommandTotal = 0;

	public static char GetLetter()
    {
        if(weightedCommandTotal == 0)
        {
            weightedCommandTotal = GetWeightedCommandTotal();
        }
        var command = GetRandomWeightCommand();
        var letterIndex = Random.Range(0, command.name.Length);
        return command.name[letterIndex];
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
