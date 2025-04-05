using UnityEngine;

public class BookIcon : MonoBehaviour
{
    private MainComputer _mainComputer;

    private void Start()
    {
        _mainComputer = GetComponentInParent<MainComputer>();
    }

    private void OnMouseDown()
    {
        _mainComputer.Enlarge();
        UIManager.Instance.OnDisableUI?.Invoke();
    }
}
