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
        for (var y = board.GetLength(1) - 1; y >= 0; y--)
        {
            if(yCoord == y) {
                var comparisonString = "";
                for(var x = 0 ; x < board.GetLength(0); x++) {
                    comparisonString += board[x,y];
                }
                var startWordIndex = comparisonString.IndexOf(word);
                var endWordIndex = comparisonString.IndexOf(word) + word.Length;
                Debug.Log("Start: " + startWordIndex + " end " + endWordIndex);
                for(var x = 0 ; x < board.GetLength(0); x++) {
                    if(x >= startWordIndex && x < endWordIndex)
                        textString += board[x,y];
                    else
                        textString += " ";
                }
            } else {
                for(var x = 0 ; x < board.GetLength(0); x++) {
                    textString += " ";
                }
            }

            textString += "\n";
        }

        //Debug.Log("Highlighting word with " + textString);
        commandText.text = textString;
    }

    void HighLightWordInColumn(char[,] board, string word, int xCoord)
    {
        // In the given column
        // Construct a comparison string
        // Determine what the y coordinates of the given word are
        // Then type out the board
        var columnString = "";
        for(var y = board.GetLength(1) - 1; y >= 0 ; y--) {
            columnString += board[xCoord, y];
        }

        Debug.Log("Column string: " + columnString);

        var endYCoord = board.GetLength(1) - columnString.IndexOf(word);
        var startYCoord = endYCoord - word.Length;

        Debug.Log("Start Y " + startYCoord + ", end y " + endYCoord);

        var textString = "";
        for (var y = board.GetLength(1) - 1; y >= 0; y--)
        {
            for(var x = 0 ; x < board.GetLength(0); x++) {
                 if(y >= startYCoord - 1 && y < endYCoord && x == xCoord) {
                     textString += board[x,y];
                 } else {
                     textString += " ";
                 }
            }

            textString += "\n";
        }

        //Debug.Log("Highlighting word with " + textString);
        commandText.text = textString;
    }

    void HighlightLine(char[,] board, int yCoord)
    {
        // If the line complete text is blank, only this line should be highlighted.
        // Otherwise, we should use the existing text and just add a new line to it because we may want to display multiple lines at once
        var useLineCompleteText = lineCompleteText.text.Length > 0;
        var textString = "";
        for (var y = board.GetLength(1) - 1; y >= 0; y--)
        {
            for (var x = 0; x < board.GetLength(0); x++)
            {
                if (yCoord == y)
                {
                    textString += board[x, y];
                }
                else
                {
                    textString += useLineCompleteText ? lineCompleteText.text[y*(board.GetLength(0)+1) + x] : ' ';
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
