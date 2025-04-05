using DG.Tweening;
using UnityEngine;

public class MainComputer : MonoBehaviour
{
    private Vector3 _defaultScale;
    [SerializeField] private Vector3 _enlargeScale;

    private void Start()
    {
        _defaultScale = transform.localScale;

        UIManager.Instance.OnResetUI += ResetScale;
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnResetUI -= ResetScale;
    }

    public void Enlarge()
    {
        transform.DOMoveZ(transform.position.z - 1, 0.2f);
        transform.DOScale(_enlargeScale, 0.2f);
    }

    public void ResetScale ()
    {
        transform.DOMoveZ(transform.position.z + 1, 0.2f);
        transform.DOScale(_defaultScale, 0.2f);
    }
}
