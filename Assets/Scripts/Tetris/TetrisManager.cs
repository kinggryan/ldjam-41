using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisManager : MonoBehaviour {

    struct CommandReturnTuple
    {
        public readonly LetterGenerator.WeightedCommand command;
        public readonly string word;
        public readonly int coordinate; // This is for the purpose of determining the y coordinate of vertical commands. Not used for horizontal commands

        public CommandReturnTuple(LetterGenerator.WeightedCommand command, string word, int coordinate)
        {
            this.command = command;
            this.word = word;
            this.coordinate = coordinate;
        }

        public CommandReturnTuple(LetterGenerator.WeightedCommand command, string word)
        {
            this.command = command;
            this.word = word;
            this.coordinate = 0;
        }
    }

    public int playDelayFrames;

    private WakeUpTetris pauser;
    private TetrisDisplay display;
    public TetrisBlockQueueDisplay nextBlockDisplay;
    public TetrisBlockQueueDisplay stashBlockDisplay;
    private TextrisTwinePlayer twinePlayer;

    private int boardSizeX = 10;
    public int boardSizeY = 26;

    private char[,] tetrisBoard;
    private TetrisBlock currentBlock;
    private TetrisBlock[] nextBlocks;
    private TetrisBlock stashedBlock;
    private int numNextBlocks = 1;

    private float gameStepsDuration = 1;
    private float gameStepTimer;
    private float lineCompleteHangDuration = 1.5f;
    private SoundEngine soundEngine;
    private MusicEngine musicEngine;

	// Use this for initialization
	void Start () {
        display = Object.FindObjectOfType<TetrisDisplay>();
        twinePlayer = Object.FindObjectOfType<TextrisTwinePlayer>();
        soundEngine = Object.FindObjectOfType<SoundEngine>();
        musicEngine = Object.FindObjectOfType<MusicEngine>();
        pauser = Object.FindObjectOfType<WakeUpTetris>();
        // nextBlockDisplay = Object.FindObjectOfType<TetrisBlockQueueDisplay>();

        tetrisBoard = new char[boardSizeX, boardSizeY];
        for (var x = 0; x < boardSizeX; x++)
        {
            for (var y = 0; y < boardSizeY; y++)
            {
                tetrisBoard[x, y] = ' ';
            }
        }

        // Initialize the block queue
        nextBlocks = new TetrisBlock[numNextBlocks];
        for(var i = 0 ; i < numNextBlocks ; i++) {
            nextBlocks[i] = GetNextBlock();
        }
        UpdateNextBlocks();
        display.UpdateBoard(tetrisBoard, currentBlock);
        gameStepTimer = gameStepsDuration;
    }
	
	// Update is called once per frame
	void Update () {
        // The delay frames are negative when the game is paused and positive when pausing is ending.
        // This means that when 0, game play should proceed as normal
        if(playDelayFrames == 0)
        {
            gameStepTimer -= Time.deltaTime;
            if (gameStepTimer <= 0)
            {
                gameStepTimer += gameStepsDuration;
                PerformNextGameStep();
            }

            PerformPlayerControlledMovement();
        }
        else if(playDelayFrames > 0)
        {
            playDelayFrames -= 1;
        }
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

        if(Input.GetButtonDown("stash")) {
            StashCurrentBlock();
        }

        display.UpdateBoard(tetrisBoard, currentBlock);
    }

    void PerformNextGameStep()
    {
        if (currentBlock == null)
            UpdateNextBlocks();

        PerformNextDownwardMove();

        if(CheckForDeathInTwine()){
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
            if(CheckForCompleteLines() || CheckForCompleteColumns())
            {
                gameStepTimer += lineCompleteHangDuration;
                shouldUpdateBoard = false;
            }
        }

        if(shouldUpdateBoard)
            display.UpdateBoard(tetrisBoard, currentBlock);
        
    }

    // This stashes the current block and returns the next current block.
    TetrisBlock StashCurrentBlock() {
        // If there is no current block, then we can't do any stashing.
        if(currentBlock == null) {
            return currentBlock;
        }

        // If there's a stashed block, switch it and the current block.
        if(stashedBlock != null) {
            var retBlock = stashedBlock;
            retBlock.SetPosition(currentBlock.GetPositionX(), currentBlock.GetPositionY());
            stashedBlock = currentBlock;
            stashBlockDisplay.SetNextBlock(stashedBlock);
            currentBlock = retBlock;
            return currentBlock;
        } else {
            // Otherwise, put the current block in the stash and get the next block
            stashedBlock = currentBlock;
            stashBlockDisplay.SetNextBlock(stashedBlock);
            UpdateNextBlocks();
            return currentBlock;
        }
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
                soundEngine.PlaySoundWithName("LineClear");
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
                pauser.PausePlay();
                return true;
            }
        }

        return didEraseLine;
    }

    bool CheckForCompleteColumns()
    {
        //Debug.Log("Checking For Complete Columns");
        // Check for complete lines
        // Go from the top down so we don't have to recheck lines if they move down
        var didEraseColumn = false;
        for (var x = boardSizeX - 1; x >= 0; x--)
        {
            var command = GetCommandFromColumn(x);

            if (command.command.command != TwineTextPlayer.Command.None)
            {
                display.UpdateBoardWithCommandOnColumn(tetrisBoard, command.word, x);
                soundEngine.PlaySoundWithName("EnterCommand");
            }

            if(IsColumnComplete(x))
            {
                display.UpdateBoardWithCompleteColumn(tetrisBoard, x);
                soundEngine.PlaySoundWithName("LineClear");
            }

            // Do something with the command
            if (command.command.command != TwineTextPlayer.Command.None || IsColumnComplete(x))
            {
                Debug.Log("Command: " + command.command.command);
                Debug.Log("Command: " + command.command);
                RemoveMultipleLinesAndMoveAboveLinesDown(command.coordinate, command.word.Length);
                didEraseColumn = true;
            }

            if (command.command.command != TwineTextPlayer.Command.None)
            {
                Debug.Log("FUCKING PAUSE");
                twinePlayer.TypeCommand(command.word);
                twinePlayer.DoCommand(command.command.command);
                pauser.PausePlay();
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
        TurnDownAllInstrumentsExceptBass();
        SceneManager.LoadScene("Lose");
        
    }

    void TurnDownAllInstrumentsExceptBass(){
        musicEngine.ChangeMusicWithName("MelodyDown");
        musicEngine.ChangeMusicWithName("BloopsDown");
        musicEngine.ChangeMusicWithName("KickDown");
        musicEngine.ChangeMusicWithName("DrumsDown");
        musicEngine.ChangeMusicWithName("CymbalDown");
    }

    void WinGame(){
        Debug.Log("YOU WON");
		SceneManager.LoadScene("Win");
    }


    CommandReturnTuple GetCommandFromLine(int yCoord)
    {
        var lineString = "";

        // Do some stuff to find teh commands
        for (var x = 0; x < boardSizeX; x++)
        {
            // if(tetrisBoard[x,yCoord] != ' ')
            // {
                lineString += tetrisBoard[x, yCoord];
            // }
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
        //Debug.Log("Checking column " + xCoord + " for Commands");
        var columnString = "";

        // Do some stuff to find teh commands
        for (var y = boardSizeY - 1; y >= 0; y--)
        {
            columnString += tetrisBoard[xCoord, y];

            // if(tetrisBoard[xCoord, y] != ' ')
            // {
            //     columnString += tetrisBoard[xCoord, y];
            // }
        }

        if (columnString.Length != 0)
        {
            //Debug.Log("CollectionBase string:" + columnString);
            foreach (var command in LetterGenerator.weightedCommandsList)
            {
                var bottomY = columnString.IndexOf(command.name);
                var textToUse = command.name;
                var wasFound = bottomY > -1;

                if(!wasFound) {
                    foreach(var alias in command.aliases)
                    {
                        if(wasFound)
                        {
                            //Debug.Log("Command Found In Column " + xCoord);
                            break;
                        }

                        bottomY = columnString.IndexOf(alias);
                        if(bottomY >= 0)
                        {
                            textToUse = alias;
                            wasFound = true;
                        }
                    }
                }
                
                if (wasFound)
                {
                    bottomY = tetrisBoard.GetLength(1) - (bottomY + textToUse.Length);
                    Debug.Log("Found Index of " + bottomY + " in string " + columnString);
                    //twinePlayer.TypeCommand(textToUse);
                    return new CommandReturnTuple(command, textToUse, bottomY);
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
        //soundEngine.PlaySoundWithName("LineClear");
    }

    void RemoveColumnAndMoveAboveCharactersDown(int xCoord){
        Debug.Log("Trying to remove column " + xCoord);

        //for(var x = xCoord; x < boardSizeX - 1;  x++)
        //{
            //Debug.Log("X: " + xCoord);
            for(var y = boardSizeY - 2; y >= 0; y--)
            {
                //Debug.Log("Y: " + y);
                tetrisBoard[xCoord, y] = tetrisBoard[xCoord, y + 1];
                //Debug.Log("Moving x: " + xCoord + " y: " + y);
            }
        //}
        //soundEngine.PlaySoundWithName("LineClear");
    }

    void RemoveMultipleLinesAndMoveAboveLinesDown(int bottomY, int length) {
        Debug.Log("Removing with bottom y " + bottomY + " and length " + length);
        for(var i = 0; i < length; i++) {
            RemoveLineAndMoveAboveLinesDown(bottomY);
        }
    }

    public void PausePlay()
    {
        playDelayFrames = -1;
        musicEngine.PauseMusic();
        
    }

    public void ResumePlay()
    {
        playDelayFrames = 0;
        soundEngine.PlaySoundWithName("UnPause");
        musicEngine.UnpauseMusic();
    }

    public bool IsPaused()
    {
        return playDelayFrames == -1;
    }

	public void UpdateNextBlocks()
    {
        // set the current block
        currentBlock = nextBlocks[0];
        
        // Move the next blocks forward in the queue
        for(var i = 0 ; i < numNextBlocks-1 ; i++) {
            nextBlocks[i] = nextBlocks[i+1];
        }

        nextBlocks[numNextBlocks-1] = GetNextBlock();
        nextBlockDisplay.SetNextBlock(nextBlocks[0]);
    }
}