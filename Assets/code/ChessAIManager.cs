using UnityEngine;
using System.Collections.Generic;

public class ChessAIManager : MonoBehaviour
{
    public static int currentTurn = 1; // 1 = 백(플레이어), 2 = 흑(AI)
    public static bool isThinking = false;

    void Update()
    {
        if (currentTurn == 2 && !isThinking)
        {
            isThinking = true;
            Invoke(nameof(AIMove), 0.5f);
        }
    }

    void AIMove()
    {
        board11[] all = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);
        foreach (var c in all)
        {
            c.realhandle = 0;
        }

        typeof(board11)
            .GetField("handle", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, 0);

        board11[] allCells = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);
        List<board11> blackPieces = new List<board11>();

        // 1️⃣ 흑 기물만 수집
        foreach (board11 cell in allCells)
        {
            if (cell.color == 2 && cell.piece1 != 0)
                blackPieces.Add(cell);
        }

        if (blackPieces.Count == 0)
        {
            Debug.Log("AI: 흑 기물 없음");
            EndAITurn();
            return;
        }

        // 2️⃣ 랜덤 흑 기물 선택
        board11 from = blackPieces[Random.Range(0, blackPieces.Count)];

        // 3️⃣ 이동 가능 칸 계산 (기존 로직 재사용)
        from.HandleClick(true);

        List<board11> movable = new List<board11>();
        foreach (board11 cell in allCells)
        {
            if (cell.canmove != 0)
            {
                movable.Add(cell);
            }
        }

        if (movable.Count == 0)
        {
            Debug.Log("AI: 이동 가능한 칸 없음, 다시 시도");
            from.ResetAllCanMove();
            typeof(board11)
                .GetField("handle", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, 0);

            // 이동 불가능하면 다른 기물 시도
            Invoke(nameof(AIMove), 0.1f);
            return;
        }

        // 4️⃣ 이동 실행
        board11 to = movable[Random.Range(0, movable.Count)];
        ExecuteMove(from, to);

        EndAITurn();
    }

    void ExecuteMove(board11 from, board11 to)
    {
        Debug.Log($"AI 이동: {from.row}{from.col} → {to.row}{to.col}");

        // 기물 이동
        to.piece1 = from.piece1;
        to.color = from.color;
        from.piece1 = 0;
        from.color = 0;

        from.ResetAllCanMove();

        // ✅ 핵심 수정: handle 초기화
        typeof(board11)
            .GetField("handle", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, 0);
    }

    void EndAITurn()
    {
        Debug.Log("AI 턴 종료. 플레이어 턴으로 전환");
        currentTurn = 1;  // ✅ 플레이어 턴으로 전환
        isThinking = false;
    }
}