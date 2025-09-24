using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    public void GoToScene(string name)
    {
        StartCoroutine(GoToSceneRoutine(name));
    }

    IEnumerator GoToSceneRoutine(string name)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        SceneManager.LoadScene(name);
    }
}
