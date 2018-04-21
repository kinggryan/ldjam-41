using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisDisplay : MonoBehaviour {

    public UnityEngine.UI.Text debugText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateBoard(char[,] board, TetrisBlock currentBlock)
    {
        var displayBoard = currentBlock.AddToBoard(board);
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
    }
}
