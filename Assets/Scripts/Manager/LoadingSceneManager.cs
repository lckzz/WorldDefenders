using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LoadingSceneManager
{
    public static Define.Scene nextScene;

    public void LoadScene(Define.Scene scenetype)
    {
        nextScene = scenetype;
        SceneManager.LoadScene("LoadingScene");
        Time.timeScale = 1.0f;
    }

    public IEnumerator LoadScene(Image prograssBar)
    {
        yield return null;
        AsyncOperation op = Managers.Scene.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (timer <= .9f)
        {
            yield return null;
            timer += Time.deltaTime / 3.5f;
            prograssBar.fillAmount = timer;
        }

        timer = .0f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (op.progress < 0.9f)
            {
                prograssBar.fillAmount = Mathf.Lerp(prograssBar.fillAmount, op.progress, timer);
                //prograssBar.fillAmount = op.progress;
                if (prograssBar.fillAmount >= op.progress)
                    timer = .0f;
            }
            else
            {

                //prograssBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                prograssBar.fillAmount = 1.0f;
                if(prograssBar.fillAmount >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
