using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 유니티에서 빈 GameObject 만들고 이 스크립트를 붙이세요!
public class ChessAIManager : MonoBehaviour
{
    public static int currentTurn = 1; // 1=백(플레이어), 2=흑(AI)
    private bool aiThinking = false;

    [Header("AI 설정")]
    public float aiThinkTime = 1f; // AI 생각하는 시간

    void Update()
    {
        // AI 차례가 되면 자동 실행
        if (currentTurn == 2 && !aiThinking)
        {
            StartCoroutine(AITurn());
        }
    }

    // === AI 턴 실행 ===
    IEnumerator AITurn()
    {
        aiThinking = true;
        yield return new WaitForSeconds(aiThinkTime);

        Debug.Log(" AI 차례!");

        // 1. 모든 흑 기물 찾기
        List<board11> blackPieces = FindAllBlackPieces();

        if (blackPieces.Count == 0)
        {
            Debug.Log("흑 기물이 없습니다!");
            aiThinking = false;
            yield break;
        }

        // 2. 이동할 수 있는 기물 찾을 때까지 시도
        bool moveMade = false;
        int attempts = 0;

        while (!moveMade && attempts < 100)
        {
            attempts++;

            // 랜덤으로 기물 선택
            board11 selectedPiece = blackPieces[Random.Range(0, blackPieces.Count)];

            // 이동 가능한 칸 찾기
            List<board11> possibleMoves = GetPossibleMoves(selectedPiece);

            if (possibleMoves.Count > 0)
            {
                // 랜덤으로 이동할 칸 선택
                board11 targetSquare = possibleMoves[Random.Range(0, possibleMoves.Count)];

                Debug.Log($"AI 이동: ({selectedPiece.row},{selectedPiece.col}) → ({targetSquare.row},{targetSquare.col})");

                // 기물 이동 실행
                MovePiece(selectedPiece, targetSquare);
                moveMade = true;
            }
        }

        if (!moveMade)
        {
            Debug.Log("AI가 이동할 수 없습니다! (체크메이트?)");
        }

        // 3. 백 차례로 변경
        currentTurn = 1;
        aiThinking = false;

        Debug.Log(" 플레이어 차례!");
    }

    // === 모든 흑 기물 찾기 ===
    List<board11> FindAllBlackPieces()
    {
        List<board11> blackPieces = new List<board11>();
        board11[] allBoards = GameObject.FindObjectsOfType<board11>();

        foreach (board11 cell in allBoards)
        {
            if (cell.color == 2 && cell.piece1 != 0)
            {
                blackPieces.Add(cell);
            }
        }

        return blackPieces;
    }

    // === 이동 가능한 칸 계산 ===
    List<board11> GetPossibleMoves(board11 piece)
    {
        List<board11> moves = new List<board11>();

        switch (piece.piece1)
        {
            case 1: // 폰
                CheckPawnMoves(piece, moves);
                break;
            case 2: // 비숍
                CheckBishopMoves(piece, moves);
                break;
            case 3: // 나이트
                CheckKnightMoves(piece, moves);
                break;
            case 4: // 룩
                CheckRookMoves(piece, moves);
                break;
            case 5: // 퀸
                CheckQueenMoves(piece, moves);
                break;
            case 6: // 킹
                CheckKingMoves(piece, moves);
                break;
        }

        return moves;
    }

    // === 폰 이동 ===
    void CheckPawnMoves(board11 piece, List<board11> moves)
    {
        int direction = (piece.color == 1) ? 1 : -1; // 백은 위로, 흑은 아래로

        // 전진 1칸
        board11 forward = FindSquare(piece.row + direction, piece.col);
        if (forward != null && forward.piece1 == 0)
        {
            moves.Add(forward);
        }

        // 대각선 공격 (왼쪽)
        board11 attackLeft = FindSquare(piece.row + direction, piece.col - 1);
        if (attackLeft != null && attackLeft.piece1 != 0 && attackLeft.color != piece.color)
        {
            moves.Add(attackLeft);
        }

        // 대각선 공격 (오른쪽)
        board11 attackRight = FindSquare(piece.row + direction, piece.col + 1);
        if (attackRight != null && attackRight.piece1 != 0 && attackRight.color != piece.color)
        {
            moves.Add(attackRight);
        }
    }

    // === 나이트 이동 ===
    void CheckKnightMoves(board11 piece, List<board11> moves)
    {
        int[,] knightMoves = new int[,]
        {
            { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
            { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
        };

        for (int i = 0; i < knightMoves.GetLength(0); i++)
        {
            int targetRow = piece.row + knightMoves[i, 0];
            int targetCol = piece.col + knightMoves[i, 1];

            board11 target = FindSquare(targetRow, targetCol);
            if (target != null && (target.piece1 == 0 || target.color != piece.color))
            {
                moves.Add(target);
            }
        }
    }

    // === 비숍 이동 ===
    void CheckBishopMoves(board11 piece, List<board11> moves)
    {
        CheckLineMoves(piece, moves, 1, 1);   // 오른쪽 위
        CheckLineMoves(piece, moves, 1, -1);  // 왼쪽 위
        CheckLineMoves(piece, moves, -1, 1);  // 오른쪽 아래
        CheckLineMoves(piece, moves, -1, -1); // 왼쪽 아래
    }

    // === 룩 이동 ===
    void CheckRookMoves(board11 piece, List<board11> moves)
    {
        CheckLineMoves(piece, moves, 1, 0);   // 위
        CheckLineMoves(piece, moves, -1, 0);  // 아래
        CheckLineMoves(piece, moves, 0, 1);   // 오른쪽
        CheckLineMoves(piece, moves, 0, -1);  // 왼쪽
    }

    // === 퀸 이동 (비숍 + 룩) ===
    void CheckQueenMoves(board11 piece, List<board11> moves)
    {
        CheckBishopMoves(piece, moves);
        CheckRookMoves(piece, moves);
    }

    // === 킹 이동 ===
    void CheckKingMoves(board11 piece, List<board11> moves)
    {
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;

                board11 target = FindSquare(piece.row + dr, piece.col + dc);
                if (target != null && (target.piece1 == 0 || target.color != piece.color))
                {
                    moves.Add(target);
                }
            }
        }
    }

    // === 직선 이동 체크 (비숍, 룩, 퀸 공통) ===
    void CheckLineMoves(board11 piece, List<board11> moves, int rowDir, int colDir)
    {
        for (int i = 1; i <= 8; i++)
        {
            int targetRow = piece.row + (rowDir * i);
            int targetCol = piece.col + (colDir * i);

            board11 target = FindSquare(targetRow, targetCol);

            if (target == null) break; // 보드 밖

            if (target.piece1 == 0)
            {
                moves.Add(target); // 빈 칸 - 계속 진행
            }
            else if (target.color != piece.color)
            {
                moves.Add(target); // 적 기물 - 먹고 멈춤
                break;
            }
            else
            {
                break; // 아군 기물 - 막힘
            }
        }
    }

    // === 특정 위치의 칸 찾기 ===
    board11 FindSquare(int row, int col)
    {
        if (row < 1 || row > 8 || col < 1 || col > 8)
            return null;

        string targetName = row.ToString() + col.ToString();
        GameObject target = GameObject.Find(targetName);

        if (target != null)
        {
            return target.GetComponent<board11>();
        }

        return null;
    }

    // === 기물 이동 실행 ===
    void MovePiece(board11 from, board11 to)
    {
        // 목적지에 기물 복사
        to.piece1 = from.piece1;
        to.color = from.color;

        // 원래 칸 비우기
        from.piece1 = 0;
        from.color = 0;

        // 시각적 업데이트 (색 되돌리기)
        Renderer fromRend = from.GetComponent<Renderer>();
        if (from.judge % 2 == 0)
            fromRend.material.color = Color.black;
        else
            fromRend.material.color = Color.white;

        Renderer toRend = to.GetComponent<Renderer>();
        if (to.judge % 2 == 0)
            toRend.material.color = Color.black;
        else
            toRend.material.color = Color.white;
    }
}