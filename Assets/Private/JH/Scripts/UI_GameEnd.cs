using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameEnd : MonoBehaviour
{
    public Sprite GameClearSprite;
    public Sprite GameOverSprite;
    private Canvas _canvas;
    private Image _image;
    private TextMeshProUGUI _gameEndText;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _image = GetComponent<Image>();
        _gameEndText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ShowGameEndUI(bool isClear)
    {
        _canvas.enabled = true;
        _gameEndText.text = isClear ? "Game Clear" : "Game Over";
        _image.sprite = isClear ? GameClearSprite : GameOverSprite;
    }
}
