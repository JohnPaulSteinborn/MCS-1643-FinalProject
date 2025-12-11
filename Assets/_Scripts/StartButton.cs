using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponentInChildren<AudioSource>();
    }
    public void OnClick()
    {
        audioSrc.Play();
        SceneManager.LoadScene("_Scene_0");
        GameManager.StartGame();
        Time.timeScale = 1;
    }
}
