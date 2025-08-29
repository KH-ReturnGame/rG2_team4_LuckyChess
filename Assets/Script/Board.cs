using UnityEngine;

public class Board : MonoBehaviour
{

    public GameObject tile;
    public GameObject[,] board = new GameObject[8, 8];

    void Awake()
    {
        
    }
}
