using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private SceneLoader sceneLoader;
    private const float DELAY_BETWEEN_TEXT = 5f;

    private static readonly string[] CREDITS = 
    {
        "Zombeats",
        "Made by\nNanashi no Gombe",
        "Music by Victormame",
        "Thaleah fat font by\nTiny Worlds",
        "Minifantasy dungeon by\nKrishna Palacio",
        "Thanks for playing",
        "...",
        "Okay, that's all",
        "...",
        "You could go do the dishes, you know",
        "Or take your dog for a walk",
        "...",
        "Turn your computer off and go to sleep"
    };

    private void Start()
    {
        StartCoroutine(ShowCredits());
    }

    private IEnumerator ShowCredits()
    {
        foreach (var message in CREDITS)
        {
            text.text = message;
            yield return new WaitForSeconds(DELAY_BETWEEN_TEXT);
        }
    }
}
