using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] float amplitude = 0.5f;   // Hauteur du mouvement
    [SerializeField] float frequency = 2f;     // Vitesse d’oscillation

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}
