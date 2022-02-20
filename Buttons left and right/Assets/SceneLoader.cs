using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string FADE_OUT_ANIMATION_TRIGGER = "FadeOut";
    private static readonly int FadeOut = Animator.StringToHash(FADE_OUT_ANIMATION_TRIGGER);
    private const float FADE_DURATION = 2f;
    [SerializeField] private Animator animator;

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        animator.SetTrigger(FadeOut);
        yield return new WaitForSeconds(FADE_DURATION);
        SceneManager.LoadScene(sceneIndex);
    }
}
