using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemiesManager : MonoBehaviour
{

    [Header("Enemy info")]
    [SerializeField] EnemyBehaviour enemy;
    [SerializeField] float speedLerp = 1f;
    [SerializeField] float nextShoot = 2f;
    [SerializeField] float laserDuration = 0.25f;

    [Header("Sound")]
    [SerializeField] AudioClip fadeIn;
    [SerializeField] AudioClip[] beamEnd;
    [SerializeField] AudioClip[] beam;

    [Header("Game")]
    [SerializeField] GameObject canvasEnd;

    AudioSource audioSource;

    Vector2 colliderSize;
    Vector2 worldScreenSize;


    PlayerController player;
    List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    Coroutine enemiesCouroutine;
    float fadeInTime = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        colliderSize = enemy.GetComponent<BoxCollider>().size;
        colliderSize *= transform.localScale;
        colliderSize *= Utilities.GetScreenScale(colliderSize, Vector2.one);

        worldScreenSize.y = Camera.main.orthographicSize - colliderSize.y / 2; // don't need to multiply x2 bcs the camera is centred
        worldScreenSize.x = worldScreenSize.y / Screen.height * Screen.width - colliderSize.x / 2;
        player = FindAnyObjectByType<PlayerController>();

        enemiesCouroutine = StartCoroutine(SpawnEnemies());
    }


    Vector3 GetRandPosAroundScreen(out Vector3 enemyPos)
    {
        Vector3 pos = Vector3.zero;

        enemyPos.z = 0;

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

    private void OnDestroy()
    {
        StopCoroutine(enemiesCouroutine);
    }

    IEnumerator SpawnEnemies()
    {
        while (player.nbHp > 0)
        {
            Vector3 endPos = GetRandPosAroundScreen(out Vector3 spawnPos);
            EnemyBehaviour tempEnemy = Instantiate(enemy, spawnPos, Quaternion.identity, transform);
            int indexBeamSound = Random.Range(0, beam.Length);

            tempEnemy.SetEndPos(endPos);
            tempEnemy.SetSpeedLerp(speedLerp);
            tempEnemy.SetLaserDuration(laserDuration);

            enemies.Add(tempEnemy);

            yield return new WaitForSeconds(speedLerp);

            foreach (EnemyBehaviour enemy in enemies)
            {
                enemy.Shoot(nextShoot);
            }

            yield return new WaitForSeconds(nextShoot - fadeInTime);

            audioSource.clip = fadeIn;
            audioSource.Play();

            yield return new WaitForSeconds(fadeInTime);

            audioSource.clip = beam[indexBeamSound];
            audioSource.Play();

            yield return new WaitForSeconds(laserDuration);

            audioSource.clip = beamEnd[indexBeamSound == beam.Length - 1 ? 1 : 0];
            audioSource.Play();
        }

        if(canvasEnd != null)
        {
            canvasEnd.SetActive(true);
        }
    }

    public int GetNbEnemies() { return enemies.Count; }
}
