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
    public int row = 0;  //가로
    public int col = 0;  //세로
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
    void ResetAllCanMove()
    {
        board11[] allBoards = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);

        foreach (board11 cell in allBoards)
        {
            cell.canmove = 0;

            Renderer rend = cell.GetComponent<Renderer>(); // cell 기준
            if (cell.judge % 2 == 0)                       // cell의 judge 사용
                rend.material.color = Color.black;
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
    }

    void clickboard()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Color.blue;
    }
    void wq(string naame)
    {

        // 🔥 2) 복사 원본 찾기
        GameObject original = GameObject.Find(naame);
        if (original == null)
        {
            Debug.LogError("wpa_0 오브젝트를 찾을 수 없습니다!");
            return;
        }

        // 🔥 3) 모든 셀(board11) 검색
        board11[] allCells = GameObject.FindObjectsByType<board11>(FindObjectsSortMode.None);

        foreach (board11 cell in allCells)
        {
            // 🔥 4) 조건 맞으면 새 복사본 생성
            int pstring = naame[0] - '0';
            int cstring = naame[1] - '0';
            if (cell.piece1 == pstring && cell.color == cstring)
            {
                GameObject copy = Instantiate(original);

                // 위치 이동
                Vector3 pos = cell.transform.position;
                pos.y += 0;
                copy.transform.position = pos;

                // 렌더링 순서
                SpriteRenderer sr = copy.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sortingOrder = 50;

                // 이름 + 태그 설정
                copy.name = naame+"copy";
                copy.tag = "WPACOPY";   // ⭐ 중요: 삭제를 위해 태그 붙임
            }
        }
    }






    private void Start()                                        //기본 세팅
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
            rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
        }
        else
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
        }
        if (row == 7)
        {
            piece1 = 1;
            color = 2;
        }
        if (row == 1 && col == 2)
        {
            piece1 = 3; //나이트
            color = 1;
        }
        if (row == 1 && col == 7)
        {
            piece1 = 3; //나이트
            color = 1;
        }
        if (row == 8 && col == 2)
        {
            piece1 = 3; //나이트
            color = 2;
        }
        if (row == 8 && col == 7)
        {
            piece1 = 3; //나이트
            color = 2;
        }
        if (row == 1 && col == 5)
        {
            piece1 = 5; //퀸
            color = 1;
        }
        if (row == 8 && col == 5)
        {
            piece1 = 5; //퀸
            color = 2;
        }
        if (row == 1 && col == 4)
        {
            piece1 = 6; //킹
            color = 1;
        }
        if (row == 8 && col == 4)
        {
            piece1 = 6;  //킹
            color = 2;
        }
        if (row == 1 && col == 1 || row == 1 && col == 8)
        {
            piece1 = 4;  //룩
            color = 1;
        }
        if (row == 8 && col == 1 || row == 8 && col == 8)
        {
            piece1 = 4; // 룩
            color = 2;
        }
        if (row == 1 && col == 3 || row == 1 && col == 6)
        {
            piece1 = 2; //비숍
            color = 1;
        }
        if (row == 8 && col == 3 || row == 8 && col == 6)
        {
            piece1 = 2; //비숍
            color = 2;
        }
        ResetAllCanMove();

    }

    void Update()
    {
        // P 키를 눌렀을 때 한 번만 감지
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P 키 눌림!");
            // 여기서 원하는 함수 호출 가능
            whose();
            ResetAllCanMove();
        }
        //여기세 기물 등장 만들예정
    }
                                  



    private void OnMouseDown()                                  //폰 움직임 구현
    {
        if (color == 1 && handle == 0)
        {
            if (handle == 0)
            {
                handle += 1; // 클릭하면 handle 증가
                realhandle = 1;
                nowmovecol = col;
                nowmoverow = row;
                clickboard();

                if (piece1 == 1) // Pawn이라면
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    int targetRow = row + 1;
                    int targetCol = col;
                    string targetName = targetRow.ToString() + targetCol.ToString();

                    GameObject target = GameObject.Find(targetName);
                    if (target != null)
                    {
                        board11 targetBoard = target.GetComponent<board11>();

                        if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                        {
                            Renderer rend = target.GetComponent<Renderer>();
                            rend.material = new Material(rend.material);
                            rend.material.color = Color.red;

                            targetBoard.canmove = 1;
                        }
                        else
                        {
                        }
                    }

                    int targetRow2 = row + 1;
                    int targetCol2 = col + 1;
                    string targetName2 = targetRow2.ToString() + targetCol2.ToString();

                    GameObject target2 = GameObject.Find(targetName2);
                    if (target2 != null)
                    {
                        board11 targetBoard2 = target2.GetComponent<board11>();
                        if (targetBoard2.piece1 != 0 && targetBoard2.color == 2) // target 칸에 상대 기물이 있으면
                        {
                            Renderer rend = target2.GetComponent<Renderer>();
                            rend.material = new Material(rend.material);
                            rend.material.color = Color.red;
                            targetBoard2.canmove = 1;
                        }
                        else
                        {
                        }
                    }
                    int targetRow3 = row + 1;
                    int targetCol3 = col - 1;
                    string targetName3 = targetRow3.ToString() + targetCol3.ToString();

                    GameObject target3 = GameObject.Find(targetName3);
                    if (target3 != null)
                    {
                        board11 targetBoard3 = target3.GetComponent<board11>();
                        if (targetBoard3.piece1 != 0 && targetBoard3.color == 2) // target 칸에 상대 기물이 있으면
                        {
                            Renderer rend = target3.GetComponent<Renderer>();
                            rend.material = new Material(rend.material);
                            rend.material.color = Color.red;
                            targetBoard3.canmove = 1;
                        }
                        else
                        {
                        }
                    }
                }
                else if (piece1 == 3) //나이트라면
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    int[,] knightMoves = new int[,] { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };
                    for (int i = 0; i < knightMoves.GetLength(0); i++)
                    {
                        int targetRow = row + knightMoves[i, 0];
                        int targetCol = col + knightMoves[i, 1];
                        string targetName = targetRow.ToString() + targetCol.ToString();
                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();
                            if (targetBoard.piece1 == 0 || targetBoard.color == 2) // target 칸에 기물이 없거나 상대 기물이 있으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 3;
                            }
                        }
                    }
                }
                else if (piece1 == 5) //퀸이라면
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    int[,] KingMoves = new int[,] { { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { 0, 1 }, { -1, 1 }, { -1, -1 }, { -1, 0 } };
                    for (int i = 0; i < KingMoves.GetLength(0); i++)
                    {
                        int targetRow = row + KingMoves[i, 0];
                        int targetCol = col + KingMoves[i, 1];
                        string targetName = targetRow.ToString() + targetCol.ToString();
                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();
                            if (targetBoard.piece1 == 0 || targetBoard.color == 2) // target 칸에 기물이 없거나 상대 기물이 있으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;
                                targetBoard.canmove = 5;
                            }
                        }

                    }
                }
                else if (piece1 == 6)//킹이라면
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    for (int i = 1; i < 10; i++)                                        //left up
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row + i;
                        int targetCol = col - i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if(targetBoard.color==2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //right up
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row + i;
                        int targetCol = col + i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //left down
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row - i;
                        int targetCol = col - i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //right down
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row - i;
                        int targetCol = col + i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //up
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row + i;
                        int targetCol = col;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 8; i++)                                        //down
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row - i;
                        int targetCol = nowmovecol;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (targetRow < 0 || targetRow > 7 || targetCol < 0 || targetCol > 7)
                        {
                            break;
                        }

                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //left
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row;
                        int targetCol = col - i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //right
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row;
                        int targetCol = col + i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 6;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else if (piece1 == 2)//비숍이라면
                {
                    nowmovecol = col;
                    nowmoverow = row;
                    for (int i = 1; i < 10; i++)                                        //left up
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row + i;
                        int targetCol = col - i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 2;
                            }
                            else if (targetBoard.color == 2)
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
                    for (int i = 1; i < 10; i++)                                        //right up
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row + i;
                        int targetCol = col + i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 2;
                            }
                            else if (targetBoard.color == 2)
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
                    for (int i = 1; i < 10; i++)                                        //left down
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row - i;
                        int targetCol = col - i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 2;
                            }
                            else if (targetBoard.color == 2)
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
                    for (int i = 1; i < 10; i++)                                        //right down
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row - i;
                        int targetCol = col + i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 2;
                            }
                            else if (targetBoard.color == 2)
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
                else if (piece1 == 4) //룩일때
                {
                    for (int i = 1; i < 10; i++)                                        //up
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row + i;
                        int targetCol = col;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 4;
                            }
                            else if (targetBoard.color == 2)
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
                    for (int i = 1; i <8; i++)                                        //down
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row - i;
                        int targetCol = col;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 4;
                            }
                            else if (targetBoard.color == 2)
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 4;
                                break;
                            }
                            else if(targetBoard.color == 1)
                            {
                                break;
                            }

                        }
                    }
                    for (int i = 1; i < 10; i++)                                        //left
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row;
                        int targetCol = col - i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 4;
                            }
                            else if (targetBoard.color == 2)
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
                    for (int i = 1; i < 10; i++)                                        //right
                    {
                        nowmovecol = col;
                        nowmoverow = row;
                        int targetRow = row;
                        int targetCol = col + i;
                        string targetName = targetRow.ToString() + targetCol.ToString();

                        GameObject target = GameObject.Find(targetName);
                        if (target != null)
                        {
                            board11 targetBoard = target.GetComponent<board11>();

                            if (targetBoard.piece1 == 0) // target 칸에 기물이 없으면
                            {
                                Renderer rend = target.GetComponent<Renderer>();
                                rend.material = new Material(rend.material);
                                rend.material.color = Color.red;

                                targetBoard.canmove = 4;
                            }
                            else if (targetBoard.color == 2)
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
        else if(realhandle == 1)
        {
            whose();
            ResetAllCanMove();
        }

        else if (canmove == 1)     //여기부터 움직임 구현, 폰의 움직임
        {
            this.piece1 = 1;
            this.color = 1;
            this.canmove = 0;

            handle = 0;
            if (judge % 2 == 0)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
            }
            string targetName = nowmoverow.ToString() + nowmovecol.ToString();

            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                board11 targetBoard = target.GetComponent<board11>();
                targetBoard.piece1 = 0;
                targetBoard.color = 0;
                targetBoard.realhandle = 0;
            }
            nowmovecol = 0;
            nowmoverow = 0;
            ResetAllCanMove();
        }
        else if (canmove == 3)        //나이트를 움직일때
        {
            this.piece1 = 3;
            this.color = 1;
            this.canmove = 0;

            handle = 0;
            if (judge % 2 == 0)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
            }
            string targetName = nowmoverow.ToString() + nowmovecol.ToString();

            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                board11 targetBoard = target.GetComponent<board11>();
                targetBoard.piece1 = 0;
                targetBoard.color = 0;
                targetBoard.realhandle = 0;
            }
            ResetAllCanMove();
        }
        else if (canmove == 5)  //퀸을 움직일떄
        {
            this.piece1 = 5;
            this.color = 1;
            this.canmove = 0;

            handle = 0;
            if (judge % 2 == 0)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
            }
            string targetName = nowmoverow.ToString() + nowmovecol.ToString();

            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                board11 targetBoard = target.GetComponent<board11>();
                targetBoard.piece1 = 0;
                targetBoard.color = 0;
                targetBoard.realhandle = 0;
            }
            nowmovecol = 0;
            nowmoverow = 0;
            ResetAllCanMove();
        }
        else if (canmove == 6)        //킹을움직일떄
        {
            this.piece1 = 6;
            this.color = 1;
            this.canmove = 0;

            handle = 0;
            if (judge % 2 == 0)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
            }
            string targetName = nowmoverow.ToString() + nowmovecol.ToString();

            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                board11 targetBoard = target.GetComponent<board11>();
                targetBoard.piece1 = 0;
                targetBoard.color = 0;
                targetBoard.realhandle = 0;
            }
            nowmovecol = 0;
            nowmoverow = 0;
            ResetAllCanMove();
        }
        else if (canmove == 4)        //룩을움직일떄
        {
            this.piece1 = 4;
            this.color = 1;
            this.canmove = 0;

            handle = 0;
            if (judge % 2 == 0)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
            }
            string targetName = nowmoverow.ToString() + nowmovecol.ToString();

            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                board11 targetBoard = target.GetComponent<board11>();
                targetBoard.piece1 = 0;
                targetBoard.color = 0;
                targetBoard.realhandle = 0;
            }
            nowmovecol = 0;
            nowmoverow = 0;
            ResetAllCanMove();
        }
        else if (canmove == 2)        //비숍을움직일떄
        {
            this.piece1 = 2;
            this.color = 1;
            this.canmove = 0;

            handle = 0;
            if (judge % 2 == 0)
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.black; // 오브젝트 색상을 검은색으로 변경
            }
            else
            {
                Renderer rend = GetComponent<Renderer>();
                rend.material.color = Color.white; //오브젝트 색상을 검은색으로 변경
            }
            string targetName = nowmoverow.ToString() + nowmovecol.ToString();

            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                board11 targetBoard = target.GetComponent<board11>();
                targetBoard.piece1 = 0;
                targetBoard.color = 0;
                targetBoard.realhandle = 0;
            }
            nowmovecol = 0;
            nowmoverow = 0;
            ResetAllCanMove();
        }
    }
}