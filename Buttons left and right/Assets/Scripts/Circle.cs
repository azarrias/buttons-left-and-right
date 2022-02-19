using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    [SerializeField] private Image highlightImage;
    
    public void Highlight(Color highlightColor, float duration)
    {
        highlightImage.color = highlightColor;
        highlightImage.gameObject.SetActive(true);
        StartCoroutine(HighlightCoroutine(duration));
    }
    
    IEnumerator HighlightCoroutine(float duration)
    {
        var startTime = Time.time;
        var interpolationPoint = 0f;
        var color = highlightImage.color;
        while (interpolationPoint < 1f)
        {
            interpolationPoint = (Time.time - startTime) / duration;
            var alpha = Mathf.Lerp(1, 0, interpolationPoint);
            color.a = alpha;
            highlightImage.color = color;
            yield return null;
        }
        highlightImage.gameObject.SetActive(false);
    }
}
