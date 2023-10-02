using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleScreenPlay : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public void Play()
    {
        //Go straight to orchestra if finished tutorial, else start tutorial.
        sceneLoader.LoadScene(PlayerPrefs.GetInt("TutorialComplete") > 0 
            ? SceneLoader.Scene.Orchestra : SceneLoader.Scene.Tutorial);
    }

}
