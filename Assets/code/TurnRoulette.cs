using UnityEngine;
using System.Collections;

public class TurnRoulette : MonoBehaviour
{
    public static TurnRoulette instance;

    private bool isSpinning = false;
    public float spinDuration = 2f; // 2초 동안 회전

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 초기 색상 설정
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
        }

        // 게임 시작 시 첫 턴 결정
        SpinAndDecideTurn();
    }

    // 룰렛을 돌리고 다음 턴 결정
    public void SpinAndDecideTurn()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinRoulette());
        }
    }

    IEnumerator SpinRoulette()
    {
        isSpinning = true;
        Debug.Log("🎲 룰렛 돌리는 중...");

        float elapsed = 0f;
        float rotations = 5f; // 총 5바퀴 회전

        // 시작 각도
        float startRotation = transform.eulerAngles.z;

        // 랜덤으로 최종 턴 결정
        int finalTurn = Random.Range(0, 2) == 0 ? 1 : 2;

        // 목표 각도
        float targetAngle;
        if (finalTurn == 1)
            targetAngle = 0f; // 플레이어
        else
            targetAngle = 180f; // AI

        float totalRotation = (rotations * 360f) + targetAngle;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / spinDuration;

            // Ease-out 효과 (처음엔 빠르게, 나중엔 천천히)
            float easeProgress = 1f - Mathf.Pow(1f - progress, 3f);

            // 회전 애니메이션
            float currentRotation = startRotation + (totalRotation * easeProgress);
            transform.eulerAngles = new Vector3(0, 0, currentRotation);

            // 회전하는 동안 색상 변화 (깜빡임 효과)
            float colorLerp = Mathf.PingPong(elapsed * 5f, 1f);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.Lerp(Color.white, Color.black, colorLerp);
            }

            yield return null;
        }

        // 최종 각도와 색상 설정
        if (finalTurn == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;
            Debug.Log("🎲 룰렛 결과: 플레이어 턴!");
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            if (spriteRenderer != null)
                spriteRenderer.color = Color.black;
            Debug.Log("🎲 룰렛 결과: AI 턴!");
        }

        // 턴 설정
        ChessAIManager.currentTurn = finalTurn;

        isSpinning = false;
    }

    public bool IsSpinning()
    {
        return isSpinning;
    }
}