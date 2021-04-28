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

    public static Dictionary<int, GameObject> Creatures = new Dictionary<int, GameObject>();
    public static int CreatureId = 0;

    //The AI's speed per second
    public float speed = 100;
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    public HealthHandler.OnDeath _method;
    public float repathRate = 0.5f;
    private float lastRepath = -9999;
    private float delta = 0f;
    private int status;
    //0 = pathing, 1 = attacking
    private Path path;
    private int scanIndex = 0;
    private int direction;
    private GameObject targetAttack;
    private HealthHandler _health;
    private bool debounce = false;
    private int myCID;
    void Start()
    {
        status = 0;
        myCID = CreatureId + 1;
        CreatureId++;
        Creatures[myCID] = gameObject;
        _health = gameObject.GetComponent<HealthHandler>();
        _method = DeathMethod;
        _health.RegisterDeathMethod(_method);
        killObject = GameObject.FindGameObjectWithTag("Defend");
        body = gameObject.GetComponent<Rigidbody2D>();
        _seeker = gameObject.GetComponent<Seeker>();
        scanIndex = Structures.currentScan;
        direction = 0;
        _seeker.StartPath(transform.position, killObject.transform.position, OnPathComplete);
    }
    private void DeathMethod()
    {

        Creatures.Remove(myCID);
        Destroy(gameObject);

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
            _seeker.StartPath(transform.position, killObject.transform.position, OnPathComplete);
            scanIndex = Structures.currentScan;
            targetAttack = null;
            status = 0;
        }
        delta += Time.deltaTime;
        switch (status)
        {
            case 0:
                if (delta >= 1f)
                {
                    delta = 0f;
                    GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
                    GraphNode node2 = AstarPath.active.GetNearest(killObject.transform.position, NNConstraint.Default).node;
                    if (!PathUtilities.IsPathPossible(node1, node2))
                    {
                        status = 1;
                        delta = 0f;
                    }
                }

                break;
            case 1:
                if (!debounce)
                {
                    debounce = true;
                    if (body.velocity == Vector2.zero)
                    {
                        if (targetAttack == null)
                        {
                            targetAttack = Structures.GetNearest(transform.position);
                            delta = 0f;

                        }
                        if (targetAttack != null && delta >= 1f)
                        {
                            HealthHandler _health = targetAttack.GetComponent<HealthHandler>();
                            _health.TakeDamage(Damage);
                            delta = 0f;
                        }

                    }
                    debounce = false;
                }
                break;

        }
    }
}
