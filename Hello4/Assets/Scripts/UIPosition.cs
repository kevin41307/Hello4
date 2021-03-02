using UnityEngine;

public class UIPosition
{
    public static Vector3 WorldToUI(Vector3 pos, RectTransform rootRect)
    {
        Vector2 screenPos = Game.mainCamera.WorldToViewportPoint(pos); //世界物件在螢幕上的座標，螢幕左下角為(0,0)，右上角為(1,1)
        if (screenPos.x < 0 || screenPos.x > 1f || screenPos.y < 0 || screenPos.y > 1f)
            return Game.vectorTenThousands;
        Vector2 viewPos = (screenPos - rootRect.pivot) * 2; //世界物件在螢幕上轉換為UI的座標，UI的Pivot point預設是(0.5, 0.5)，這邊把座標原點置中，並讓一個單位從0.5改為1
        float width = rootRect.rect.width / 2; //UI一半的寬，因為原點在中心
        float height = rootRect.rect.height / 2; //UI一半的高
        //Debug.Log(new Vector3(screenPos.x, screenPos.y, 0));
        return new Vector3(viewPos.x * width, viewPos.y * height, 0); ; //回傳UI座標   
    }
    /*
    public static bool IsInScreen(Vector3 pos)
    {
        Vector2 screenPos = Game.mainCamera.WorldToViewportPoint(pos);
        //Debug.Log(screenPos);
        if (screenPos.x < 0 || screenPos.x > 1f || screenPos.y < 0 || screenPos.y > 1f)
            return false;
        else return true;
    }
    */
    /*
if ( !Game.IsInFrontOfCamera(pos))
{
    screenPos.x = 1 - Mathf.Clamp01(screenPos.x);
    screenPos.y = 0f;
    Debug.Log(screenPos);
}
*/
}
