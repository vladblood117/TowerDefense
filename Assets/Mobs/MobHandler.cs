using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MobHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static int ID = 8;
    [SerializeField] public string MobName;
    [SerializeField] public int MoveSpeed;
    [SerializeField] public int Damage;

    private Rigidbody2D body;
    private BoxCollider2D _collider;

    private GameObject killObject;
    private Seeker _seeker;

    //The AI's speed per second
    public float speed = 100;
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    public float repathRate = 0.5f;
    private float lastRepath = -9999;
    private float delta = 0f;
    private Path path;
    private int scanIndex = 0;
    private int direction;
    void Start()
    {
        killObject = GameObject.FindGameObjectWithTag("Defend");
        body = gameObject.GetComponent<Rigidbody2D>();
        _seeker = gameObject.GetComponent<Seeker>();
        // _seeker.pathCallback = OnPathComplete;
        // AIDestinationSetter aids = GetComponent<AIDestinationSetter>();
        //  aids.target = killObject.transform;
        scanIndex = Structures.currentScan;
        print(killObject.transform.position);
        direction = 0;
        _seeker.StartPath(transform.position, killObject.transform.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
            Debug.Log("Oh noes, the target was not reachable: " + p.errorLog);
        }

        // Update is called once per frame

    }

    public void Update()
    {
        if (Structures.currentScan > scanIndex)
        {
            print("Rescan processed, update!");
            _seeker.StartPath(transform.position, killObject.transform.position, OnPathComplete);
            scanIndex = Structures.currentScan;
        }
        delta += Time.deltaTime;
        if (delta >= 1f)
        {
            delta = 0f;
            GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
            GraphNode node2 = AstarPath.active.GetNearest(killObject.transform.position, NNConstraint.Default).node;
            if (!PathUtilities.IsPathPossible(node1, node2))
            {
                Structures.MobDestruction();
                print("Path not possible");
            }
            else
            {
                print("Path possible?");
            }
        }
    }
}
