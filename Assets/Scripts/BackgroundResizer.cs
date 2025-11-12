using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] int nbScreenBackGround = 2;


    Material backgroundMat;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        float width = renderer.sprite.bounds.size.x;
        float height = renderer.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 scale = Vector3.one;


        scale.x = worldScreenWidth / width;
        scale.y = worldScreenHeight / height * nbScreenBackGround;

        transform.localScale = scale;
        backgroundMat = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {

        backgroundMat.mainTextureOffset += Vector2.down * (scrollSpeed * Time.deltaTime);
    }
}
