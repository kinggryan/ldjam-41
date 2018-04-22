using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour {

    private TetrisDisplay display;
    private TwineTextPlayer twinePlayer;

    private int boardSizeX = 10;
    private int boardSizeY = 24;

    private char[,] tetrisBoard;
    private TetrisBlock currentBlock;

    private float gameStepsDuration = 1;
    private float gameStepTimer;
    private float lineCompleteHangDuration = 1.5f;
    private SoundEngine soundEngine;

	// Use this for initialization
	void Start () {
        display = Object.FindObjectOfType<TetrisDisplay>();
        twinePlayer = Object.FindObjectOfType<TwineTextPlayer>();
        soundEngine = Object.FindObjectOfType<SoundEngine>();

        tetrisBoard = new char[boardSizeX, boardSizeY];
        for (var x = 0; x < boardSizeX; x++)
        {
            for (var y = 0; y < boardSizeY; y++)
            {
                tetrisBoard[x, y] = ' ';
            }
        }

        currentBlock = GetNextBlock();
        display.UpdateBoard(tetrisBoard, currentBlock);
        gameStepTimer = gameStepsDuration;
    }
	
	// Update is called once per frame
	void Update () {
        gameStepTimer -= Time.deltaTime;
        if(gameStepTimer <= 0)
        {
            gameStepTimer += gameStepsDuration;
            PerformNextGameStep();
        }

        PerformPlayerControlledMovement();
	}

    void PerformPlayerControlledMovement()
    {
        if(currentBlock == null)
        {
            return;
        }

        if(Input.GetButtonDown("rotateclockwise"))
        {
            currentBlock.RotateClockwise(tetrisBoard);
            soundEngine.PlaySoundWithName("BlockRotate");
        }
        if (Input.GetButtonDown("rotatecounterclockwise"))
        {
            currentBlock.RotateCounterClockwise(tetrisBoard);
            soundEngine.PlaySoundWithName("BlockRotate");
        }

        if (Input.GetButtonDown("left"))
        {
            currentBlock.MoveLeft(tetrisBoard);
            soundEngine.PlaySoundWithName("BlockMove");
        }

        if(Input.GetButtonDown("right"))
        {
            currentBlock.MoveRight(tetrisBoard);
            soundEngine.PlaySoundWithName("BlockMove");
        }

        if(Input.GetButtonDown("up"))
        {
            currentBlock.MoveDownMax(tetrisBoard);
        }
        else if(Input.GetButtonDown("down"))
        {
            PerformNextDownwardMove();
            soundEngine.PlaySoundWithName("BlockMove");
        }

        display.UpdateBoard(tetrisBoard, currentBlock);
    }

    void PerformNextGameStep()
    {
        if (currentBlock == null)
            currentBlock = GetNextBlock();

        PerformNextDownwardMove();
    }

    void PerformNextDownwardMove()
    {
        if (currentBlock == null)
            return;

        bool blockCanKeepMoving = false;
        bool shouldUpdateBoard = true;
        
        blockCanKeepMoving = currentBlock.MoveDown(tetrisBoard);
        if(!blockCanKeepMoving)
        {
            
            tetrisBoard = currentBlock.AddToBoard(tetrisBoard);
            soundEngine.PlaySoundWithName("BlockLand");
            currentBlock = null;
            if(CheckForCompleteLines())
            {
                gameStepTimer += lineCompleteHangDuration;
                shouldUpdateBoard = false;
            }
        }

        if(shouldUpdateBoard)
            display.UpdateBoard(tetrisBoard, currentBlock);
        
    }


    TetrisBlock GetNextBlock()
    {
        // Update the letters table to be based on the current possible commands.
        LetterGenerator.UpdateWithPossibleCommands(twinePlayer.GetCurrentPossibleCommands());

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

    bool CheckForCompleteLines()
    {
        // Check for complete lines
        // Go from the top down so we don't have to recheck lines if they move down
        var didEraseLine = false;
        for (var y = boardSizeY - 1; y >= 0; y--)
        {
            var command = GetCommandFromLine(y);

            if (command.command != TwineTextPlayer.Command.None)
            {
                display.UpdateBoardWithCommandOnLine(tetrisBoard, command.name, y);
            }

            if(IsLineComplete(y))
            {
                display.UpdateBoardWithCompleteLine(tetrisBoard, y);
            }

            // Do something with the command
            if (command.command != TwineTextPlayer.Command.None || IsLineComplete(y))
            {
                RemoveLineAndMoveAboveLinesDown(y);
                didEraseLine = true;
            }

            if (command.command != TwineTextPlayer.Command.None)
            {
                twinePlayer.DoCommand(command.command);
                return true;
            }
        }

        return didEraseLine;
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

    LetterGenerator.WeightedCommand GetCommandFromLine(int yCoord)
    {
        var lineString = "";

        // Do some stuff to find teh commands
        for (var x = 0; x < boardSizeX; x++)
        {
            if(tetrisBoard[x,yCoord] != ' ')
            {
                lineString += tetrisBoard[x, yCoord];
            }
        }

        if (lineString.Length != 0)
        {
            foreach (var command in LetterGenerator.weightedCommandsList)
            {
                if (lineString.Contains(command.name))
                {
                    return command;
                }
            }
        }

        return new LetterGenerator.WeightedCommand(TwineTextPlayer.Command.None, "", 0);
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
        soundEngine.PlaySoundWithName("LineClear");
    }
}