using UnityEngine;

public class BookIcon : MonoBehaviour
{
    private MainComputer _mainComputer;

    private void Start()
    {
        _mainComputer = FindAnyObjectByType<MainComputer>();
    }

    private void OnMouseDown()
    {
        _mainComputer.Enlarge();
        UIManager.Instance.SetAttackManualScreen();
        UIManager.Instance.OnDisableUI?.Invoke();
    }
}
