using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponentInChildren<AudioSource>();
    }

    public string MainMenuSceneName = "MainMenu";
    public void OnClick()
    {
        audioSrc.Play();
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
