using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Text textButton;
    private readonly Color32 redish = new Color32(191, 30, 26, 180);
    private Color previousColor;

    private void Start()
    {
        textButton = GetComponentInChildren<Text>();
        previousColor = textButton.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textButton.color = redish; //Color.red; //Or however you do your color
        textButton.fontStyle = FontStyle.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textButton.color = previousColor; //Or however you do your color
        textButton.fontStyle = FontStyle.Normal;
    }
}