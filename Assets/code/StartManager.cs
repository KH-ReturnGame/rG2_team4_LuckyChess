using UnityEngine;

public class StartManager : MonoBehaviour
{
    public GameObject startPanel;

    void Start()
    {
        // 게임 시작 시 멈춤
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        // 게임 시작
        Time.timeScale = 1f;
        startPanel.SetActive(false);
    }
}
