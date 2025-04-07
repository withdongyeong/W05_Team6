using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShatterEffect : MonoBehaviour
{
    RawImage rawimage;
    private void Awake()
    {
        rawimage = GetComponent<RawImage>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.Player.ShatterAction += StartShatterEffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShatterEffectCoroutine()
    {
        Color color = rawimage.color;
        color.a = 1f;
        rawimage.color = color;
        yield return new WaitForSeconds(GlobalSettings.Instance.ShatterEffectDuration);
        float currentTime = 0f;
        while(currentTime < 2.5f)
        {
            color.a = Mathf.Lerp(1, 0, currentTime / 2.5f);
            rawimage.color = color;
            currentTime += Time.deltaTime;
            yield return null;
        }

    }

    private void StartShatterEffect()
    {
        StartCoroutine("ShatterEffectCoroutine");
    }
}
