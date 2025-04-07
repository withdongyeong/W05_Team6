using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManualTab : MonoBehaviour
{
    public int ManualIndex;

    private void OnMouseDown()
    {
        Debug.Log("Tab Clicked");
        if (ManualIndex == 0)
        {
            UIManager.Instance.SetAttackManualScreen();
        }
        if (ManualIndex == 1)
        {
            UIManager.Instance.SetDefenseManualScreen();
        }
    }
}
