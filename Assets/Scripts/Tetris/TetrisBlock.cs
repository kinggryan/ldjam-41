using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock {

    protected char[,] blockLetters;
    protected int positionX;
    protected int positionY;
    protected int localOriginX;
    protected int localOriginY;
    protected int upperLeftCornerX;
    protected int upperleftCornerY;

    public virtual bool IsBlockAbovePlayArea(int playAreaHeight){
        // Debug.Log("Block position: " + positionY + " areaHeight: " + playAreaHeight);
        if(positionY >= playAreaHeight - 3){
            return true;
        }
        return false;
        

    }

    public virtual Vector2 GetCurrentBlockPosition(){
        return new Vector2 (positionX, positionY);
    }
	// Returns true if the rotation was successful
    public virtual bool RotateClockwise(char[,] board)
    {
        var newRotation = GetRotationOfBlockLetters(blockLetters, true);
        if (CanBeAtPosition(board, newRotation, positionX, positionY))
        {
            blockLetters = newRotation;
            return true;
        }
        return false;
    }

    // returns true if the rotation was successful
    public virtual bool RotateCounterClockwise(char[,] board)
    {
        var newRotation = GetRotationOfBlockLetters(blockLetters, false);
        if (CanBeAtPosition(board, newRotation, positionX, positionY))
        {
            blockLetters = newRotation;
            return true;
        }
        return false;
    }

    // returns true if the move down was successful
    public bool MoveDown(char[,] board)
    {
        if(CanBeAtPosition(board, blockLetters, positionX, positionY - 1))
        {
            positionY -= 1;
            return true;
        }

        return false;
    }

    // always works
    public void MoveDownMax(char[,] board)
    {
        // Move down as long as you can
        while (MoveDown(board)) ;
    }

    public bool MoveRight(char[,] board)
    {
        if (CanBeAtPosition(board, blockLetters, positionX + 1, positionY))
        {
            positionX += 1;
            return true;
        }
        return false;
    }

    public bool MoveLeft(char[,] board)
    {
        if(CanBeAtPosition(board, blockLetters, positionX - 1, positionY))
        {
            positionX -= 1;
            return true;
        }
        return false;
    }

    /// This function returns the supplied board with the given piece added at its position.
    /// If zeroed is true, then the block will be cornered in the uper left of the board.!-- 
    public char[,] AddToBoard(char[,] board, bool cornered = false)
    {
        var newBoard = (char[,])board.Clone();
        for (var localX = 0; localX < blockLetters.GetLength(0); localX++)
        {
            var x = localX + (cornered ? -upperLeftCornerX : (positionX - localOriginX));
            for (var localY = 0; localY < blockLetters.GetLength(1); localY++) {
                var y = localY + (cornered ? -upperleftCornerY : (positionY - localOriginY));
                if (blockLetters[localX, localY] != ' ') {
                    // Ensure you don't try to write things outside of the valid space
                    if(x >= 0 && x < newBoard.GetLength(0) && y >= 0 && y < newBoard.GetLength(1))
                        newBoard[x, y] = blockLetters[localX, localY];
                }
            }
        }

        return newBoard;
    }

    private bool CanBeAtPosition(char[,] board, char[,] blockLetters, int newPosX, int newPosY)
    {
        for (var localX = 0; localX < blockLetters.GetLength(0); localX++)
        {
            var x = newPosX - localOriginX + localX;
            for (var localY = 0; localY < blockLetters.GetLength(1); localY++)
            {
                if (blockLetters[localX, localY] == ' ')
                {
                    continue;
                }

                var y = newPosY - localOriginY + localY;
               // Debug.Log("For position " + newPosX + "," + newPosY + " checking at " + x + " y " + y + " with local (" + localX + "," + localY + ") and origin " + localOriginX + "," + localOriginY + " and position " +positionX+","+positionY);

                if (x < 0 || y < 0 || x >= board.GetLength(0))
                {
                    return false;
                }

                if(board[x, y] != ' ')
                {
                    return false;
                }

                
            }
        }

        return true;
    }

    private char[,] GetRotationOfBlockLetters(char [,] blockLetters, bool clockwise)
    {
        var rotatedBlock = (char[,])blockLetters.Clone();
        for(var x = 0; x < blockLetters.GetLength(0);x++)
        {
            for(var y = 0; y < blockLetters.GetLength(1); y++)
            {
                var localCoordX = x - localOriginX;
                var localCoordY = y - localOriginY;
                var rotatedCoordinate = Rotate(new Vector2(localCoordX, localCoordY), clockwise ? -90 : 90);
                var rotatedX = Mathf.RoundToInt(rotatedCoordinate.x) + localOriginX;
                var rotatedY = Mathf.RoundToInt(rotatedCoordinate.y) + localOriginY;
                rotatedBlock[rotatedX, rotatedY] = blockLetters[x, y];
            }
        }

        return rotatedBlock;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    protected void RandomizeLetters()
    {
        for (var x = 0; x < blockLetters.GetLength(0); x++)
        {
            for (var y = 0; y < blockLetters.GetLength(1); y++)
            {
                if (blockLetters[x, y] != ' ')
                {
                    blockLetters[x, y] = LetterGenerator.GetLetter();
                }
            }
        }
    }

    public void SetPosition(int x, int y) {
        positionX = x;
        positionY = y;
    }

    public int GetPositionX() {
        return positionX;
    }

    public int GetPositionY() {
        return positionY;
    }
}

public class SquareBlock : TetrisBlock {
    public SquareBlock(int positionX, int positionY)
    {
        var topLetters = LetterGenerator.GetSubstringFromCommands(2);
        var leftLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, topLetters[0]);
        var bottomLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, leftLetters[1]);
        blockLetters = new char[,] { { topLetters[0], topLetters[1] }, { bottomLetters[0], bottomLetters[1] } };
        localOriginX = 0;
        localOriginY = 0;
        upperLeftCornerX = 0;
        upperleftCornerY = 0;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }

    public override bool RotateClockwise(char[,] board)
    {
        var newLetters = (char[,])blockLetters.Clone();
        newLetters[0, 0] = blockLetters[1,0];
        newLetters[1, 0] = blockLetters[1, 1];
        newLetters[1, 1] = blockLetters[0, 1];
        newLetters[0, 1] = blockLetters[0, 0];
        blockLetters = newLetters;
        return true;
    }

    public override bool RotateCounterClockwise(char[,] board)
    {
        var newLetters = (char[,])blockLetters.Clone();
        newLetters[1, 0] = blockLetters[0, 0];
        newLetters[1, 1] = blockLetters[1, 0];
        newLetters[0, 1] = blockLetters[1, 1];
        newLetters[0, 0] = blockLetters[0, 1];
        blockLetters = newLetters;
        return true;
    }
}

public class TBlock : TetrisBlock
{
    public TBlock(int positionX, int positionY)
    {
        var middleLetters = LetterGenerator.GetSubstringFromCommands(3);
        var verticalLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, middleLetters[1]);
        blockLetters = new char[,] {
            { ' ', verticalLetters[1], ' '}, 
            { middleLetters[0], middleLetters[1], middleLetters[2] }, 
            { ' ', ' ', ' ' } };
        localOriginX = 1;
        localOriginY = 1;
        upperLeftCornerX = 0;
        upperleftCornerY = 0;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}

public class LongBlock : TetrisBlock
{
    public LongBlock(int positionX, int positionY)
    {
        var substrLetters = LetterGenerator.GetSubstringFromCommands(4);
        blockLetters = new char[,] {
            {' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' '},
            {' ', substrLetters[0], substrLetters[1], substrLetters[2], substrLetters[3]},
            {' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' '}};
        localOriginX = 2;
        localOriginY = 2;
        upperLeftCornerX = 2;
        upperleftCornerY = 1;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}

public class ZBlock : TetrisBlock
{
    public ZBlock(int positionX, int positionY)
    {
        var topLetters = LetterGenerator.GetSubstringFromCommands(2);
        var middleLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, topLetters[0]);
        var bottomLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, middleLetters[1]);
        blockLetters = new char[,] {
            {' ', topLetters[0], topLetters[1]},
            {bottomLetters[1], bottomLetters[0], ' '},
            {' ', ' ', ' '}};
        localOriginX = 1;
        localOriginY = 1;
        upperLeftCornerX = 0;
        upperleftCornerY = 0;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}

public class ReverseZBlock : TetrisBlock
{
    public ReverseZBlock(int positionX, int positionY)
    {
        var topLetters = LetterGenerator.GetSubstringFromCommands(2);
        var middleLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, topLetters[0]);
        var bottomLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, middleLetters[1]);
        blockLetters = new char[,] {
            {topLetters[0], topLetters[1], ' '},
            {' ', bottomLetters[1], bottomLetters[0]},
            {' ', ' ', ' '}};
        localOriginX = 1;
        localOriginY = 1;
        upperLeftCornerX = 0;
        upperleftCornerY = 0;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}

public class LBlock : TetrisBlock
{
    public LBlock(int positionX, int positionY)
    {
        var middleLetters = LetterGenerator.GetSubstringFromCommands(3);
        var bottomLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, middleLetters[2]);
        blockLetters = new char[,] {
            {' ', middleLetters[0], ' '},
            {' ', middleLetters[1], ' '},
            {' ', middleLetters[2], bottomLetters[1]}};
        localOriginX = 1;
        localOriginY = 1;
        upperLeftCornerX = 0;
        upperleftCornerY = 0;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}


public class ReverseLBlock : TetrisBlock
{
    public ReverseLBlock(int positionX, int positionY)
    {
        var middleLetters = LetterGenerator.GetSubstringFromCommands(3);
        var bottomLetters = LetterGenerator.GetSubstringFromCommandsIncludingLetterAtStart(2, middleLetters[2]);
        blockLetters = new char[,] {
            {' ', middleLetters[0], ' '},
            {' ', middleLetters[1], ' '},
            {bottomLetters[1], middleLetters[2], ' '}};
        localOriginX = 1;
        localOriginY = 1;
        upperLeftCornerX = 0;
        upperleftCornerY = 0;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}