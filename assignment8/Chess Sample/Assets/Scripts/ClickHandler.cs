using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameManager gameManager;
    private Piece selectedPiece = null; // 지금 선택된 Piece
    private Vector3 dragOffset;
    private Vector3 originalPosition;
    private bool isDragging = false;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // 마우스의 위치를 (int, int) 좌표로 보정해주는 함수
    private (int, int) GetBoardPosition(Vector3 worldPosition)
    {
        float x = worldPosition.x + (Utils.TileSize * Utils.FieldWidth) / 2f;
        float y = worldPosition.y + (Utils.TileSize * Utils.FieldHeight) / 2f;
        
        int boardX = Mathf.FloorToInt(x / Utils.TileSize);
        int boardY = Mathf.FloorToInt(y / Utils.TileSize);
        
        return (boardX, boardY);
    }

    void HandleMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var boardPos = GetBoardPosition(mousePosition);

        if (!Utils.IsInBoard(boardPos)) return;
        Piece clickedPiece = gameManager.Pieces[boardPos.Item1, boardPos.Item2];
        if (clickedPiece != null && clickedPiece.PlayerDirection == gameManager.CurrentTurn)
        {
            selectedPiece = clickedPiece;
            isDragging = true;
            dragOffset = selectedPiece.transform.position - mousePosition;
            dragOffset.z = 0;
            originalPosition = selectedPiece.transform.position;
            
            gameManager.ShowPossibleMoves(selectedPiece);
        }
    }

    void HandleDrag()
    {
        if (selectedPiece != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            selectedPiece.transform.position = mousePosition + dragOffset;
        }
    }

    void HandleMouseUp()
    {
        if (selectedPiece != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var boardPos = GetBoardPosition(mousePosition); // 현재 좌표

            if (gameManager.IsValidMove(selectedPiece, boardPos))
            {
                gameManager.Move(selectedPiece, boardPos);
                gameManager.ChangeTurn();
            }
            else
            {
                selectedPiece.transform.position = originalPosition;
            }

            gameManager.ClearEffects();
            isDragging = false;
            selectedPiece = null;
        }
    }

    void Update()
    {
        // 입력 제어
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            HandleDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }
}