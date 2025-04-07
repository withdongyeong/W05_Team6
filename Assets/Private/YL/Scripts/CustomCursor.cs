using UnityEngine;

public class CustomCursor2D : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D pilotCursor;
    public Vector2 defaultHotspot = new Vector2(280, 130);
    public Vector2 pilotHotspot = new Vector2(280, 130);

    public LayerMask pilotLayerMask;

    private bool isOverPilot = false;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
    }

    void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, pilotLayerMask);

        if (hit.collider != null)
        {
            if (!isOverPilot)
            {
                Cursor.SetCursor(pilotCursor, pilotHotspot, CursorMode.Auto);
                isOverPilot = true;
            }
        }
        else
        {
            if (isOverPilot)
            {
                Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
                isOverPilot = false;
            }
        }
    }
}
