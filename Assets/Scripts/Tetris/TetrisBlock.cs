using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock {

    protected char[,] blockLetters;
    protected int positionX;
    protected int positionY;
    protected int localOriginX;
    protected int localOriginY;

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

    public char[,] AddToBoard(char[,] board)
    {
        var newBoard = (char[,])board.Clone();
        for (var localX = 0; localX < blockLetters.GetLength(0); localX++)
        {
            var x = positionX - localOriginX + localX;
            for (var localY = 0; localY < blockLetters.GetLength(1); localY++) {
                var y = positionY - localOriginY + localY;
                if (blockLetters[localX, localY] != ' ') {
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
}

public class SquareBlock : TetrisBlock {
    public SquareBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] { { 'X', 'X' }, { 'X', 'X' } };
        localOriginX = 0;
        localOriginY = 0;
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
        blockLetters = new char[,] {
            { ' ', 'X', ' '}, 
            { 'X', 'X', 'X' }, 
            { ' ', ' ', ' ' } };
        localOriginX = 1;
        localOriginY = 1;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }
}

public class LongBlock : TetrisBlock
{
    public LongBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] {
            {' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' '},
            {' ', 'X', 'X', 'X', 'X'},
            {' ', ' ', ' ', ' ', ' '},
            {' ', ' ', ' ', ' ', ' '}};
        localOriginX = 2;
        localOriginY = 2;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }
}

public class ZBlock : TetrisBlock
{
    public ZBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] {
            {' ', 'X', 'X'},
            {'X', 'X', ' '},
            {' ', ' ', ' '}};
        localOriginX = 1;
        localOriginY = 1;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }
}

public class ReverseZBlock : TetrisBlock
{
    public ReverseZBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] {
            {'X', 'X', ' '},
            {' ', 'X', 'X'},
            {' ', ' ', ' '}};
        localOriginX = 1;
        localOriginY = 1;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }
}

public class LBlock : TetrisBlock
{
    public LBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] {
            {' ', 'X', ' '},
            {' ', 'X', ' '},
            {' ', 'X', 'X'}};
        localOriginX = 1;
        localOriginY = 1;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }
}


public class ReverseLBlock : TetrisBlock
{
    public ReverseLBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] {
            {' ', 'X', ' '},
            {' ', 'X', ' '},
            {'X', 'X', ' '}};
        localOriginX = 1;
        localOriginY = 1;
        this.positionX = positionX;
        this.positionY = positionY;

        RandomizeLetters();
    }
}