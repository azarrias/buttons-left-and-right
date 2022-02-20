using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private SceneLoader sceneLoader;
    private const float DELAY_AFTER_TITLE = 5f;

    private const string GAME_TITLE = "Zombeats";

    private void Start()
    {
        StartCoroutine(ShowCredits());
    }

    private IEnumerator ShowCredits()
    {
        text.text = GAME_TITLE;
        yield return new WaitForSeconds(DELAY_AFTER_TITLE);
        sceneLoader.LoadNextScene();
    }
}