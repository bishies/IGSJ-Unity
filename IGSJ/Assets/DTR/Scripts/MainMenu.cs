using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void QuitGame ()
    {
        StartCoroutine(DelayedQuitGame());
    }

    IEnumerator DelayedQuitGame()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.15f);
        Application.Quit();
    }
}
