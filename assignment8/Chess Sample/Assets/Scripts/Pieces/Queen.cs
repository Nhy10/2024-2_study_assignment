using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // 직선, 대각선 무제한
        return new MoveInfo[]
        {
            // 직선 방향
            new MoveInfo(1, 0, int.MaxValue),   // 오른쪽
            new MoveInfo(-1, 0, int.MaxValue),  // 왼쪽
            new MoveInfo(0, 1, int.MaxValue),   // 위쪽
            new MoveInfo(0, -1, int.MaxValue),  // 아래쪽
            
            // 대각선 방향
            new MoveInfo(1, 1, int.MaxValue),   // 우상
            new MoveInfo(1, -1, int.MaxValue),  // 우하
            new MoveInfo(-1, 1, int.MaxValue),  // 좌상
            new MoveInfo(-1, -1, int.MaxValue)  // 좌하
        };
    }
}