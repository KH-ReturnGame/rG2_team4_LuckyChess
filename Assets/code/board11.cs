using JetBrains.Annotations;
using Mono.Cecil;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class board11 : MonoBehaviour
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

    public void ResetAllCanMove()
    {
        board11[] allBoards = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);
        foreach (board11 cell in allBoards)
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
        board11[] allBoards = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);
        foreach (board11 cell in allBoards)
        {
            handle = 0;
            realhandle = 0;
        }
        handle = 0;
        foreach (board11 cell in allBoards)
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

        board11[] allCells = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);
        foreach (board11 cell in allCells)
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

    public void HandleClick(bool isAI = false)
    {
        if (!isAI && ChessAIManager.isThinking)
        {
            return;
        }

        if (handle == 0 && ((color == 1 && ChessAIManager.currentTurn == 1) || (color == 2 && ChessAIManager.currentTurn == 2)))
        {
            if (handle == 0)
            {
                handle += 1;
                realhandle = 1;
                nowmovecol = col;
                nowmoverow = row;
                clickboard();

                if (piece1 == 1)
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    int dir = (color == 1) ? 1 : -1;
                    int targetRow = row + dir;
                    int targetCol = col;
                    string targetName = targetRow.ToString() + targetCol.ToString();
                    GameObject target = GameObject.Find(targetName);

                    if (target != null)
                    {
                        board11 targetBoard = target.GetComponent<board11>();
                        if (targetBoard.piece1 == 0)
                        {
                            Renderer rend = target.GetComponent<Renderer>();
                            rend.material = new Material(rend.material);
                            rend.material.color = Color.red;
                            targetBoard.canmove = 1;
                        }
                    }

                    int targetRow2 = row + dir;
                    int targetCol2 = col + 1;
                    string targetName2 = targetRow2.ToString() + targetCol2.ToString();
                    GameObject target2 = GameObject.Find(targetName2);

                    if (target2 != null)
                    {
                        board11 targetBoard2 = target2.GetComponent<board11>();
                        if (targetBoard2.piece1 != 0 && targetBoard2.color != this.color)
                        {
                            Renderer rend = target2.GetComponent<Renderer>();
                            rend.material = new Material(rend.material);
                            rend.material.color = Color.red;
                            targetBoard2.canmove = 1;
                        }
                    }

                    int targetRow3 = row + dir;
                    int targetCol3 = col - 1;
                    string targetName3 = targetRow3.ToString() + targetCol3.ToString();
                    GameObject target3 = GameObject.Find(targetName3);

                    if (target3 != null)
                    {
                        board11 targetBoard3 = target3.GetComponent<board11>();
                        if (targetBoard3.piece1 != 0 && targetBoard3.color != this.color)
                        {
                            Renderer rend = target3.GetComponent<Renderer>();
                            rend.material = new Material(rend.material);
                            rend.material.color = Color.red;
                            targetBoard3.canmove = 1;
                        }
                    }
                }
                else if (piece1 == 3)
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    int[,] knightMoves = new int[,]
                    {
                        { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                        { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
                    };

                    for (int i = 0; i < knightMoves.GetLength(0); i++)
                    {
                        int targetRow = row + knightMoves[i, 0];
                        int targetCol = col + knightMoves[i, 1];
                        string targetName = targetRow.ToString() + targetCol.ToString();
                        GameObject target = GameObject.Find(targetName);

                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();
                            if (targetBoard.piece1 == 0 || targetBoard.color != this.color)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 3;
                            }
                        }
                    }
                }
                else if (piece1 == 5)
                {
                    nowmovecol = col;
                    nowmoverow = row;

                    int[,] directions = {
                        { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 },
                        { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
                    };

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

                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 5;
                            }
                            else if (targetBoard.color != this.color)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 5;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else if (piece1 == 6)
                {
                    nowmovecol = col;
                    nowmoverow = row;

                    int[,] KingMoves = new int[,]
                    {
                        { 1, 1 }, { 1, 0 }, { 1, -1 },
                        { 0, -1 }, { 0, 1 },
                        { -1, 1 }, { -1, -1 }, { -1, 0 }
                    };

                    for (int i = 0; i < KingMoves.GetLength(0); i++)
                    {
                        int targetRow = row + KingMoves[i, 0];
                        int targetCol = col + KingMoves[i, 1];
                        string targetName = targetRow.ToString() + targetCol.ToString();
                        GameObject target = GameObject.Find(targetName);

                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();
                            if (targetBoard.piece1 == 0 || targetBoard.color != this.color)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 6;
                            }
                        }
                    }
                }
                else if (piece1 == 2)
                {
                    int[,] directions = {
                        { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
                    };

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

                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 2;
                            }
                            else if (targetBoard.color != this.color)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 2;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else if (piece1 == 4)
                {
                    int[,] directions = {
                        { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }
                    };

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

                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 4;
                            }
                            else if (targetBoard.color != this.color)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 4;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        else if (realhandle == 1)
        {
            whose();
            ResetAllCanMove();
        }
        else if (canmove != 0)
        {
            string sourceName = nowmoverow.ToString() + nowmovecol.ToString();
            GameObject sourceObject = GameObject.Find(sourceName);

            if (sourceObject == null)
            {
                Debug.LogError("출발 칸을 찾을 수 없습니다. 이동을 취소합니다.");
                whose();
                ResetAllCanMove();
                return;
            }

            board11 sourceBoard = sourceObject.GetComponent<board11>();

            // Check if the destination is an enemy piece
            if (this.piece1 != 0 && this.color != sourceBoard.color)
            {
                // Battle logic
                int attackerPiece = sourceBoard.piece1; // Player's piece
                int defenderPiece = this.piece1;      // AI's piece

                int attackerWinChance = GetBattleWinChance(attackerPiece, defenderPiece);

                if (Random.Range(0, 100) < attackerWinChance)
                {
                    // Player wins
                    Debug.Log($"플레이어 공격 성공! {attackerPiece}번 기물이 {defenderPiece}번 기물을 잡았습니다! ({attackerWinChance}% 확률)");
                    this.piece1 = sourceBoard.piece1;
                    this.color = sourceBoard.color;
                    sourceBoard.piece1 = 0;
                    sourceBoard.color = 0;
                }
                else
                {
                    // Player loses the piece
                    Debug.Log($"플레이어 공격 실패! {defenderPiece}번 기물이 {attackerPiece}번 플레이어 기물을 막아냈습니다! ({100 - attackerWinChance}% 확률)");
                    sourceBoard.piece1 = 0;
                    sourceBoard.color = 0;
                }
            }
            else
            {
                // Normal move to an empty square
                this.piece1 = sourceBoard.piece1;
                this.color = sourceBoard.color;
                sourceBoard.piece1 = 0;
                sourceBoard.color = 0;
            }

            whose();
            ResetAllCanMove();

            if (!isAI)
            {
                // 매 턴마다 랜덤으로 결정
                ChessAIManager.currentTurn = Random.Range(0, 2) == 0 ? 1 : 2;

                if (ChessAIManager.currentTurn == 1)
                    Debug.Log("다음 턴: 플레이어");
                else
                    Debug.Log("다음 턴: AI");
            }
        }
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
}