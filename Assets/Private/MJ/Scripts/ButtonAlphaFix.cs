using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAlphaFix : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    Image image;   
    private Color glowColor;
    private bool isHovering = false;
    float currentTime = 0;
    
    private void Awake()
    {
        image = GetComponent<Image>();
        if (image!=null)
        {
            image.alphaHitTestMinimumThreshold = 0.2f;
        }
        glowColor = image.color;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering)
        {
            currentTime += Time.deltaTime * 4f;
            float t = (Mathf.Sin(currentTime) + 0.6f) / 2.5f;
            image.color = new Color(glowColor.r, glowColor.g, glowColor.b, t);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        image.color = new Color(glowColor.r, glowColor.g, glowColor.b, 1f);
        currentTime = 0;
    }
}
