using UnityEngine;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    private bool isLoading = false;

    void Update()
    {
        if (isLoading) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isLoading = true;
            if (SceneManage.instance != null)
                SceneManage.instance.QuitGame();
            else
                Application.Quit();
            enabled = false;
            return;
        }

        bool anyKey = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        bool leftClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (anyKey || leftClick)
        {
            isLoading = true;
            if (SceneManage.instance != null)
                SceneManage.instance.LoadGameplay1Scene();
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            enabled = false;
        }
    }
}