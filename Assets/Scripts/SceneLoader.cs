using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private OVRScreenFade _screenFade;

    private AudioSource _audioSource;

    public AudioClip Bauen;
    public AudioClip Sprechen;
    public AudioClip Menu;

    private void Start()
    {
        _audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
    }

    private IEnumerator LoadSceneAsync(string sceneToLoad, AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!async.isDone && _audioSource.isPlaying)
        {
            yield return null;
        }
    }

    public void LoadMenu() => LoadScene("Menu", Menu);
    public void LoadZahlenbauen() => LoadScene("Zahlenwelten", Bauen);
    public void LoadZahlensagen() => LoadScene("ZahlenSagen", Sprechen);

    public void LoadScene(string sceneToLoad, AudioClip clip = null)
    {
        _screenFade.FadeOut();
        StartCoroutine(LoadSceneAsync(sceneToLoad, clip));
    }
}
