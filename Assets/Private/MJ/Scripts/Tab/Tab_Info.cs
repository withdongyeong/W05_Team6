using UnityEngine;

public class Tab_Info : MonoBehaviour, ITab
{
    public void StartTab()
    {
        gameObject.SetActive(true);
    }

    public void UpdateTab()
    {
       
    }

    public void StopTab()
    {
        gameObject.SetActive(false);
    }
}
