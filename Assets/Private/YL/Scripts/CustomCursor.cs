using UnityEngine;

//커서 이미지 변경 및 커서 텍스처 이미지에서 클릭 위치 조절
public class CustomCursor : MonoBehaviour
{
    public Texture2D Customcursor;
    void Start()
    {
        Vector2 hotspot = new Vector2(280, 130);
        Cursor.SetCursor(Customcursor, hotspot, CursorMode.Auto);
    }
}
