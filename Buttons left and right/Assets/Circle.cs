using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    [SerializeField] private Image highlightImage;
    
    public void Highlight(Color highlightColor)
    {
        highlightImage.gameObject.SetActive(true);
        highlightImage.color = highlightColor;
    }
}
