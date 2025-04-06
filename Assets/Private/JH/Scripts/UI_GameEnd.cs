using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameEnd : MonoBehaviour
{
    public Sprite GameClearSprite;
    public Sprite GameOverSprite;
    private Canvas _canvas;
    private Image _image;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _image = GetComponent<Image>();
    }

    public void ShowGameEndUI(bool isClear)
    {
        _canvas.enabled = true;
        _image.sprite = isClear ? GameClearSprite : GameOverSprite;
    }
}
