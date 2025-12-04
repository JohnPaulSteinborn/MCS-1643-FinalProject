using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public GameObject PauseMenu;

    private AudioSource audioSrc;

    private void Awake()
    {
        audioSrc = GetComponentInChildren<AudioSource>();
    }
    public void OnClick()
    {
        audioSrc.Play();
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
