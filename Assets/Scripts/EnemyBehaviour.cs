using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float speedLerp = 1f;

    Transform playerTransfrom;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    float timerLerp = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float enemyRotation;

        playerTransfrom = FindAnyObjectByType<PlayerController>().transform;

        enemyRotation = math.atan2(playerTransfrom.position.y - transform.position.y, playerTransfrom.position.x - transform.position.x);

        transform.rotation = Quaternion.Euler(0, 0, math.degrees(enemyRotation) + 90);
        transform.localScale = Utilities.GetScreenScale(GetComponentInChildren<BoxCollider>().size, transform.localScale);

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
        //if (Physics.BoxCast(transform.position, transform.localScale / 2, -transform.up, quaternion.identity, float.MaxValue, LayerMask.GetMask("Player")))
        //{
        //    Debug.Log("Oui");
        //}

        if (timerLerp < 1f)
        {
            timerLerp = timerLerp + Time.deltaTime / speedLerp > 1f ? 1f : timerLerp + Time.deltaTime / speedLerp;

            transform.position = Vector2.Lerp(startPos, endPos, timerLerp);
        }
    }

    public void SetEndPos(Vector2 pos) { endPos = pos; }
}
