using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TurretBehavior.cs
//Justin Logan
//A00066979
//04-18-2023


public class TankBehaviour : MonoBehaviour
{
    [Header("Movement Properties")]
    public float rotationRate;
    public float speed;
    public Vector3 direction;

    [Header("Health System")] 
    public HealthSystem health;

    // Update is called once per frame
    void Update()
    {
        MoveTankBody();
    }

    private void MoveTankBody()
    {
        var rotationAngle = Input.GetAxisRaw("Horizontal") * rotationRate * Time.deltaTime * -1.0f;
        var movementRate = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        transform.Rotate(Vector3.forward, rotationAngle);
        var radiansAngle = transform.localEulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3((float)Mathf.Cos(radiansAngle), (float)Mathf.Sin(radiansAngle), 0.0f);

        transform.position += direction * movementRate;
    }
}
