using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource != null && audioSource.clip != null)
        {
            float half = audioSource.clip.length / 2f;
            audioSource.time = half;

            audioSource.Play();
        }

    }
    public void Play()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
