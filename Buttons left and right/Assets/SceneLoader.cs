using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string FADE_OUT_ANIMATION_TRIGGER = "FadeOut";
    private static readonly int FadeOut = Animator.StringToHash(FADE_OUT_ANIMATION_TRIGGER);
    private const float FADE_DURATION = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private MusicManager musicManager;

    public void LoadNextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadStartScene()
    {
        LoadScene(0);
    }

    private void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneCoroutine(sceneIndex));
        if (musicManager != null)
        {
            StartCoroutine(FadeOutMusic());
        }
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        animator.SetTrigger(FadeOut);
        yield return new WaitForSeconds(FADE_DURATION);
        SceneManager.LoadScene(sceneIndex);
    }
    
    private IEnumerator FadeOutMusic()
    {
        var originalVolume = musicManager.AudioSource.volume;
        while (musicManager.AudioSource.volume > 0)
        {
            musicManager.AudioSource.volume -= originalVolume * Time.deltaTime / FADE_DURATION;
            yield return null;
        }
        musicManager.AudioSource.Stop();
        musicManager.AudioSource.volume = originalVolume;
    }
}
