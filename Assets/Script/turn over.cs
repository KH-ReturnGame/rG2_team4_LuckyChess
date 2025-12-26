using UnityEngine;

public class turnover : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 static instance (싱글톤) 생성
    public static turnover instance;

    // 현재 턴이 백의 차례인지 여부
    public bool isWhiteTurn;

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작 시 첫 턴을 무작위로 결정
        NextTurn();
    }

    // 다음 턴을 무작위로 결정하는 함수
    public void NextTurn()
    {
        // Random.Range(0, 2)는 0 또는 1을 반환합니다.
        isWhiteTurn = (Random.Range(0, 2) == 0);

        if (isWhiteTurn)
        {
            Debug.Log("백의 턴");
        }
        else
        {
            Debug.Log("흑의 턴");
        }
    }
}