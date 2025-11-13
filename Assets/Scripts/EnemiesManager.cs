using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{

    [SerializeField] EnemyBehaviour enemy;
    [SerializeField] Vector2 worldScreenSize;
    [SerializeField] float speedLerp = 1f;
    [SerializeField] float nextShoot = 2f;

    Vector2 colliderSize;

    List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    IEnumerator enemiesCouroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderSize = enemy.GetComponent<BoxCollider>().size;
        colliderSize *= transform.localScale;
        colliderSize *= Utilities.GetScreenScale(colliderSize, Vector2.one);

        worldScreenSize.y = Camera.main.orthographicSize - colliderSize.y / 2; // don't need to multiply x2 bcs the camera is centred
        worldScreenSize.x = worldScreenSize.y / Screen.height * Screen.width - colliderSize.x / 2;

        StartCoroutine(SpawnEnemies());
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

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Vector2 endPos = GetRandPosAroundScreen(out Vector2 spawnPos);
            EnemyBehaviour tempEnemy = Instantiate(enemy, spawnPos, Quaternion.identity, transform);

            tempEnemy.SetEndPos(endPos);
            tempEnemy.SetSpeedLerp(1f);

            enemies.Add(tempEnemy);

            yield return new WaitForSeconds(speedLerp * 2);

            foreach (EnemyBehaviour enemy in enemies)
            {
                enemy.Shoot(nextShoot);
            }

            yield return new WaitForSeconds(nextShoot);
        }
    }
}
