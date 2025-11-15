using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public int nbHp = 3;

    [SerializeField] float timeInvincible = 1f;
    [SerializeField] float blinkInterval = 0.1f;

    float timerBlink;
    float timerInvincible;
    bool isInvincible = false;


    PlayerInput input;
    Vector2 worldScreenSize;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        transform.localScale = Utilities.GetScreenScale(spriteRenderer.size, transform.localScale);

        input = GetComponent<PlayerInput>();
        input.actions["Move"].performed += Move;

        worldScreenSize.y = Camera.main.orthographicSize; // don't need to multiply x2 bcs the camera is centred
        worldScreenSize.x = worldScreenSize.y / Screen.height * Screen.width;

        //Removes the size of the collider /2
        worldScreenSize -= spriteRenderer.size * transform.localScale / 2;

        timerInvincible = timeInvincible;
    }

    private void Update()
    {
        if (isInvincible)
        {
            timerInvincible += Time.deltaTime;
            timerBlink += Time.deltaTime;

            // Clignotement
            if (timerBlink >= blinkInterval)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                timerBlink = 0f;
            }

            // Fin de l'invincibilité
            if (timerInvincible >= timeInvincible)
            {
                isInvincible = false;
                spriteRenderer.enabled = true;
            }
        }

    }

    private void OnDestroy()
    {
            input.actions["Move"].performed -= Move;
    }
    void Move(InputAction.CallbackContext _context)
    {
        Vector3 moveInput = _context.ReadValue<Vector2>();
        Vector3 newPos = transform.position + (moveInput * Time.deltaTime) / 4f;

        newPos.x = Mathf.Clamp(newPos.x, -worldScreenSize.x, worldScreenSize.x);
        newPos.y = Mathf.Clamp(newPos.y, -worldScreenSize.y, worldScreenSize.y);

        transform.position = newPos;
    }

    public void Hited()
    {
        if (!isInvincible && nbHp > 0)
        {
            nbHp--;

            if (nbHp <= 0)
            {
                spriteRenderer.enabled = false;
                return;
            }

            isInvincible = true;
            timerInvincible = 0f;
            timerBlink = 0f;
            spriteRenderer.enabled = false;
            

        }
    }



}
