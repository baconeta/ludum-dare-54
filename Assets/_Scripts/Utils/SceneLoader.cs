using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        Title,
        NightSelection,
        Orchestra,
    }

    public void LoadScene(Scene sceneToLoad)
    {
        LoadScene((int)sceneToLoad);
    }

    public void LoadScene(int sceneBuildIndex)
    {
        //Load the given scene
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void LoadSceneAdditive(Scene sceneToAdd)
    {
        LoadSceneAdditive(sceneToAdd);
    }

    public void LoadSceneAdditive(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Additive);
    }


}
