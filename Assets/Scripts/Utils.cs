using UnityEngine;

public class Utils
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static void MarkPoint(Vector2 point, Color? color = null, float time = 0.0f, float size = 0.1f)
    {
        Debug.DrawRay(point + new Vector2(-1, 1).normalized * size, 2 * size * new Vector2(1, -1).normalized, color ?? Color.white, time);
        Debug.DrawRay(point + new Vector2(1, 1).normalized * size, 2 * size * new Vector2(-1, -1).normalized, color ?? Color.white, time);
    }

    public static float RoundToIncrement(float value, float increment)
    {
        return Mathf.Round(value / increment) * increment;
    }
}
