using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour {

    private TetrisDisplay display;

    private int boardSizeX = 10;
    private int boardSizeY = 24;

    private char[,] tetrisBoard;
    private TetrisBlock currentBlock;

    private float gameStepsDuration = 1;
    private float gameStepTimer = 0;

	// Use this for initialization
	void Start () {
        display = Object.FindObjectOfType<TetrisDisplay>();
        tetrisBoard = new char[boardSizeX, boardSizeY];
        for (var x = 0; x < boardSizeX; x++)
        {
            for (var y = 0; y < boardSizeY; y++)
            {
                tetrisBoard[x, y] = ' ';
            }
        }
        currentBlock = new SquareBlock(0, boardSizeY - 2);
        display.UpdateBoard(tetrisBoard, currentBlock);
    }
	
	// Update is called once per frame
	void Update () {
        gameStepTimer += Time.deltaTime;
        if(gameStepTimer >= gameStepsDuration)
        {
            gameStepTimer -= gameStepsDuration;
            PerformNextDownwardMove();
        }

        PerformPlayerControlledMovement();
	}

    void PerformPlayerControlledMovement()
    {
        if(Input.GetButtonDown("left"))
        {
            currentBlock.MoveLeft(tetrisBoard);
        }

        if(Input.GetButtonDown("right"))
        {
            currentBlock.MoveRight(tetrisBoard);
        }

        if(Input.GetButtonDown("up"))
        {
            currentBlock.MoveDownMax(tetrisBoard);
            currentBlock = GetNextBlock();
        }
        else if(Input.GetButtonDown("down"))
        {
            currentBlock.MoveDown(tetrisBoard);
        }

        display.UpdateBoard(tetrisBoard, currentBlock);
    }

    void PerformNextDownwardMove()
    {
        bool blockCanKeepMoving = false;
        blockCanKeepMoving = currentBlock.MoveDown(tetrisBoard);
        if(!blockCanKeepMoving)
        {
            tetrisBoard = currentBlock.AddToBoard(tetrisBoard);
            currentBlock = GetNextBlock();
            CheckForCompleteLines();
        }

        display.UpdateBoard(tetrisBoard, currentBlock);
    }

    TetrisBlock GetNextBlock()
    {
        return new TetrisBlock();
    }

    void CheckForCompleteLines()
    {
        // Check for complete lines
        // Go from the top down so we don't have to recheck lines if they move down
        for (var y = boardSizeY; y >= 0; y--)
        {
            var command = GetCommandFromLine(y);
            // Do something with the command
            if (command == Command.None || IsLineComplete(y))
            {
                RemoveLineAndMoveAboveLinesDown(y);
            }
        }
    }

    bool IsLineComplete(int yCoord)
    {
        for(var i = 0; i < boardSizeX; i++)
        {
            if(tetrisBoard[i,yCoord] == ' ')
            {
                return false;
            }
        }

        return true;
    }

    Command GetCommandFromLine(int yCoord)
    {
        // Do some stuff to find teh commands
        return Command.None;
    }

    void RemoveLineAndMoveAboveLinesDown(int yCoord)
    {
        for(var y = yCoord; y < boardSizeY - 1;  y++)
        {
            for(var x = 0; x < boardSizeX; x++)
            {
                tetrisBoard[x, y] = tetrisBoard[x, y + 1];
            }
        }
    }
}
