using Unity.Mathematics;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float durationLaser = 0.25f;
    [SerializeField] float timeShoot = 4f;
    [SerializeField] float timeBlink = 0.1f;
    [SerializeField] SpriteRenderer laserSprite;

    float intervalBlink;
    float timerShoot = 0f;
    float timerBlink = 0f;
    float timerLaser = float.MaxValue;

    [SerializeField] bool shoot = true;

    Transform playerTransfrom;
    float timerLerp = 0;
    float speedLerp = 1f;

    Vector2 startPos;
    Vector2 endPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (laserSprite != null)
        {
            float enemyRotation;

            playerTransfrom = FindAnyObjectByType<PlayerController>().transform;

            enemyRotation = math.atan2(playerTransfrom.position.y - endPos.y, playerTransfrom.position.x - endPos.x);

            transform.rotation = Quaternion.Euler(0, 0, math.degrees(enemyRotation) + 90f);
            transform.localScale = Utilities.GetScreenScale(GetComponentInChildren<BoxCollider>().size, transform.localScale);

            startPos = transform.position;

            intervalBlink = timeShoot / 2;
            laserSprite.enabled = false;
        }
        else
        {
            Debug.LogError("Lasersprite not assigned");
        }
    }

    void OnDrawGizmos()
    {
        if (!laserSprite) return;

        // --- paramètres identiques à ton test ---
        Vector3 realSize = Vector3.Scale(laserSprite.size, laserSprite.transform.localScale);
        Vector3 halfExtents = realSize / 2f;
        Vector3 origin = transform.position - transform.up * halfExtents.y;
        Vector3 direction = -transform.up;
        float distance = realSize.y;

        // --- Debug visuel principal ---
        Vector3 endPoint = origin + direction * distance;

        // Cube de départ (vert)
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(origin, transform.rotation, halfExtents * 2f);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        // Cube de fin (rouge transparent)
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.matrix = Matrix4x4.TRS(endPoint, transform.rotation, halfExtents * 2f);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        // Ligne entre les deux (jaune)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, endPoint);

        // --- Debug collision ---
        RaycastHit[] hits = Physics.BoxCastAll(origin, halfExtents, direction, transform.rotation, distance, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            Gizmos.color = Color.magenta;
            foreach (var hit in hits)
            {
                Gizmos.matrix = Matrix4x4.TRS(hit.point, transform.rotation, halfExtents * 2f);
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (timerLerp < 1f)
        {
            timerLerp += Time.deltaTime / speedLerp;
            transform.position = Vector2.Lerp(startPos, endPos, timerLerp);

            if(timerLerp + Time.deltaTime / speedLerp > 1f)
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
                Debug.Log("Oui");
            }

            if (timerLaser > durationLaser)
            {
                laserSprite.material.DisableKeyword("_EMISSION");
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

    public void SetEndPos(Vector2 pos) { endPos = pos; }
    public void SetSpeedLerp(float speed) { speedLerp = speed; }
}
