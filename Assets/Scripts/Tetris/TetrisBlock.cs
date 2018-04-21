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
    public bool RotateLeft(char[,] board)
    {
        return true;
    }

    // returns true if the rotation was successful
    public bool RotateRight(char[,] board)
    {
        return true;
    }

    // returns true if the move down was successful
    public bool MoveDown(char[,] board)
    {
        if(CanBeAtPosition(board, positionX, positionY - 1))
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
        if (positionX == 8)
        {
            Debug.Log("stop");
        }
        if (CanBeAtPosition(board, positionX + 1, positionY))
        {
            positionX += 1;
            return true;
        }
        return false;
    }

    public bool MoveLeft(char[,] board)
    {
        if(CanBeAtPosition(board, positionX - 1, positionY))
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

    private bool CanBeAtPosition(char[,] board, int newPosX, int newPosY)
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
}

public class SquareBlock : TetrisBlock {
    public SquareBlock(int positionX, int positionY)
    {
        blockLetters = new char[,] { { 'X', 'X' }, { 'X', 'X' } };
        localOriginX = 0;
        localOriginY = 0;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}