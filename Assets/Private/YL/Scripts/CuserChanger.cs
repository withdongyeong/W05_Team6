using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D pilotCursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public LayerMask pilotLayerMask;

    private bool isOnPilot = false;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, pilotLayerMask))
        {
            if (!isOnPilot)
            {
                Cursor.SetCursor(pilotCursorTexture, hotSpot, cursorMode);
                isOnPilot = true;
            }
        }
        else
        {
            if (isOnPilot)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                isOnPilot = false;
            }
        }
    }
}
