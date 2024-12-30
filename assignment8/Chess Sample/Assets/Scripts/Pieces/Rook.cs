using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rook.cs
public class Rook : Piece
{
    public override MoveInfo[] GetMoves()
    {
        return new MoveInfo[]
            {
            new MoveInfo(1, 0, int.MaxValue),   // 오른쪽으로 무제한
            new MoveInfo(-1, 0, int.MaxValue),  // 왼쪽으로 무제한
            new MoveInfo(0, 1, int.MaxValue),   // 위로 무제한
            new MoveInfo(0, -1, int.MaxValue)   // 아래로 무제한
            };
    }
}
