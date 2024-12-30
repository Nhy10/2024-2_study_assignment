using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject effectPrefab;
    private Transform effectParent;
    private List<GameObject> currentEffects = new List<GameObject>();   // 현재 effect들을 저장할 리스트
    
    public void Initialize(GameManager gameManager, GameObject effectPrefab, Transform effectParent)
    {
        this.gameManager = gameManager;
        this.effectPrefab = effectPrefab;
        this.effectParent = effectParent;
    }

    private bool TryMove(Piece piece, (int, int) targetPos, MoveInfo moveInfo)
    {
        (int, int) currentPos = piece.MyPos;

        for (int step = 1; step <= moveInfo.distance; step++)
        {
            (int, int) nextPos = (currentPos.Item1 + step * moveInfo.dirX, currentPos.Item2 + step * moveInfo.dirY);

            if (!Utils.IsInBoard(nextPos)) return false;

            var targetPiece = gameManager.Pieces[nextPos.Item1, nextPos.Item2];
            if (targetPiece != null)
            {
                if (targetPiece.PlayerDirection == piece.PlayerDirection) return false;

                if (step == moveInfo.distance) return true; // Capturing move
                return false;
            }

            if (step == moveInfo.distance) return true;
        }

        return false;
    }

    // 체크를 제외한 상황에서 가능한 움직임인지를 검증
    private bool IsValidMoveWithoutCheck(Piece piece, (int, int) targetPos)
    {
        if (!Utils.IsInBoard(targetPos) || targetPos == piece.MyPos) return false;

        foreach (var moveInfo in piece.GetMoves())
        {
            if (TryMove(piece, targetPos, moveInfo))
                return true;
        }
        
        return false;
    }

    // 체크를 포함한 상황에서 가능한 움직임인지를 검증
    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMoveWithoutCheck(piece, targetPos)) return false;

        // 체크 상태 검증을 위한 임시 이동
        var originalPiece = gameManager.Pieces[targetPos.Item1, targetPos.Item2];
        var originalPos = piece.MyPos;

        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = piece;
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = null;
        piece.MyPos = targetPos;

        bool isValid = !IsInCheck(piece.PlayerDirection);

        // 원상 복구
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = piece;
        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = originalPiece;
        piece.MyPos = originalPos;

        return isValid;
    }

    // 체크인지를 확인
    private bool IsInCheck(int playerDirection)
    {
        (int, int) kingPos = (-1, -1); // 왕의 위치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece is King && piece.PlayerDirection == playerDirection)
                {
                    kingPos = (x, y);
                    break;
                }
            }
            if (kingPos.Item1 != -1 && kingPos.Item2 != -1) break;
        }

        // 왕이 지금 체크 상태인지를 리턴
        // gameManager.Pieces에서 Piece들을 참조하여 움직임을 확인
        foreach (var piece in gameManager.Pieces)
        {
            if (piece != null && piece.PlayerDirection != playerDirection)
            {
                foreach (var move in piece.GetMoves())
                {
                    (int, int) checkPos = (piece.MyPos.Item1 + move.dirX, piece.MyPos.Item2 + move.dirY);
                    if (checkPos == kingPos) return true;
                }
            }
        }


        return false;
    }

    public void ShowPossibleMoves(Piece piece)
    {
        ClearEffects();

        // 가능한 움직임을 표시
        // IsValidMove를 사용
        // effectPrefab을 effectParent의 자식으로 생성하고 위치를 적절히 설정
        // currentEffects에 effectPrefab을 추가
        foreach (var move in piece.GetMoves())
        {
            for (int step = 1; step <= move.distance; step++)
            {
                (int, int) nextPos = (piece.MyPos.Item1 + step * move.dirX, piece.MyPos.Item2 + step * move.dirY);
                if (!IsValidMove(piece, nextPos)) break;

                GameObject effect = Instantiate(effectPrefab, effectParent);
                effect.transform.position = Utils.ToRealPos(nextPos);
                currentEffects.Add(effect);

                if (gameManager.Pieces[nextPos.Item1, nextPos.Item2] != null) break;
            }
        }
    }

    // 효과 비우기
    public void ClearEffects()
    {
        foreach (var effect in currentEffects)
        {
            if (effect != null) Destroy(effect);
        }
        currentEffects.Clear();
    }
}