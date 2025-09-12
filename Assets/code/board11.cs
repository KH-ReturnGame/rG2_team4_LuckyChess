using JetBrains.Annotations;
using Mono.Cecil;
using UnityEngine;

public class board11 : MonoBehaviour
{
    public int row = 0;  //가로
    public int col = 0;  //세로
    public int piece1 = 0; //순서대로 폰(1),비숍(2),나이트(3),룩(4),퀸(5),킹(6)
    static int handle = 0; //기물 소유 유무

    private void Start()
    {
        string objName = gameObject.name;
        row = int.Parse(objName[0].ToString());
        col = int.Parse(objName[1].ToString());
        if(row == 2)
        {
            piece1 = 1;
        }
    }

    private void OnMouseDown()
    {
        if (handle == 0)
        {
            handle += 1; // 클릭하면 handle 증가

            if (piece1 == 1) // Pawn이면
            {
                int targetRow = row + 1;
                int targetCol = col;
                string targetName = targetRow.ToString() + targetCol.ToString();

                GameObject target = GameObject.Find(targetName);
                if (target != null)
                {
                    Renderer[] rends = target.GetComponentsInChildren<Renderer>();
                    foreach (Renderer rend in rends)
                    {
                        rend.material = new Material(rend.material);
                        rend.material.color = Color.red;
                    }
                }
            }
        }
    }
}
