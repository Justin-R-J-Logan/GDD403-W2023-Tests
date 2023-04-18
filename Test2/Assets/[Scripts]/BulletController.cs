using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

//BulletController.cs
//Justin Logan
//A00066979
//04-18-2023

[System.Serializable]
public class BulletController : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public BulletType type;

    private BulletManager bulletManager;
    private GameObject smokePrefab;
    private Transform bulletParent;
    void Awake()
    {
        smokePrefab = Resources.Load<GameObject>("Prefabs/Smoke");
    }

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
        bulletParent = GameObject.Find("[BULLETS]").transform;
    }

    public void Activate()
    {
        var turretSound = GetComponent<AudioSource>();
        turretSound.pitch = Random.Range(0.5f, 3.0f);
        turretSound.Play();
        Invoke("DestroyYourSelf", 3.0f);
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (type == BulletType.SEEKING)
        {
            var target = FindObjectOfType<EnemyController>().transform.position;
            if (Vector3.Distance(target, transform.position) <= 1.0f)
            {
                direction = Vector3.Normalize(target - transform.position);
            }

            var rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
        }

        if (direction == Vector3.zero)
        {
            var node = FindObjectOfType<Graph>().FindClosestNode(transform.position);
            direction = Vector3.Normalize(node.transform.position - transform.position);
        }

        transform.position += (direction * speed * Time.deltaTime);
    }

    public void DestroyYourSelf()
    {
        if (gameObject.activeInHierarchy)
        {
            bulletManager.ReturnBullet(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Graph")
        {
            switch (other.gameObject.tag)
            {
                case "Player":
                {
                    if (type == BulletType.ENEMY)
                    {
                        other.gameObject.GetComponent<TankBehaviour>().health.TakeDamage(10);
                        Explode(other);
                    }
                }
                    break;
                case "Enemy":
                {
                    other.gameObject.GetComponent<EnemyController>().health.TakeDamage(10);
                    Explode(other);
                }
                    break;
                default:
                {
                    if (type != BulletType.SEEKING)
                    {
                        Explode(other);
                    }
                }
                    break;
            }

        }

    }

    private void Explode(Collider2D other)
    {
        var explosionPoint = other.ClosestPoint(transform.position);
        DestroyYourSelf();

        // Create Smoke at the "hit" location
        Instantiate(smokePrefab, explosionPoint, Quaternion.identity, bulletParent);
    }
}
