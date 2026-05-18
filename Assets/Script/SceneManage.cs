using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    private SceneManage instance;

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

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadUpgradeScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
