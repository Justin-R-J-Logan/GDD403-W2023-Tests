using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Follow.cs
//Justin Logan
//A00066979
//04-18-2023


public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
