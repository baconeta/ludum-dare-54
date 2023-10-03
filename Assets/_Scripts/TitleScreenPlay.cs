using Managers;
using UnityEngine;

public class TitleScreenPlay : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public void Play()
    {
        Destroy(FindObjectOfType<AudioManager>().gameObject);
        
        //Go straight to orchestra if finished tutorial, else start tutorial.
        sceneLoader.LoadScene(PlayerPrefs.GetInt("TutorialComplete") > 0 
            ? SceneLoader.Scene.Orchestra : SceneLoader.Scene.Tutorial);
    }
}
