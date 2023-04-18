using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ReticleController.cs
//Justin Logan
//A00066979
//04-18-2023


public class ReticleController : MonoBehaviour
{
    public GameObject reticle;

    private GameObject reticlePrefab;

    void Awake()
    {
        reticlePrefab = Resources.Load<GameObject>("Prefabs/Reticle");
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        reticle = Instantiate(reticlePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        reticle.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0.0f);
    }
}
