using UnityEngine;

public class UI_Background : MonoBehaviour
{

    private void Start()
    {
        UIManager.Instance.OnResetUI += InactiveSelf;
        UIManager.Instance.OnDisableUI += ActiveSelf;
        InactiveSelf();
    }
    
    private void OnDestroy()
    {
        UIManager.Instance.OnResetUI -= InactiveSelf;
        UIManager.Instance.OnDisableUI -= ActiveSelf;
    }
    private void OnMouseDown()
    {
        UIManager.Instance.OnResetUI?.Invoke();
    }

    public void ActiveSelf()
    {
        gameObject.SetActive(true);
    }
    public void InactiveSelf()
    {
        gameObject.SetActive(false);
    }
}
