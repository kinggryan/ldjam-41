using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockQueueDisplay : MonoBehaviour {

	UnityEngine.UI.Text blockQueueDisplay;

	// Use this for initialization
	void Start () {
		blockQueueDisplay = GetComponent<UnityEngine.UI.Text>();

	}
	
	public void SetNextBlock(TetrisBlock block) {
		// Draw the block
		var blockQueueDisplayText = new char[,]{ {' ', ' ', ' ', ' '},
												{' ', ' ', ' ', ' '},
												{' ', ' ', ' ', ' '} };
		blockQueueDisplayText = block.AddToBoard(blockQueueDisplayText, true);
		var blockQueueText = "";
        for(var x = 0; x < blockQueueDisplayText.GetLength(0); x++)
        {
            for(var y = 0; y < blockQueueDisplayText.GetLength(1); y++)
            {
				var nextText = blockQueueDisplayText[x, y];
                blockQueueText += nextText == ' ' ? '.' : nextText;
            }
            blockQueueText += "\n";
        }
		blockQueueDisplay.text = blockQueueText;

		// Debug.Log("Adding to board:" + blockQueueText);
	}
}
