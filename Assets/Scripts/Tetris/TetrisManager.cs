using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour {

    struct CommandReturnTuple
    {
        public readonly LetterGenerator.WeightedCommand command;
        public readonly string word;

        public CommandReturnTuple(LetterGenerator.WeightedCommand command, string word)
        {
            this.command = command;
            this.word = word;
        }
    }

    public bool playingEnabled;

    private TetrisDisplay display;
    private TextrisTwinePlayer twinePlayer;

    private int boardSizeX = 10;
    public int boardSizeY = 26;

    private char[,] tetrisBoard;
    private TetrisBlock currentBlock;

    private float gameStepsDuration = 1;
    private float gameStepTimer;
    private float lineCompleteHangDuration = 1.5f;
    private SoundEngine soundEngine;

	// Use this for initialization
	void Start () {
        display = Object.FindObjectOfType<TetrisDisplay>();
        twinePlayer = Object.FindObjectOfType<TextrisTwinePlayer>();
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
        if(playingEnabled)
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

        if(CheckForCompleteColumns() || CheckForDeathInTwine()){
            LoseGame();
        }

        if(CheckForWinInTwine())
        {
                WinGame();
        }
        
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
            if(currentBlock.IsBlockAbovePlayArea(boardSizeY)){
                LoseGame();
            }
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

            if (command.command.command != TwineTextPlayer.Command.None)
            {
                display.UpdateBoardWithCommandOnLine(tetrisBoard, command.word, y);
            }

            if(IsLineComplete(y))
            {
                display.UpdateBoardWithCompleteLine(tetrisBoard, y);
            }

            // Do something with the command
            if (command.command.command != TwineTextPlayer.Command.None || IsLineComplete(y))
            {
                RemoveLineAndMoveAboveLinesDown(y);
                didEraseLine = true;
            }

            if (command.command.command != TwineTextPlayer.Command.None)
            {
                twinePlayer.TypeCommand(command.word);
                twinePlayer.DoCommand(command.command.command);
                return true;
            }
        }

        return didEraseLine;
    }

        bool CheckForCompleteColumns()
        {
        // Check for complete lines
        // Go from the top down so we don't have to recheck lines if they move down
        var didEraseColumn = false;
        for (var x = boardSizeX - 1; x >= 0; x--)
        {
            var command = GetCommandFromColumn(x);

            if (command.command.command != TwineTextPlayer.Command.None)
            {
                display.UpdateBoardWithCommandOnLine(tetrisBoard, command.word, x);
            }

            if(IsColumnComplete(x))
            {
                display.UpdateBoardWithCompleteLine(tetrisBoard, x);
            }

            // Do something with the command
            if (command.command.command != TwineTextPlayer.Command.None || IsColumnComplete(x))
            {
                //RemoveLineAndMoveAboveLinesDown(y);
                didEraseColumn = true;
            }

            if (command.command.command != TwineTextPlayer.Command.None)
            {
                twinePlayer.TypeCommand(command.word);
                twinePlayer.DoCommand(command.command.command);
                return true;
            }
        }

        return didEraseColumn;
    }

    bool CheckForDeathInTwine()
    {
        return twinePlayer.GetTwineVarState("game_over");     
    }

    bool CheckForWinInTwine()
    {
        return twinePlayer.GetTwineVarState("win");
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

  

    bool IsColumnComplete(int xCoord)
        {
        // Debug.Log("Line " + yCoord);
            for(var y = 0; y < boardSizeY; y++)
            {
            // Debug.Log("is " + tetrisBoard[x, yCoord]);
                if(tetrisBoard[xCoord,y] == ' ')
                {
                    return false;
                }
            }

            return true;
        }

    void LoseGame(){
        Debug.Log("GAME OVER");
        Application.LoadLevel("Lose");
    }

    void WinGame(){
        Debug.Log("YOU WON");
        Application.LoadLevel("Win");
    }


    CommandReturnTuple GetCommandFromLine(int yCoord)
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
                var wasFound = lineString.Contains(command.name);
                var textToUse = command.name;
                foreach(var alias in command.aliases)
                {
                    if(wasFound)
                    {
                        break;
                    }

                    if(lineString.Contains(alias))
                    {
                        textToUse = alias;
                        wasFound = true;
                    }
                }

                if (wasFound)
                {
                    //twinePlayer.TypeCommand(textToUse);
                    return new CommandReturnTuple(command, textToUse);
                }
                
            }
        }

        return new CommandReturnTuple( new LetterGenerator.WeightedCommand(
            TwineTextPlayer.Command.None, "", 0, new string[] { }), "" );
    }

    CommandReturnTuple GetCommandFromColumn(int xCoord)
    {
        var columnString = "";

        // Do some stuff to find teh commands
        for (var y = 0; y < boardSizeX; y++)
        {
            if(tetrisBoard[xCoord, y] != ' ')
            {
                columnString += tetrisBoard[xCoord, y];
            }
        }

        if (columnString.Length != 0)
        {
            foreach (var command in LetterGenerator.weightedCommandsList)
            {
                var wasFound = columnString.Contains(command.name);
                var textToUse = command.name;
                foreach(var alias in command.aliases)
                {
                    if(wasFound)
                    {
                        break;
                    }

                    if(columnString.Contains(alias))
                    {
                        textToUse = alias;
                        wasFound = true;
                    }
                }

                if (wasFound)
                {
                    //twinePlayer.TypeCommand(textToUse);
                    return new CommandReturnTuple(command, textToUse);
                }
            }
        }

        return new CommandReturnTuple( new LetterGenerator.WeightedCommand(
            TwineTextPlayer.Command.None, "", 0, new string[] { }), "" );
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