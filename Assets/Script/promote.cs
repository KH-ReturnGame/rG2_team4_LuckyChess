using System.Collections;
using UnityEngine;

public class promote : MonoBehaviour
{

    int rand;
    int piece_num;
    void Start()
    {
        rand = Random.Range(1, 1001);
    }


    void Update()
    {
        if (piece_num == 1) //이거는 임시용으로 넣은거고 이 조건에는 폰이 8랭크 도달한 상태를 넣으면 됨
        {
            if (rand <= 200) //20퍼 퀸으로 프로모션
            {
                piece_num = 5;
            }
            else if (rand <= 400) //20퍼 룩으로 프로모션
            {
                piece_num = 4;
            }
            else if (rand <= 600) // 20퍼 나이트 프로모션
            {
                piece_num = 3;
            }
            else if (rand <= 800)// 20퍼 비숍
            {
                piece_num = 2;
            }
            else if (rand <= 900) //10퍼 킹
            {
                piece_num = 7;
            }
            else //10퍼 실패
            {
                Debug.Log("실패"); //실패했다는 스프라이트 띄우기
            }
        }
        
    }
}
