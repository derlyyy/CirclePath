using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : MonoBehaviour
{
    public bool isTrigger;

    [SerializeField] private SceneField sceneToLoad;
    [SerializeField] private SceneField sceneToUnload;

    public Collider2D collider;
    public ParticleSystem completePart;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTrigger)
        {
            if (other.CompareTag("PlayerCheck"))
            {
                completePart.Play();
                PlayerManager.instance.LevelComplete();
                StartCoroutine(LoadScene());
            }
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        UnloadScene();
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
