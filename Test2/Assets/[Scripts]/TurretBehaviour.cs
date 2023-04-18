using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TurretBehavior.cs
//Justin Logan
//A00066979
//04-18-2023

public class TurretBehaviour : MonoBehaviour
{
    public Transform bulletSpawn;

    private Vector3 target;
    private BulletManager bulletManager;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
    }

    void Update()
    {
        TurnTurretToFaceMouse();
        FireBullets();
    }

    private void TurnTurretToFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        target = Vector3.Normalize(Camera.main.ScreenToWorldPoint(mousePos) - transform.position);

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(target.x, -target.y) * Mathf.Rad2Deg - 90.0f);
    }

    private void FireBullets()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        {
            var bullet = bulletManager.GetBullet(bulletSpawn.position);
            var attackAngle = (transform.localEulerAngles.z + (transform.parent.transform.localEulerAngles.z)) * Mathf.Deg2Rad;
            var bulletDirection = new Vector3((float)Mathf.Cos(attackAngle), (float)Mathf.Sin(attackAngle), 0.0f);
            bullet.GetComponent<BulletController>().type = BulletType.DIRECT_FIRE;
            bullet.GetComponent<BulletController>().direction = bulletDirection;
            bullet.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
            bullet.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (transform.localEulerAngles.z + transform.parent.transform.localEulerAngles.z));
        }
        else if (Input.GetMouseButtonDown(1)) // Right Mouse Button
        {
            var bullet = bulletManager.GetBullet(bulletSpawn.position);
            var attackAngle = (transform.localEulerAngles.z + (transform.parent.transform.localEulerAngles.z)) * Mathf.Deg2Rad;
            var bulletDirection = new Vector3((float)Mathf.Cos(attackAngle), (float)Mathf.Sin(attackAngle), 0.0f);
            bullet.GetComponent<BulletController>().type = BulletType.SEEKING; //Seeking instead of direct fire
            bullet.GetComponent<BulletController>().direction = bulletDirection;
            bullet.GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
            bullet.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (transform.localEulerAngles.z + transform.parent.transform.localEulerAngles.z));
        }
    }
}
