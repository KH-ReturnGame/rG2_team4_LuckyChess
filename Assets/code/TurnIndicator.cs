using UnityEngine;

public class TurnIndicator : MonoBehaviour
{
    void Update()
    {
        Renderer rend = GetComponent<Renderer>();

        if (rend != null)
        {
            if (ChessAIManager.currentTurn == 1)
                rend.material.color = Color.white;
            else
                rend.material.color = Color.black;
        }
    }
}