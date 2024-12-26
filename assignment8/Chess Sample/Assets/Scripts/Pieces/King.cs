using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// King.cs
public class King : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
            new MoveInfo(1, 0, 1),  // Right
            new MoveInfo(-1, 0, 1), // Left
            new MoveInfo(0, 1, 1),  // Up
            new MoveInfo(0, -1, 1), // Down
            new MoveInfo(1, 1, 1),  // Up-Right Diagonal
            new MoveInfo(-1, 1, 1), // Up-Left Diagonal
            new MoveInfo(1, -1, 1), // Down-Right Diagonal
            new MoveInfo(-1, -1, 1) // Down-Left Diagonal
        };
        // ------
    }
}
