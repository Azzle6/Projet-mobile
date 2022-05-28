using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager_UD : MonoBehaviour
{
    public void SceneLoad(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DestroyMainSceneObjects()
    {
        Destroy(WaveManager.instance.gameObject);
    }
}
