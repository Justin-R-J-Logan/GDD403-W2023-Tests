using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyController.cs
//Justin Logan
//A00066979
//04-18-2023


public class EnemyController : MonoBehaviour
{
    [Header("Movement Properties")] 
    [Range(0.001f, 0.1f)]
    public float speedValue = 0.05f;
    public float speedDistanceModifier = 0.0f;
    public bool timerIsActive;
    public bool isLooping;
    public bool isReverse;
    private Vector3 direction;
    private float lookAngle;

    [Header("Path Points")] 
    public List<PathNode> pathNodes;

    [Header("Health System")] 
    public HealthSystem health;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private PathNode currentNode;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        timerIsActive = true;
        isLooping = true;
        isReverse = false;
        direction = Vector3.right;

        startPoint = transform.position;
        BuildPathNodes();
    }

    private void BuildPathNodes()
    {
        // create an empty List Container
        pathNodes = new List<PathNode>();

        // add all PathNodes to the PathNodes List
        foreach (Transform child in transform)
        {
            if (!child.gameObject.CompareTag("Turret"))
            {
                var pathNode = new PathNode(child.position, null, null);
                pathNodes.Add(pathNode);
            }
        }

        // set up all links
        for (var i = 0; i < pathNodes.Count; i++)
        {
            pathNodes[i].next = (i == pathNodes.Count - 1) ? pathNodes[0] : pathNodes[i + 1];
            pathNodes[i].prev = (i == 0) ? pathNodes[^1] : pathNodes[i - 1];
        }

        currentNode = pathNodes[0];

        endPoint = currentNode.position;

        speedDistanceModifier = speedValue / Vector2.Distance(startPoint, endPoint);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
    }

    void FixedUpdate()
    {
        Move();
        UpdateTimer();
    }

    /// <summary>
    /// Updates the timer.
    /// Used for Movement and Turning.
    /// </summary>
    private void UpdateTimer()
    {
        if (timerIsActive)
        {
            // increment timer
            if (timer <= 1.0f)
            {
                timer += speedDistanceModifier;
            }

            // resets timer
            if (timer >= 1.0f)
            {
                timer = 0.0f;

                Traverse((isReverse) ? 0 : ^1, (isReverse) ? currentNode.prev : currentNode.next);
            }
        }
    }

    /// <summary>
    ///  This method traverses from one pathNode to the next depending on the direction of the motion (forward or reverse)
    /// </summary>
    /// <param name="boundaryIndex"></param>
    /// <param name="nextNode"></param>
    private void Traverse(System.Index boundaryIndex, PathNode nextNode)
    {
        startPoint = currentNode.position;
        endPoint = nextNode.position;

        speedDistanceModifier = speedValue / Vector2.Distance(startPoint, endPoint);

        if (currentNode != pathNodes[boundaryIndex])
        {
            currentNode = nextNode;
        }
        else if ((currentNode == pathNodes[boundaryIndex]) && (isLooping))
        {
            currentNode = nextNode;
        }
        else
        {
            timerIsActive = false;
        }
    }

    /// <summary>
    /// This moves the tank from currentNode to nextNode (or previousNode).
    /// The direction depends on the isReverse variable
    /// </summary>
    private void Move()
    {
        transform.position = Vector2.Lerp(startPoint, endPoint, timer);
    }

    /// <summary>
    /// This changes the direction that the Tank is "looking"
    /// </summary>
    private void ChangeDirection()
    {
        direction = Vector3.Normalize(endPoint - startPoint);
        lookAngle = (Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg) - 90.0f;
        var targetAngle = Mathf.LerpAngle(transform.eulerAngles.z, lookAngle, timer);
        transform.eulerAngles = new Vector3(0.0f, 0.0f, targetAngle);
    }
}
