using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    public GameObject winPanel;
    public GameObject losePanel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
