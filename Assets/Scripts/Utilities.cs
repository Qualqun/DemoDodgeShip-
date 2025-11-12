using UnityEngine;
using UnityEngine.UIElements;

static public class Utilities
{
    static Vector2 defaultSize = new Vector2(1242, 2688);
    static public Vector3 GetScreenScale(SpriteRenderer _spriteRenderer, Vector2 defaultScale)
    {
        Vector3 scale = Vector3.one;
        float spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = _spriteRenderer.sprite.bounds.size.y;

        // Cam size
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Cross product -> Screen.Width / DefalutSize * (ActualScale)
        scale.x = Screen.width / defaultSize.x * defaultScale.x;
        scale.y = Screen.height / defaultSize.y * defaultScale.y;

        return scale;
    }
}
