using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PressToContinue : MonoBehaviour
{
    [SerializeField] float timeBlink = 0.25f;

    [SerializeField] TMP_Text text;

    Coroutine blinkCouroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TMP_Text>();

        blinkCouroutine = StartCoroutine(Blink());
    }

    private void Update()
    {

        if (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true || Keyboard.current?.anyKey.wasPressedThisFrame == true)
        {
            StopCoroutine(blinkCouroutine);
            SceneManager.LoadScene("Menu");
        }

       

    }
    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBlink);

            text.enabled = !text.enabled;
        }
    }
}
