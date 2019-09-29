using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public string gameScene;
    public void PlayGame ()
    {
        StartCoroutine(DelayedPlayGame());
    }

    IEnumerator DelayedPlayGame()
    {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(gameScene);
    }
}
