using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public string nextLevelName; // Название следующей сцены
    public Image fadeOverlay; // UI элемент для фейда
    public float fadeDuration = 1f; // Длительность фейда

    public SceneField persistentGameplay;
    public SceneField levelScene;

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string levelName)
    {
        StartCoroutine(FadeAndLoadScene(levelName));
    }

    private IEnumerator FadeAndLoadScene(string levelName)
    {
        yield return Fade(1); // Фейд к черному
        scenesToLoad.Add(SceneManager.LoadSceneAsync(persistentGameplay));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive));
        yield return new WaitUntil(() => scenesToLoad.TrueForAll(op => op.isDone));
        yield return Fade(0); // Фейд обратно
    }

    private IEnumerator FadeAndReloadScene()
    {
        yield return Fade(1); // Фейд к черному
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return Fade(0); // Фейд обратно
    }

    public IEnumerator Fade(float targetAlpha)
    {
        float speed = Mathf.Abs(fadeOverlay.color.a - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(fadeOverlay.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(fadeOverlay.color.a, targetAlpha, speed * Time.deltaTime);
            fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, alpha);
            yield return null;
        }
    }
}