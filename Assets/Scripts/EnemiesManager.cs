using System.Collections;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{

    [SerializeField] EnemyBehaviour enemy;
    [SerializeField] Vector2 worldScreenSize;

    Vector2 colliderSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderSize = enemy.GetComponent<BoxCollider>().size;
        colliderSize *= transform.localScale;
        colliderSize *= Utilities.GetScreenScale(colliderSize, Vector2.one);

        worldScreenSize.y = Camera.main.orthographicSize - colliderSize.y / 2; // don't need to multiply x2 bcs the camera is centred
        worldScreenSize.x = worldScreenSize.y / Screen.height * Screen.width - colliderSize.x / 2;

        StartCoroutine(TestRand());
    }

    // Update is called once per frame
    void Update()
    {

    }


    Vector2 GetRandPosAroundScreen(out Vector2 enemyPos)
    {
        Vector2 pos = Vector2.zero;

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            // rand x
            pos.x = UnityEngine.Random.Range(-worldScreenSize.x, worldScreenSize.x);
            pos.y = UnityEngine.Random.Range(0, 2) == 0 ? -worldScreenSize.y : worldScreenSize.y;

            enemyPos.x = pos.x;
            enemyPos.y = pos.y + colliderSize.y * (pos.y / worldScreenSize.y) * 2;
        }
        else
        {
            // rand y
            pos.x = UnityEngine.Random.Range(0, 2) == 0 ? -worldScreenSize.x : worldScreenSize.x; ;
            pos.y = UnityEngine.Random.Range(-worldScreenSize.y, worldScreenSize.y);

            enemyPos.x = pos.x + colliderSize.x * (pos.x / worldScreenSize.x) * 2;
            enemyPos.y = pos.y;
        }


        return pos;
    }

    IEnumerator TestRand()
    {
        while (true)
        {
            Vector2 randPos = GetRandPosAroundScreen(out Vector2 spawnPos);

            Instantiate(enemy, spawnPos, Quaternion.identity).SetEndPos(randPos);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
