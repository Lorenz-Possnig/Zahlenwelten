using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private OVRScreenFade _screenFade;

    //[SerializeField]
    //private AudioClip Welcome;

    private AudioSource _audioSource;

    public AudioClip Bauen;
    public AudioClip Sprechen;
    public AudioClip Menu;

    private void Start()
    {
        _audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        //_audioSource.PlayOneShot(Welcome);
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

    public void LoadMenu()
    {
        try
        {
            DataSaver.Instance.Save();
            DataSaver.Instance.Reset();
        } catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        LoadScene("Menu", Menu);
    }
    public void LoadZahlenbauen() => LoadScene("Zahlenwelten", Bauen);
    public void LoadZahlensagen() => LoadScene("ZahlenSagen", Sprechen);

    public void LoadZahlenlegenTest() => LoadScene("Testmodus");
    public void LoadZahlensagenTest() => LoadScene("ZahlenSagenTest");

    public void LoadZahlenlegenTraining() => LoadScene("ZahlenlegenTraining");

    public void LoadZahlensagenTraining() => LoadScene("ZahlenSagenTraining");
    public void LoadScene(string sceneToLoad, AudioClip clip = null)
    {
        _screenFade.FadeOut();
        StartCoroutine(LoadSceneAsync(sceneToLoad, clip));
    }
}
