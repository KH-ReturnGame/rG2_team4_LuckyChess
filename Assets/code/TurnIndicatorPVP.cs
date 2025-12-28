using UnityEngine;

public class TurnIndicatorPVP : MonoBehaviour
{
    void Update()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (boardPVP.currentTurn == 1)
                rend.material.color = Color.white; // 플레이어 1
            else
                rend.material.color = Color.black; // 플레이어 2
        }
    }
}