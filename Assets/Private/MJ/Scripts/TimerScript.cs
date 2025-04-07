using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    Enemy enemy;
    TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        enemy = GameManager.Instance.Enemy;
        enemy.timeFlowed += ChangeText;
    }

    private void ChangeText(float time)
    {
        if (time < 0)
            time = 0;
        time = Mathf.FloorToInt(time);
        text.text = "예상 시간\n" + time;
    }
}
