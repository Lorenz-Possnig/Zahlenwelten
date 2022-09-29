using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private OVRScreenFade _screenFade;

    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        _screenFade.FadeOut();
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }
}
