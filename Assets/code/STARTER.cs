using UnityEngine;
using UnityEngine.SceneManagement;

public class STARTER : MonoBehaviour
{
    // ButtonAI에 연결할 함수
    public void LoadAIScene()
    {
        SceneManager.LoadScene("chess_ai");
        Debug.Log("AI 씬으로 이동");
    }

    // ButtonPVP에 연결할 함수
    public void LoadPVPScene()
    {
        SceneManager.LoadScene("chess_pvp");
        Debug.Log("PVP 씬으로 이동");
    }
}