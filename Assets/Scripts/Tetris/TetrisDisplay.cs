using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisDisplay : MonoBehaviour {

    public UnityEngine.UI.Text debugText;
    public UnityEngine.UI.Text commandText;
    public UnityEngine.UI.Text lineCompleteText;

	// Use this for initialization
	void Start () {
		// debugText.SetAllDirty();
        // commandText.SetAllDirty();
        // lineCompleteText.SetAllDirty();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateBoard(char[,] board, TetrisBlock currentBlock)
    {
        var displayBoard = board;
        if(currentBlock != null)
            displayBoard = currentBlock.AddToBoard(board);
        var debugTextStr = "";
        for(var y = displayBoard.GetLength(1)-1; y >= 0; y--)
        {
            for(var x = 0; x < displayBoard.GetLength(0); x++)
            {
                debugTextStr += displayBoard[x, y];
            }
            debugTextStr += "\n";
        }

        debugText.text = debugTextStr;
        commandText.text = "";
        lineCompleteText.text = "";

    }

    public void UpdateBoardWithCommandOnLine(char[,] board, string command, int yCoord)
    {
        UpdateBoard(board, null);
        HighLightWord(board, command, yCoord);
        HighlightLine(board, yCoord);
    }

    public void UpdateBoardWithCommandOnColumn(char[,] board, string command, int xCoord)
    {
        UpdateBoard(board, null);
        HighLightWordInColumn(board, command, xCoord);
        HighlightColumn(board, xCoord);
    }

    public void UpdateBoardWithCompleteLine(char[,] board, int yCoord)
    {
        HighlightLine(board, yCoord);
    }

    public void UpdateBoardWithCompleteColumn(char[,] board, int xCoord)
    {
        HighlightColumn(board, xCoord);
    }

    void HighLightWord(char[,] board, string word, int yCoord)
    {
        var textString = "";
        var currentWordIndex = 0;
        for (var y = board.GetLength(1) - 1; y >= 0; y--)
        {
            for (var x = 0; x < board.GetLength(0); x++)
            {
                if(currentWordIndex < word.Length && yCoord == y && board[x, y] == word[currentWordIndex])
                {
                    textString += board[x, y];
                    currentWordIndex++;
                } else
                {
                    textString += ' ';
                }
            }
            textString += "\n";
        }

        //Debug.Log("Highlighting word with " + textString);
        commandText.text = textString;
    }

    void HighLightWordInColumn(char[,] board, string word, int xCoord)
    {
        var textString = "";
        var currentWordIndex = 0;
        for (var x = board.GetLength(0) - 1; x >= 0; x--)
        {
            for (var y = 0; y < board.GetLength(1); y++)
            {
                if(currentWordIndex < word.Length && xCoord == x && board[x, y] == word[currentWordIndex])
                {
                    textString += board[x, y];
                    currentWordIndex++;
                } else
                {
                    textString += ' ';
                }
            }
            textString += "\n";
        }

        //Debug.Log("Highlighting word with " + textString);
        commandText.text = textString;

        Debug.Log("WORD HIGLIGHTED?");
    }

    void HighlightLine(char[,] board, int yCoord)
    {
        var textString = "";
        var currentWordIndex = 0;
        for (var y = board.GetLength(1) - 1; y >= 0; y--)
        {
            for (var x = 0; x < board.GetLength(0); x++)
            {
                if (yCoord == y)
                {
                    textString += board[x, y];
                    currentWordIndex++;
                }
                else
                {
                    textString += ' ';
                }
            }
            textString += "\n";
        }

        lineCompleteText.text = textString;
    }

    void HighlightColumn(char[,] board, int xCoord)
    {
        var textString = "";
        var currentWordIndex = 0;
        for (var x = board.GetLength(0) - 1; x >= 0; x--)
        {
            for (var y = 0; y < board.GetLength(1); y++)
            {
                if (xCoord == x)
                {
                    textString += board[x, y];
                    currentWordIndex++;
                }
                else
                {
                    textString += ' ';
                }
            }
            textString += "\n";
        }

        lineCompleteText.text = textString;
        Debug.Log("COLUMN HIGLIGHTED?");
    }
}
