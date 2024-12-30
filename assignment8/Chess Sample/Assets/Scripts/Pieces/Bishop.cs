using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override MoveInfo[] GetMoves()
    {
        return new MoveInfo[]
        {
            new MoveInfo { dirX = 1, dirY = 1, distance = int.MaxValue },  // 우상, 대각선 제한 없음
            new MoveInfo { dirX = 1, dirY = -1, distance = int.MaxValue }, // 우하
            new MoveInfo { dirX = -1, dirY = 1, distance = int.MaxValue }, // 좌상
            new MoveInfo { dirX = -1, dirY = -1, distance = int.MaxValue } // 좌하
        };
    }
}

