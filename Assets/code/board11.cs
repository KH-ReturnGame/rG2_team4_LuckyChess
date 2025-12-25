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
    public int row = 0; //가로
    public int col = 0; //세로
    public int piece1 = 0; //순서대로 폰(1),비숍(2),나이트(3),룩(4),퀸(5),킹(6)
    static int handle = 0; //전체 칸에서 선택 유무
    public int judge = 0; //칸의 색 설정
    public int canmove = 0; // 움직일 수 있는 칸 폰(1), 비숍(2), 나이트 (3), 룩(4), 퀸(5), 킹(6)
    public int color = 0; // 백(1), 흑(2)
    public int realhandle = 0; //현재칸 클릭 유무
    static int nowmoverow = 0;
    static int nowmovecol = 0; //현재 움직이는 폰의 열
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
        foreach(board11 cell in allBoards)
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

    private void Start() //기본 세팅
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

    public void HandleClick(bool isAI = false) {

        if (!isAI && ChessAIManager.isThinking)
        {
            return;
        }
        //폰 움직임 구현
        
        if (handle == 0 && ((color == 1 && ChessAIManager.currentTurn == 1) || (color == 2 && ChessAIManager.currentTurn == 2))
)
        {
            

            if (handle == 0)
            {
                handle += 1;
                realhandle = 1;
                nowmovecol = col;
                nowmoverow = row;
                clickboard();

                if (piece1 == 1) // Pawn이라면
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
                else if (piece1 == 3) //나이트라면
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
                else if (piece1 == 5) //퀸이라면 (수정됨 - 모든 방향 무제한)
                {
                    nowmovecol = col;
                    nowmoverow = row;

                    // 8 방향
                    int[,] directions = {
                        { 1, 0 },   // up
                        { -1, 0 },  // down
                        { 0, 1 },   // right
                        { 0, -1 },  // left
                        { 1, 1 },   // right up
                        { 1, -1 },  // left up
                        { -1, 1 },  // right down
                        { -1, -1 }  // left down
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
                else if (piece1 == 6) // 킹이라면 (수정됨 - 1칸만 이동)
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
                else if (piece1 == 2) // 비숍
                {
                    int[,] directions = {
                        { 1, 1 },   // right up
                        { 1, -1 },  // left up
                        { -1, 1 },  // right down
                        { -1, -1 }  // left down
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
                else if (piece1 == 4) // 룩
                {
                    int[,] directions = {
                        { 1, 0 },   // up
                        { -1, 0 },  // down
                        { 0, 1 },   // right
                        { 0, -1 },  // left
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
        else if (realhandle == 1) // 같은 기물 다시 클릭 = 선택 취소
        {
            whose();
            ResetAllCanMove();
        }
        else if (canmove != 0) // 이동 가능한 칸을 클릭했을 때
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

            // [FIX] 기물 정보 올바르게 전달
            this.piece1 = sourceBoard.piece1;
            this.color = sourceBoard.color;

            // [FIX] 기존 칸 비우기
            sourceBoard.piece1 = 0;
            sourceBoard.color = 0;

            // 상태 초기화
            whose();
            ResetAllCanMove(); // 보드 다시 그리기 포함

            // [FIX] AI 턴으로 전환
            if (!isAI)
            {
                ChessAIManager.currentTurn = 2;
                Debug.Log("플레이어 이동 완료. AI 턴입니다.");
            }
        }
    }
}