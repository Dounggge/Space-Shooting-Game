using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage instance;

    private void Awake()
    {
        SceneSingleton();
    }

    private void SceneSingleton()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadGameplay1Scene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGameplay2Scene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadGameplay3Scene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
