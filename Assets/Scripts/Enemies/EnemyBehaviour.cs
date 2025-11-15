using Unity.Mathematics;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float timeShoot = 4f;
    [SerializeField] float timeBlink = 0.1f;
    [SerializeField] SpriteRenderer laserSprite;

    float intervalBlink;
    float timerShoot = 0f;
    float timerBlink = 0f;
    float timerLaser = float.MaxValue;
    float durationLaser = 0.25f;

    bool shoot = true;

    PlayerController player;
    float timerLerp = 0;
    float speedLerp = 1f;

    Vector3 startPos;
    Vector3 endPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (laserSprite != null)
        {
            float enemyRotation;

            player = FindAnyObjectByType<PlayerController>();

            enemyRotation = math.atan2(player.transform.position.y - endPos.y, player.transform.position.x - endPos.x);

            transform.rotation = Quaternion.Euler(0, 0, math.degrees(enemyRotation) + 90f);
            transform.localScale = Utilities.GetScreenScale(GetComponentInChildren<BoxCollider>().size, transform.localScale);

            startPos = transform.position;
            
            intervalBlink = timeShoot / 2;
            laserSprite.enabled = false;
            laserSprite.material.DisableKeyword("_EMISSION");

        }
        else
        {
            Debug.LogError("Lasersprite not assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (timerLerp < 1f)
        {
            timerLerp += Time.deltaTime / speedLerp;
            transform.localPosition = Vector3.Lerp(startPos, endPos, timerLerp);

            if (timerLerp + Time.deltaTime / speedLerp > 1f)
            {
                laserSprite.enabled = false;
                timerLerp = 1f;
            }
        }

        if (shoot)
        {
            Blink();
        }

        if (timerLaser <= durationLaser)
        {
            timerLaser += Time.deltaTime;
            laserSprite.material.EnableKeyword("_EMISSION");

            Vector3 realSize = Vector3.Scale(laserSprite.size, laserSprite.transform.localScale);
            Vector3 halfExtents = realSize / 2f;
            Vector3 origin = transform.position - transform.up * halfExtents.y;

            if (Physics.BoxCastAll(origin, halfExtents, -transform.up, transform.rotation, realSize.y, LayerMask.GetMask("Player")).Length > 0)
            {
                player.Hited();
            }

            if (timerLaser > durationLaser)
            {
                laserSprite.material.DisableKeyword("_EMISSION");
                laserSprite.transform.position = laserSprite.transform.position + Vector3.forward;

            }
        }

    }

    void Blink()
    {
        timerBlink += Time.deltaTime;
        timerShoot += Time.deltaTime;

        if (timerShoot > timeShoot)
        {
            timerBlink = 0;
            timerShoot = 0;
            timerLaser = 0;

            intervalBlink = timeShoot / 2;
            laserSprite.enabled = true;
            shoot = false;

            laserSprite.material.EnableKeyword("_EMISSION");
            laserSprite.transform.position = laserSprite.transform.position - Vector3.forward ;

            Debug.Log("Shoot");
            return;
        }

        if (timerBlink > intervalBlink - timeBlink)
        {
            laserSprite.enabled = false;

            if (timerBlink > intervalBlink)
            {
                laserSprite.enabled = true;
                timerBlink = 0;
                intervalBlink /= 2;
            }
        }
    }

    public void Shoot(float _timeShoot)
    {
        timeShoot = _timeShoot;

        timerBlink = 0;
        timerShoot = 0;

        intervalBlink = timeShoot / 2;
        laserSprite.enabled = true;
        shoot = true;
    }

    public void SetEndPos(Vector3 pos) { endPos = pos; }
    public void SetSpeedLerp(float speed) { speedLerp = speed; }
    public void SetLaserDuration(float duration) { durationLaser = duration; }
}
