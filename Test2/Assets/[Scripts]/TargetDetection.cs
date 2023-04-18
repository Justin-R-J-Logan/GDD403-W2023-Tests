using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TargetDirection.cs
//Justin Logan
//A00066979
//04-18-2023


public class TargetDetection : MonoBehaviour
{
    [Header("Sensing Suite")] 
    public LayerMask collisionLayerMask;
    public bool isTargetDetected;
    public bool hasLOS;

    [Header("Targeting Properties")]
    public Transform targetTransform;
    public Transform turretTransform;

    [Header("Bullet Firing Properties")] 
    public float fireDelay;
    public Transform bulletSpawn;

    private float targetDirection;
    private Vector2 targetDirectionVector;
    private Collider2D colliderName;
    private BulletManager bulletManager;


    // Start is called before the first frame update
    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();

        targetDirectionVector = Vector2.zero;
        targetDirection = 0;

        targetTransform = FindObjectOfType<TankBehaviour>().transform;

        isTargetDetected = false;
        hasLOS = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargetDetected)
        {
            var hit = Physics2D.Linecast(transform.position, targetTransform.position, collisionLayerMask);
            colliderName = hit.collider;

            targetDirectionVector = (targetTransform.position - transform.position);
            targetDirectionVector.Normalize(); // creates a unit vector (magnitude of 1)
            targetDirection = Mathf.Atan2(targetDirectionVector.x, -targetDirectionVector.y) * Mathf.Rad2Deg;

            hasLOS = (hit.collider.gameObject.CompareTag("Player"));
            if (hasLOS)
            {
                var targetAngle = targetDirection - transform.localEulerAngles.z - 90.0f;
                turretTransform.localEulerAngles = new Vector3(0.0f, 0.0f, targetAngle);

                if (Time.frameCount % fireDelay == 0)
                {
                    FireBullet(targetAngle);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTargetDetected = true;
        }
    }

    private void FireBullet(float targetAngle)
    {
        var bullet = bulletManager.GetBullet(bulletSpawn.position);
        var attackAngle = (targetAngle + (transform.localEulerAngles.z)) * Mathf.Deg2Rad;
        var bulletDirection = new Vector3((float)Mathf.Cos(attackAngle), (float)Mathf.Sin(attackAngle), 0.0f);
        bullet.GetComponent<BulletController>().type = BulletType.ENEMY;
        bullet.GetComponent<BulletController>().direction = bulletDirection;
        bullet.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
        bullet.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (targetAngle + transform.localEulerAngles.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (hasLOS) ? Color.green : Color.red;

        if (isTargetDetected)
        {
            Gizmos.DrawLine(transform.position, targetTransform.position);
        }

        Gizmos.color = (isTargetDetected) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 4.75f);
    }
}
