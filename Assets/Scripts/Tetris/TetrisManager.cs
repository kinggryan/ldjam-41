using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour {

    private TetrisDisplay display;

    private int boardSizeX = 10;
    private int boardSizeY = 18;

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
        if(Input.GetButtonDown("rotateclockwise"))
        {
            currentBlock.RotateClockwise(tetrisBoard);
        }
        if (Input.GetButtonDown("rotatecounterclockwise"))
        {
            currentBlock.RotateCounterClockwise(tetrisBoard);
        }

        if (Input.GetButtonDown("left"))
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
            PerformNextDownwardMove();
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
        var spawnX = boardSizeX / 2;
        var spawnY = boardSizeY - 3;
        var randomNum = Random.value;
        var numberOfBlockTypes = 7;

        if (randomNum < 1f / numberOfBlockTypes)
        {
            return new SquareBlock(spawnX, spawnY);
        }
        else if (randomNum < 2* (1f / numberOfBlockTypes))
        {
            return new LongBlock(spawnX, spawnY);
        }
        else if (randomNum < 3 * (1f / numberOfBlockTypes))
        {
            return new TBlock(spawnX, spawnY);
        }
        else if (randomNum < 4 * (1f / numberOfBlockTypes))
        {
            return new ZBlock(spawnX, spawnY);
        }
        else if (randomNum < 5 * (1f / numberOfBlockTypes))
        {
            return new ReverseZBlock(spawnX, spawnY);
        }
        else if (randomNum < 6 * (1f / numberOfBlockTypes))
        {
            return new LBlock(spawnX, spawnY);
        } else
        {
            return new ReverseLBlock(spawnX, spawnY);
        }
    }

    void CheckForCompleteLines()
    {
        // Check for complete lines
        // Go from the top down so we don't have to recheck lines if they move down
        for (var y = boardSizeY - 1; y >= 0; y--)
        {
            var command = GetCommandFromLine(y);
            // Do something with the command
            if (command != Command.None || IsLineComplete(y))
            {
                RemoveLineAndMoveAboveLinesDown(y);
            }
        }
    }

    bool IsLineComplete(int yCoord)
    {
       // Debug.Log("Line " + yCoord);
        for(var x = 0; x < boardSizeX; x++)
        {
           // Debug.Log("is " + tetrisBoard[x, yCoord]);
            if(tetrisBoard[x,yCoord] == ' ')
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