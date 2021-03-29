using UnityEngine;

//Draws the box selection when left mouse button is selected
public static class SelectionBox 
{
    private static Texture2D boxTexture;
    public static Texture2D BoxTexture
    {
        get
        {
            if (boxTexture == null)
            {
                boxTexture = new Texture2D(1, 1);
                boxTexture.SetPixel(0, 0, Color.blue);
                boxTexture.Apply();
            }
            return boxTexture;
        }
    }


    //Gets position of mouse in screenspace so it can be used to position the screen rect
    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    //Gets viewport bounds for selection box to grab objects within selection
    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    //dRaws the inside space of the box
    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, BoxTexture);
        GUI.color = Color.blue;
    }

    //Draws border of the selection box
    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        //Draws the top border of the selection box
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        //Draws the left border of the selection box
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        //Draws the right border of the selection box
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        //Draws the bottom border of the selection box
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

}

