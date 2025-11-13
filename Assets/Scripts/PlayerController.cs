using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    PlayerInput input;
    Vector2 worldScreenSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 spriteSize = GetComponentInChildren<SpriteRenderer>().size;

        transform.localScale = Utilities.GetScreenScale(spriteSize, transform.localScale);

        input = GetComponent<PlayerInput>();
        input.actions["Move"].performed += Move;

        worldScreenSize.y = Camera.main.orthographicSize; // don't need to multiply x2 bcs the camera is centred
        worldScreenSize.x = worldScreenSize.y / Screen.height * Screen.width;

        //Removes the size of the collider /2
        spriteSize *= transform.localScale / 2;
        worldScreenSize -= spriteSize;
    }

    void Move(InputAction.CallbackContext _context)
    {
        Vector3 moveInput = _context.ReadValue<Vector2>();
        Vector3 newPos = transform.position + moveInput * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, -worldScreenSize.x, worldScreenSize.x);
        newPos.y = Mathf.Clamp(newPos.y, -worldScreenSize.y, worldScreenSize.y);

        transform.position = newPos;
    }
}
