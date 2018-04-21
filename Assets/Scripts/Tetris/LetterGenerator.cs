using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LetterGenerator {

    static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public static char GetLetter()
    {
        var letterIndex = Random.Range(0, letters.Length);
        return letters[letterIndex];
    }
}
