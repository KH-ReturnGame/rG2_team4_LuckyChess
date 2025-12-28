using UnityEngine;

public class boardPVP : MonoBehaviour
{
    public int row = 0;
    public int col = 0;
    public int piece1 = 0;
    static int handle = 0;
    public int judge = 0;
    public int canmove = 0;
    public int color = 0;
    public int realhandle = 0;
    static int nowmoverow = 0;
    static int nowmovecol = 0;
    public int ima = 0;
    public GameObject pieceImage;

    // 턴 시스템 (1 = 플레이어1/하얀색, 2 = 플레이어2/검은색)
    public static int currentTurn = 1;

    public void ResetAllCanMove()
    {
        boardPVP[] allBoards = GameObject.FindObjectsByType<boardPVP>(FindObjectsSortMode.None);
        foreach (boardPVP cell in allBoards)
        {
            cell.canmove = 0;
            Renderer rend = cell.GetComponent<Renderer>();
            if (cell.judge % 2 == 0)
                rend.material.color = Color.gray;
            else
                rend.material.color = Color.white;
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("WPACOPY");
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }

        wq("111");
        wq("121");
        wq("211");
        wq("221");
        wq("311");
        wq("321");
        wq("411");
        wq("421");
        wq("511");
        wq("521");
        wq("611");
        wq("621");
    }

    void whose()
    {
        boardPVP[] allBoards = GameObject.FindObjectsByType<boardPVP>(FindObjectsSortMode.None);
        foreach (boardPVP cell in allBoards)
        {
            handle = 0;
            realhandle = 0;
        }
        handle = 0;
        foreach (boardPVP cell in allBoards)
        {
            cell.realhandle = 0;
        }
    }

    void clickboard()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.blue;
    }

    void wq(string naame)
    {
        GameObject original = GameObject.Find(naame);
        if (original == null)
        {
            Debug.LogError(naame + " 오브젝트를 찾을 수 없습니다!");
            return;
        }

        boardPVP[] allCells = GameObject.FindObjectsByType<boardPVP>(FindObjectsSortMode.None);
        foreach (boardPVP cell in allCells)
        {
            int pstring = naame[0] - '0';
            int cstring = naame[1] - '0';

            if (cell.piece1 == pstring && cell.color == cstring)
            {
                GameObject copy = Instantiate(original);
                Vector3 pos = cell.transform.position;
                pos.y += 0;
                copy.transform.position = pos;

                SpriteRenderer sr = copy.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sortingOrder = 50;

                copy.name = naame + "copy";
                copy.tag = "WPACOPY";
            }
        }
    }

    private void Start()
    {
        string objName = gameObject.name;
        row = int.Parse(objName[0].ToString());
        col = int.Parse(objName[1].ToString());
        judge = row + col;

        if (row == 2)
        {
            piece1 = 1;
            color = 1;
        }

        if (judge % 2 == 0)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.gray;
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white;
        }

        if (row == 7)
        {
            piece1 = 1;
            color = 2;
        }

        if (row == 1 && col == 2)
        {
            piece1 = 3;
            color = 1;
        }
        if (row == 1 && col == 7)
        {
            piece1 = 3;
            color = 1;
        }
        if (row == 8 && col == 2)
        {
            piece1 = 3;
            color = 2;
        }
        if (row == 8 && col == 7)
        {
            piece1 = 3;
            color = 2;
        }

        if (row == 1 && col == 5)
        {
            piece1 = 5;
            color = 1;
        }
        if (row == 8 && col == 5)
        {
            piece1 = 5;
            color = 2;
        }

        if (row == 1 && col == 4)
        {
            piece1 = 6;
            color = 1;
        }
        if (row == 8 && col == 4)
        {
            piece1 = 6;
            color = 2;
        }

        if (row == 1 && col == 1 || row == 1 && col == 8)
        {
            piece1 = 4;
            color = 1;
        }
        if (row == 8 && col == 1 || row == 8 && col == 8)
        {
            piece1 = 4;
            color = 2;
        }

        if (row == 1 && col == 3 || row == 1 && col == 6)
        {
            piece1 = 2;
            color = 1;
        }
        if (row == 8 && col == 3 || row == 8 && col == 6)
        {
            piece1 = 2;
            color = 2;
        }

        if (row == 1 && col == 1)
        {
            Invoke(nameof(InitializePieces), 0.1f);
        }
    }

    void InitializePieces()
    {
        wq("111");
        wq("121");
        wq("211");
        wq("221");
        wq("311");
        wq("321");
        wq("411");
        wq("421");
        wq("511");
        wq("521");
        wq("611");
        wq("621");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P 키 눌림!");
            whose();
            ResetAllCanMove();
        }
    }

    private void OnMouseDown()
    {
        HandleClick();
    }

    public void HandleClick()
    {
        // 현재 턴의 플레이어 기물만 선택 가능
        if (handle == 0 && color == currentTurn)
        {
            handle += 1;
            realhandle = 1;
            nowmovecol = col;
            nowmoverow = row;
            clickboard();

            // 현재 턴 표시
            string turnName = (currentTurn == 1) ? "플레이어 1 (하얀색)" : "플레이어 2 (검은색)";
            Debug.Log($"{turnName}의 턴입니다.");

            if (piece1 == 1) // 폰
            {
                int dir = (color == 1) ? 1 : -1;

                // 앞으로 한 칸 (빈 칸만)
                CheckAndMarkMove(row + dir, col, true);

                // 대각선 공격 (적이 있을 때만)
                CheckAndMarkPawnAttack(row + dir, col + 1);
                CheckAndMarkPawnAttack(row + dir, col - 1);
            }
            else if (piece1 == 3) // 나이트
            {
                int[,] knightMoves = new int[,]
                {
                    { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                    { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
                };

                for (int i = 0; i < knightMoves.GetLength(0); i++)
                {
                    CheckAndMarkMove(row + knightMoves[i, 0], col + knightMoves[i, 1], false);
                }
            }
            else if (piece1 == 5) // 퀸
            {
                int[,] directions = {
                    { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 },
                    { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
                };
                CheckDirectionalMoves(directions);
            }
            else if (piece1 == 6) // 킹
            {
                int[,] kingMoves = new int[,]
                {
                    { 1, 1 }, { 1, 0 }, { 1, -1 },
                    { 0, -1 }, { 0, 1 },
                    { -1, 1 }, { -1, -1 }, { -1, 0 }
                };

                for (int i = 0; i < kingMoves.GetLength(0); i++)
                {
                    CheckAndMarkMove(row + kingMoves[i, 0], col + kingMoves[i, 1], false);
                }
            }
            else if (piece1 == 2) // 비숍
            {
                int[,] directions = {
                    { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
                };
                CheckDirectionalMoves(directions);
            }
            else if (piece1 == 4) // 룩
            {
                int[,] directions = {
                    { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }
                };
                CheckDirectionalMoves(directions);
            }
        }
        else if (realhandle == 1)
        {
            // 기물 선택 취소
            whose();
            ResetAllCanMove();
        }
        else if (canmove != 0)
        {
            // 이동 실행
            string sourceName = nowmoverow.ToString() + nowmovecol.ToString();
            GameObject sourceObject = GameObject.Find(sourceName);

            if (sourceObject == null)
            {
                Debug.LogError("출발 칸을 찾을 수 없습니다. 이동을 취소합니다.");
                whose();
                ResetAllCanMove();
                return;
            }

            boardPVP sourceBoard = sourceObject.GetComponent<boardPVP>();

            // 적 기물이 있는 칸으로 이동하는 경우 (전투)
            if (this.piece1 != 0 && this.color != sourceBoard.color)
            {
                int attackerPiece = sourceBoard.piece1;
                int defenderPiece = this.piece1;

                int attackerWinChance = GetBattleWinChance(attackerPiece, defenderPiece);

                if (Random.Range(0, 100) < attackerWinChance)
                {
                    Debug.Log($"공격 성공! {attackerPiece}번 기물이 {defenderPiece}번 기물을 잡았습니다! ({attackerWinChance}% 확률)");
                    this.piece1 = sourceBoard.piece1;
                    this.color = sourceBoard.color;
                    sourceBoard.piece1 = 0;
                    sourceBoard.color = 0;
                }
                else
                {
                    Debug.Log($"공격 실패! {defenderPiece}번 기물이 {attackerPiece}번 기물을 막아냈습니다! ({100 - attackerWinChance}% 확률)");
                    sourceBoard.piece1 = 0;
                    sourceBoard.color = 0;
                }
            }
            else
            {
                // 빈 칸으로 이동
                this.piece1 = sourceBoard.piece1;
                this.color = sourceBoard.color;
                sourceBoard.piece1 = 0;
                sourceBoard.color = 0;
            }

            whose();
            ResetAllCanMove();

            // 킹 체크 (게임 오버 처리)
            CheckKingDead();

            // 폰 프로모션
            if (this.piece1 == 1)
            {
                if ((this.color == 1 && this.row == 8) || (this.color == 2 && this.row == 1))
                {
                    int[] promotionOptions = { 2, 3, 4, 5, 6 };
                    int randomPromotion = promotionOptions[Random.Range(0, promotionOptions.Length)];

                    string[] pieceNames = { "", "폰", "비숍", "나이트", "룩", "퀸", "킹" };
                    Debug.Log($"🎉 프로모션! 폰이 {pieceNames[randomPromotion]}(으)로 승급했습니다!");

                    this.piece1 = randomPromotion;
                    ResetAllCanMove();
                }
            }

            // 랜덤 턴 교체
            currentTurn = Random.Range(0, 2) == 0 ? 1 : 2;
            string nextPlayer = (currentTurn == 1) ? "플레이어 1 (하얀색)" : "플레이어 2 (검은색)";
            Debug.Log($"다음 턴: {nextPlayer}");
        }
    }

    // 헬퍼 함수: 폰의 대각선 공격 (적이 있을 때만)
    void CheckAndMarkPawnAttack(int targetRow, int targetCol)
    {
        string targetName = targetRow.ToString() + targetCol.ToString();
        GameObject target = GameObject.Find(targetName);

        if (target != null)
        {
            boardPVP targetBoard = target.GetComponent<boardPVP>();
            if (targetBoard != null)
            {
                // 적 기물이 있을 때만 이동 가능
                if (targetBoard.piece1 != 0 && targetBoard.color != this.color)
                {
                    MarkAsMovable(target, targetBoard);
                }
            }
        }
    }

    // 헬퍼 함수: 단일 칸 체크 및 마킹
    void CheckAndMarkMove(int targetRow, int targetCol, bool emptyOnly)
    {
        string targetName = targetRow.ToString() + targetCol.ToString();
        GameObject target = GameObject.Find(targetName);

        if (target != null)
        {
            boardPVP targetBoard = target.GetComponent<boardPVP>();
            if (targetBoard != null)
            {
                if (emptyOnly)
                {
                    // 폰의 전진 (빈 칸만)
                    if (targetBoard.piece1 == 0)
                    {
                        MarkAsMovable(target, targetBoard);
                    }
                }
                else
                {
                    // 일반 이동 또는 공격
                    if (targetBoard.piece1 == 0 || targetBoard.color != this.color)
                    {
                        MarkAsMovable(target, targetBoard);
                    }
                }
            }
        }
    }

    // 헬퍼 함수: 방향성 이동 체크 (비숍, 룩, 퀸)
    void CheckDirectionalMoves(int[,] directions)
    {
        for (int d = 0; d < directions.GetLength(0); d++)
        {
            int dr = directions[d, 0];
            int dc = directions[d, 1];

            for (int i = 1; i < 10; i++)
            {
                int targetRow = row + dr * i;
                int targetCol = col + dc * i;

                if (targetRow < 0 || targetRow > 8 || targetCol < 0 || targetCol > 8)
                    break;

                string targetName = targetRow.ToString() + targetCol.ToString();
                GameObject target = GameObject.Find(targetName);

                if (target == null) break;

                boardPVP targetBoard = target.GetComponent<boardPVP>();
                if (targetBoard == null) break;

                if (targetBoard.piece1 == 0)
                {
                    MarkAsMovable(target, targetBoard);
                }
                else if (targetBoard.color != this.color)
                {
                    MarkAsMovable(target, targetBoard);
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }

    // 헬퍼 함수: 이동 가능 표시
    void MarkAsMovable(GameObject target, boardPVP targetBoard)
    {
        Renderer rend = target.GetComponent<Renderer>();
        rend.material = new Material(rend.material);
        rend.material.color = Color.red;
        targetBoard.canmove = this.piece1;
    }

    int GetBattleWinChance(int attacker, int defender)
    {
        if (attacker == defender)
            return 50;

        if (attacker == 1)
        {
            if (defender == 3) return 40;
            if (defender == 2) return 40;
            if (defender == 4) return 30;
            if (defender == 5) return 20;
            if (defender == 6) return 10;
        }

        if (attacker == 3)
        {
            if (defender == 1) return 60;
            if (defender == 2) return 50;
            if (defender == 4) return 40;
            if (defender == 5) return 30;
            if (defender == 6) return 20;
        }

        if (attacker == 2)
        {
            if (defender == 1) return 60;
            if (defender == 3) return 50;
            if (defender == 4) return 40;
            if (defender == 5) return 30;
            if (defender == 6) return 20;
        }

        if (attacker == 4)
        {
            if (defender == 1) return 70;
            if (defender == 3) return 60;
            if (defender == 2) return 60;
            if (defender == 5) return 40;
            if (defender == 6) return 30;
        }

        if (attacker == 5)
        {
            if (defender == 1) return 80;
            if (defender == 3) return 70;
            if (defender == 2) return 70;
            if (defender == 4) return 60;
            if (defender == 6) return 40;
        }

        if (attacker == 6)
        {
            if (defender == 1) return 90;
            if (defender == 3) return 80;
            if (defender == 2) return 80;
            if (defender == 4) return 70;
            if (defender == 5) return 60;
        }

        return 50;
    }

    // 킹 사망 체크 및 게임 오버 처리
    void CheckKingDead()
    {
        bool whiteKingExists = false;
        bool blackKingExists = false;

        boardPVP[] allBoards = GameObject.FindObjectsByType<boardPVP>(FindObjectsSortMode.None);
        foreach (boardPVP cell in allBoards)
        {
            if (cell.piece1 == 6) // 킹
            {
                if (cell.color == 1)
                    whiteKingExists = true;
                else if (cell.color == 2)
                    blackKingExists = true;
            }
        }

        // 화이트 킹이 죽었으면 블랙 승리
        if (!whiteKingExists)
        {
            GameObject canvas = GameObject.Find("GameOverCanvas");
            if (canvas != null)
            {
                Transform gameWinBlack = canvas.transform.Find("GameWin_Black");
                if (gameWinBlack != null)
                {
                    gameWinBlack.gameObject.SetActive(true);
                    Debug.Log("블랙 승리!");
                }
            }
        }

        // 블랙 킹이 죽었으면 화이트 승리
        if (!blackKingExists)
        {
            GameObject canvas = GameObject.Find("GameOverCanvas");
            if (canvas != null)
            {
                Transform gameWinWhite = canvas.transform.Find("GameWin_White");
                if (gameWinWhite != null)
                {
                    gameWinWhite.gameObject.SetActive(true);
                    Debug.Log("화이트 승리!");
                }
            }
        }
    }
}