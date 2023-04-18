using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

//GraphNode.cs
//Justin Logan
//A00066979
//04-18-2023

public class GraphNode : MonoBehaviour
{
    public List<GraphNode> neighbours;
    public bool isColliding;
    public Collider2D collider2D;
    public Transform target;
    public float targetDistance;

    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        target = FindObjectOfType<EnemyController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector3.Distance(target.position, transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (isColliding) ? Color.green : Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.0f, 1.0f));
    }

    private GraphNode SelectBestNeighbour()
    {
        GraphNode best = null;
        
        //Loop through all the nodes as neighbours
        foreach(GraphNode node in neighbours)
        {
            //If best is still null, automagically set the first one
            if(best == null)
            {
                best = node;
            }
            //Check distance from target to the nodes, pick the best node.
            if (Vector2.Distance(target.transform.position, best.transform.position) >
                Vector2.Distance(target.transform.position, node.transform.position))
            {
                best = node;
            }
        }

        return best;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Bullet":
            {
                var bullet = other.gameObject.GetComponent<BulletController>();
                if (bullet.type == BulletType.SEEKING)
                {
                    isColliding = true;
                    bullet.direction = Vector3.Normalize(SelectBestNeighbour().transform.position - transform.position);
                }
            }
                break;
        }

        collider2D = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Bullet":
            {
                var bullet = other.gameObject.GetComponent<BulletController>();
                if (bullet.type == BulletType.SEEKING)
                {
                    isColliding = false;
                }
            }
                break;
        }

        collider2D = null;
    }


}
