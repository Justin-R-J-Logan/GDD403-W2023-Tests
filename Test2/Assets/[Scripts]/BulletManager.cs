using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BulletManager.cs
//Justin Logan
//A00066979
//04-18-2023

[System.Serializable]
public class BulletManager : MonoBehaviour
{
    [Header("Bullet Pool Properties")]
    public int poolSize;

    private Transform bulletParent;
    private GameObject bulletPrefab;
    private Queue<GameObject> bulletPool;

    void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

        bulletParent = GameObject.Find("[BULLETS]").transform;

        bulletPool = new Queue<GameObject>(); // creates an empty container
    }


    // Start is called before the first frame update
    void Start()
    {
        BuildBulletPool();
    }

    /// <summary>
    /// This method creates a Pool of Bullets
    /// </summary>
    private void BuildBulletPool()
    {
        for (var i = 0; i < poolSize; i++)
        {
            bulletPool.Enqueue(CreateBullet());
        }
    }

    /// <summary>
    /// This method creates a single bullet and returns its reference
    /// </summary>
    /// <returns></returns>
    private GameObject CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab, Vector2.zero, Quaternion.identity, bulletParent);
        bullet.SetActive(false);
        return bullet;
    }

    /// <summary>
    /// This method gets a bullet from the bullet pool and returns it to the calling function
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GetBullet(Vector2 position)
    {
        // if we don't have any bullets left in the pool...
        if (bulletPool.Count < 1)
        {
            //...create a new bullet
            bulletPool.Enqueue(CreateBullet());
        }

        var bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        bullet.GetComponent<BulletController>().Activate();
        bullet.transform.position = position;
        return bullet;
    }

    /// <summary>
    /// This method returns a bullet back to the pool and resets its properties
    /// </summary>
    /// <param name="bullet"></param>
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.position = Vector2.zero;
        bullet.transform.rotation = Quaternion.identity;
        bullet.GetComponent<BulletController>().direction = Vector3.zero;
        bulletPool.Enqueue(bullet);
    }

}
