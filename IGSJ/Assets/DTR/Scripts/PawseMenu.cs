using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PawseMenu : MonoBehaviour
{
    public static bool isGamePawsed = false;

    public GameObject pawseMenuUI;

    public string menuScene;

    public AudioSource resumeSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePawsed)
            {
                Resume();
                resumeSound.Play();

            } else
            {
                Pawse();
            }
        }
    }

    public void Resume ()
    {
        pawseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePawsed = false;
    }

    void Pawse ()
    {
        pawseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePawsed = true;
    }

    public void LoadMenu ()
    {
        StartCoroutine(DelayedLoadMenu());
    }

    public void QuitGame()
    {
        StartCoroutine(DelayedQuitGame());
    }

    IEnumerator DelayedLoadMenu()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(menuScene);
    }

    IEnumerator DelayedQuitGame()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.15f);
        Application.Quit();
    }
}
