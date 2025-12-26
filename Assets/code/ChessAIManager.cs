using UnityEngine;
using System.Collections.Generic;

public class ChessAIManager : MonoBehaviour
{
    public static int currentTurn; // 1 = 백(플레이어), 2 = 흑(AI)
    public static bool isThinking = false;
    public int searchDepth = 3; // 탐색 깊이 (3~4 추천)

    void Start()
    {
        // 첫 턴도 랜덤
        currentTurn = Random.Range(0, 2) == 0 ? 1 : 2;

        if (currentTurn == 1)
            Debug.Log("게임 시작! 플레이어 턴입니다.");
        else
            Debug.Log("게임 시작! AI 턴입니다.");
    }

    // 기물 가치
    private static readonly int[] pieceValues = { 0, 100, 320, 330, 500, 900, 20000 };
    // 인덱스: 0=빈칸, 1=폰, 2=비숍, 3=나이트, 4=룩, 5=퀸, 6=킹

    void Update()
    {
        // 랜덤 턴이므로 AI 턴이면 계속 체크
        if (currentTurn == 2 && !isThinking)
        {
            isThinking = true;
            Invoke(nameof(AIMove), 0.5f);
        }
    }

    void AIMove()
    {
        board11[] allCells = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);

        // 보드 상태 가져오기
        BoardState currentState = GetBoardState(allCells);

        // Minimax 실행
        Move bestMove = FindBestMove(currentState, searchDepth);

        if (bestMove == null)
        {
            Debug.Log("AI: 이동 가능한 수가 없습니다");
            EndAITurn();
            return;
        }

        // 최선의 수 실행
        board11 from = FindCell(allCells, bestMove.fromRow, bestMove.fromCol);
        board11 to = FindCell(allCells, bestMove.toRow, bestMove.toCol);

        if (from != null && to != null)
        {
            Debug.Log($"AI 최선의 수: {from.row}{from.col} → {to.row}{to.col} (평가: {bestMove.score})");
            ExecuteMove(from, to);
        }

        EndAITurn();
    }

    // Minimax 최선의 수 찾기
    Move FindBestMove(BoardState state, int depth)
    {
        List<Move> allMoves = GenerateAllMoves(state, 2); // AI는 흑(2)

        if (allMoves.Count == 0)
            return null;

        Move bestMove = null;
        int bestScore = int.MinValue;
        int alpha = int.MinValue;
        int beta = int.MaxValue;

        foreach (Move move in allMoves)
        {
            BoardState newState = ApplyMove(state, move);
            int score = Minimax(newState, depth - 1, false, alpha, beta);
            move.score = score;

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }

            alpha = Mathf.Max(alpha, score);
        }

        return bestMove;
    }

    // Minimax 알고리즘 + Alpha-Beta 가지치기
    int Minimax(BoardState state, int depth, bool isMaximizing, int alpha, int beta)
    {
        // 종료 조건
        if (depth == 0)
            return EvaluateBoard(state);

        if (isMaximizing) // AI 턴 (흑, 최대화)
        {
            int maxScore = int.MinValue;
            List<Move> moves = GenerateAllMoves(state, 2);

            if (moves.Count == 0)
                return EvaluateBoard(state);

            foreach (Move move in moves)
            {
                BoardState newState = ApplyMove(state, move);
                int score = Minimax(newState, depth - 1, false, alpha, beta);
                maxScore = Mathf.Max(maxScore, score);
                alpha = Mathf.Max(alpha, score);

                // Beta 가지치기
                if (beta <= alpha)
                    break;
            }
            return maxScore;
        }
        else // 플레이어 턴 (백, 최소화)
        {
            int minScore = int.MaxValue;
            List<Move> moves = GenerateAllMoves(state, 1);

            if (moves.Count == 0)
                return EvaluateBoard(state);

            foreach (Move move in moves)
            {
                BoardState newState = ApplyMove(state, move);
                int score = Minimax(newState, depth - 1, true, alpha, beta);
                minScore = Mathf.Min(minScore, score);
                beta = Mathf.Min(beta, score);

                // Alpha 가지치기
                if (beta <= alpha)
                    break;
            }
            return minScore;
        }
    }

    // 보드 평가 함수
    int EvaluateBoard(BoardState state)
    {
        int score = 0;

        for (int r = 1; r <= 8; r++)
        {
            for (int c = 1; c <= 8; c++)
            {
                int piece = state.GetPiece(r, c);
                int color = state.GetColor(r, c);

                if (piece == 0) continue;

                int pieceValue = pieceValues[piece];

                // 흑(AI)은 +, 백(플레이어)은 -
                if (color == 2)
                    score += pieceValue;
                else
                    score -= pieceValue;

                // 위치 보너스 (중앙 선호)
                int centerBonus = 0;
                int distanceFromCenter = Mathf.Abs(r - 4) + Mathf.Abs(c - 4);
                centerBonus = (8 - distanceFromCenter) * 2;

                if (color == 2)
                    score += centerBonus;
                else
                    score -= centerBonus;
            }
        }

        return score;
    }

    // 모든 가능한 이동 생성
    List<Move> GenerateAllMoves(BoardState state, int playerColor)
    {
        List<Move> moves = new List<Move>();

        for (int r = 1; r <= 8; r++)
        {
            for (int c = 1; c <= 8; c++)
            {
                if (state.GetColor(r, c) == playerColor)
                {
                    int piece = state.GetPiece(r, c);
                    List<Move> pieceMoves = GeneratePieceMoves(state, r, c, piece, playerColor);
                    moves.AddRange(pieceMoves);
                }
            }
        }

        return moves;
    }

    // 특정 기물의 이동 생성
    List<Move> GeneratePieceMoves(BoardState state, int row, int col, int piece, int color)
    {
        List<Move> moves = new List<Move>();

        if (piece == 1) // 폰
        {
            int dir = (color == 1) ? 1 : -1;

            // 전진
            if (state.IsInBounds(row + dir, col) && state.GetPiece(row + dir, col) == 0)
                moves.Add(new Move(row, col, row + dir, col));

            // 대각선 공격
            if (state.IsInBounds(row + dir, col + 1) && state.GetPiece(row + dir, col + 1) != 0 && state.GetColor(row + dir, col + 1) != color)
                moves.Add(new Move(row, col, row + dir, col + 1));

            if (state.IsInBounds(row + dir, col - 1) && state.GetPiece(row + dir, col - 1) != 0 && state.GetColor(row + dir, col - 1) != color)
                moves.Add(new Move(row, col, row + dir, col - 1));
        }
        else if (piece == 3) // 나이트
        {
            int[,] knightMoves = { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };

            for (int i = 0; i < knightMoves.GetLength(0); i++)
            {
                int nr = row + knightMoves[i, 0];
                int nc = col + knightMoves[i, 1];

                if (state.IsInBounds(nr, nc) && (state.GetPiece(nr, nc) == 0 || state.GetColor(nr, nc) != color))
                    moves.Add(new Move(row, col, nr, nc));
            }
        }
        else if (piece == 2 || piece == 4 || piece == 5) // 비숍, 룩, 퀸
        {
            int[,] directions;

            if (piece == 2) // 비숍
                directions = new int[,] { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            else if (piece == 4) // 룩
                directions = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            else // 퀸
                directions = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int dr = directions[d, 0];
                int dc = directions[d, 1];

                for (int i = 1; i < 8; i++)
                {
                    int nr = row + dr * i;
                    int nc = col + dc * i;

                    if (!state.IsInBounds(nr, nc)) break;

                    if (state.GetPiece(nr, nc) == 0)
                    {
                        moves.Add(new Move(row, col, nr, nc));
                    }
                    else if (state.GetColor(nr, nc) != color)
                    {
                        moves.Add(new Move(row, col, nr, nc));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else if (piece == 6) // 킹
        {
            int[,] kingMoves = { { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { 0, 1 }, { -1, 1 }, { -1, -1 }, { -1, 0 } };

            for (int i = 0; i < kingMoves.GetLength(0); i++)
            {
                int nr = row + kingMoves[i, 0];
                int nc = col + kingMoves[i, 1];

                if (state.IsInBounds(nr, nc) && (state.GetPiece(nr, nc) == 0 || state.GetColor(nr, nc) != color))
                    moves.Add(new Move(row, col, nr, nc));
            }
        }

        return moves;
    }

    // 이동 적용 (새로운 보드 상태 생성)
    BoardState ApplyMove(BoardState state, Move move)
    {
        BoardState newState = state.Clone();

        int piece = newState.GetPiece(move.fromRow, move.fromCol);
        int color = newState.GetColor(move.fromRow, move.fromCol);

        newState.SetPiece(move.toRow, move.toCol, piece);
        newState.SetColor(move.toRow, move.toCol, color);
        newState.SetPiece(move.fromRow, move.fromCol, 0);
        newState.SetColor(move.fromRow, move.fromCol, 0);

        return newState;
    }

    // 현재 보드 상태 가져오기
    BoardState GetBoardState(board11[] cells)
    {
        BoardState state = new BoardState();

        foreach (board11 cell in cells)
        {
            state.SetPiece(cell.row, cell.col, cell.piece1);
            state.SetColor(cell.row, cell.col, cell.color);
        }

        return state;
    }

    // 셀 찾기
    board11 FindCell(board11[] cells, int row, int col)
    {
        foreach (board11 cell in cells)
        {
            if (cell.row == row && cell.col == col)
                return cell;
        }
        return null;
    }

    // 실제 이동 실행
    void ExecuteMove(board11 from, board11 to)
    {
        // 기물을 잡는 경우 (목표 칸에 적 기물이 있음)
        if (to.piece1 != 0 && to.color != from.color)
        {
            int attackerPiece = from.piece1; // 공격하는 기물 (AI)
            int defenderPiece = to.piece1; // 방어하는 기물 (플레이어)

            // 공격자가 이길 확률 계산
            int attackerWinChance = GetBattleWinChance(attackerPiece, defenderPiece);

            // 랜덤 판정
            if (Random.Range(0, 100) < attackerWinChance)
            {
                Debug.Log($"AI 공격 성공! {attackerPiece}번 기물이 {defenderPiece}번 기물을 잡았습니다! ({attackerWinChance}% 확률)");
                // 정상적으로 기물 이동
                to.piece1 = from.piece1;
                to.color = from.color;
                from.piece1 = 0;
                from.color = 0;
                

            }
            else
            {
                Debug.Log($"AI 역전 당함! {defenderPiece}번 기물이 {attackerPiece}번 AI 기물을 막아냈습니다! ({100 - attackerWinChance}% 확률)");
                // 공격한 AI 기물이 죽고, 플레이어 기물은 살아남음
                from.piece1 = 0;
                from.color = 0;
            }
        }
        else
        {
            // 빈 칸으로 이동
            to.piece1 = from.piece1;
            to.color = from.color;
            from.piece1 = 0;
            from.color = 0;
        }

        // 폰 프로모션 체크
        if (to.piece1 == 1) // 폰이라면
        {
            if ((to.color == 1 && to.row == 8) || (to.color == 2 && to.row == 1))
            {
                // 랜덤 프로모션! (폰, 비숍, 나이트, 룩, 퀸, 킹 중 하나)
                int[] promotionOptions = { 1, 2, 3, 4, 5, 6 }; // 폰, 비숍, 나이트, 룩, 퀸, 킹
                int randomPromotion = promotionOptions[Random.Range(0, promotionOptions.Length)];

                string[] pieceNames = { "", "폰", "비숍", "나이트", "룩", "퀸", "킹" };
                Debug.Log($"🎉 AI 프로모션! 폰이 {pieceNames[randomPromotion]}(으)로 승급했습니다!");

                to.piece1 = randomPromotion;
            }
        }

        from.ResetAllCanMove();

        typeof(board11)
            .GetField("handle", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, 0);

        BoardState.CheckKingDead();


        
    }

    // 기물 간 전투 승률 계산 (공격자가 이길 확률 반환)
    int GetBattleWinChance(int attacker, int defender)
    {
        // 기물: 폰(1), 비숍(2), 나이트(3), 룩(4), 퀸(5), 킹(6)

        // 같은 기물끼리는 50:50
        if (attacker == defender)
            return 50;

        // 폰 공격
        if (attacker == 1)
        {
            if (defender == 3) return 40; // 폰 vs 나이트
            if (defender == 2) return 40; // 폰 vs 비숍
            if (defender == 4) return 30; // 폰 vs 룩
            if (defender == 5) return 20; // 폰 vs 퀸
            if (defender == 6) return 10; // 폰 vs 킹
        }

        // 나이트 공격
        if (attacker == 3)
        {
            if (defender == 1) return 60; // 나이트 vs 폰
            if (defender == 2) return 50; // 나이트 vs 비숍
            if (defender == 4) return 40; // 나이트 vs 룩
            if (defender == 5) return 30; // 나이트 vs 퀸
            if (defender == 6) return 20; // 나이트 vs 킹
        }

        // 비숍 공격
        if (attacker == 2)
        {
            if (defender == 1) return 60; // 비숍 vs 폰
            if (defender == 3) return 50; // 비숍 vs 나이트
            if (defender == 4) return 40; // 비숍 vs 룩
            if (defender == 5) return 30; // 비숍 vs 퀸
            if (defender == 6) return 20; // 비숍 vs 킹
        }

        // 룩 공격
        if (attacker == 4)
        {
            if (defender == 1) return 70; // 룩 vs 폰
            if (defender == 3) return 60; // 룩 vs 나이트
            if (defender == 2) return 60; // 룩 vs 비숍
            if (defender == 5) return 40; // 룩 vs 퀸
            if (defender == 6) return 30; // 룩 vs 킹
        }

        // 퀸 공격
        if (attacker == 5)
        {
            if (defender == 1) return 80; // 퀸 vs 폰
            if (defender == 3) return 70; // 퀸 vs 나이트
            if (defender == 2) return 70; // 퀸 vs 비숍
            if (defender == 4) return 60; // 퀸 vs 룩
            if (defender == 6) return 40; // 퀸 vs 킹
        }

        // 킹 공격
        if (attacker == 6)
        {
            if (defender == 1) return 90; // 킹 vs 폰
            if (defender == 3) return 80; // 킹 vs 나이트
            if (defender == 2) return 80; // 킹 vs 비숍
            if (defender == 4) return 70; // 킹 vs 룩
            if (defender == 5) return 60; // 킹 vs 퀸
        }

        return 50; // 기본값
    }

    void EndAITurn()
    {
        Debug.Log("AI 턴 종료");
        isThinking = false;

        // 룰렛 돌려서 다음 턴 결정
        if (TurnRoulette.instance != null)
        {
            TurnRoulette.instance.SpinAndDecideTurn();
        }
        else
        {
            // 룰렛 없으면 랜덤
            currentTurn = Random.Range(0, 2) == 0 ? 1 : 2;
        }
    }
}

// 이동 클래스
public class Move
{
    public int fromRow, fromCol;
    public int toRow, toCol;
    public int score;

    public Move(int fr, int fc, int tr, int tc)
    {
        fromRow = fr;
        fromCol = fc;
        toRow = tr;
        toCol = tc;
        score = 0;
    }
}

// 보드 상태 클래스
public class BoardState
{
    private int[,] pieces = new int[9, 9]; // 1~8 사용
    private int[,] colors = new int[9, 9];

    public int GetPiece(int row, int col) => pieces[row, col];
    public int GetColor(int row, int col) => colors[row, col];
    public void SetPiece(int row, int col, int val) => pieces[row, col] = val;
    public void SetColor(int row, int col, int val) => colors[row, col] = val;

    public bool IsInBounds(int row, int col)
    {
        return row >= 1 && row <= 8 && col >= 1 && col <= 8;
    }

    public BoardState Clone()
    {
        BoardState newState = new BoardState();
        for (int r = 1; r <= 8; r++)
        {
            for (int c = 1; c <= 8; c++)
            {
                newState.pieces[r, c] = this.pieces[r, c];
                newState.colors[r, c] = this.colors[r, c];
            }
        }
        return newState;
    }


        public static void CheckKingDead()
    {
        board11[] cells = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);

        bool whiteKingAlive = false;
        bool blackKingAlive = false;

        foreach (board11 cell in cells)
        {
            if (cell.piece1 == 6) // 킹
            {
                if (cell.color == 1) whiteKingAlive = true;
                if (cell.color == 2) blackKingAlive = true;
            }
        }

        if (!whiteKingAlive || !blackKingAlive)
        {
            if (whiteKingAlive == false)
            {
                Debug.Log("❌ 우리 킹 죽음 → 패배");
                GameOverUI.instance.ShowLose();
            }
            else if (blackKingAlive == false)
            {
                Debug.Log("✅ 상대 킹 죽음 → 승리");
                GameOverUI.instance.ShowWin();
            }
        }
    }


    
}
