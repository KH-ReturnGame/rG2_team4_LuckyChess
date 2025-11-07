using UnityEngine;

public class turnover : MonoBehaviour
{

    int turn;
    bool white_turn;


    void Start()
    {
        turn = Random.Range(1, 3);
        white_turn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (turn == 1)
        {
            white_turn = true;
        }
        else
        {
            white_turn = false;
        }




        if (white_turn == true)
        {
            Debug.Log("white");
        }
        else if(white_turn == false)
        {
            Debug.Log("Black");
        }

        Debug.Log($"{turn}");
    }

}
