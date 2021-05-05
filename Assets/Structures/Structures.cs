using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class Structures : TDSystem
{
    // Start is called before the first frame update 

    [SerializeField] public int SID;
    [SerializeField] public string StructureName;
    [SerializeField] public string Description;
    [SerializeField] public int GoldCost;
    [SerializeField] public bool CanAttack;
    //IF CanAttack
    [SerializeField] public int MaxDamage;
    [SerializeField] public int MinDamage;
    [SerializeField] public float Range;
    [SerializeField] public int AttackSpeed;
    [SerializeField] public GameObject Shoots;
    //ENDIF
    public static int currentScan = 1;
    public static Dictionary<int, Structures> collection = new Dictionary<int, Structures>();
    public static Dictionary<Vector3Int, int> collectionMap = new Dictionary<Vector3Int, int>();
    private HealthHandler _health;
    private int strutId;
    private bool placed = false;
    private static int StructureIndex = 0;
    public HealthHandler.OnDeath _method;
    private BoxCollider2D _collider;
    private bool debounce;
    private float delta;
    private GameObject _enemy;
    private Material _defaultMaterial;
    public Material DefaultMaterial { get { return _defaultMaterial; } }

    public bool IsPlaced { get { return placed; } }
    void Start()
    {
        debounce = false;
        delta = 0f;
        _health = gameObject.GetComponent<HealthHandler>();
        _method = DeathMethod;
        _health.RegisterDeathMethod(_method);
        _collider = this.gameObject.GetComponent<BoxCollider2D>();

    }

    IEnumerator RescanGraph()
    {
        new WaitForSeconds(.5f);
        AstarPath.active.Scan();
        currentScan++;
        yield return new WaitForSeconds(.5f);
    }
    private void DeathMethod()
    {

        var vci = Vector3Int.FloorToInt(transform.position);
        GridManager.APMap.SetTile(vci, (Tile)Resources.Load("Tilemap/TDBlank"));
        //  GridManager.ObMap.SetTile(vci, null);
        collection.Remove(strutId);
        Destroy(gameObject);
        StartCoroutine(RescanGraph());

    }

    public static Structures NewStructure(Structures strut)
    {
        GameObject obj = (GameObject)Instantiate(strut.gameObject) as GameObject;

        var s = obj.GetComponent<Structures>();
        return s;
    }
    public static GameObject GetNearest(Vector3 Position)
    {
        GameObject obj = null;
        var distance = 0f;
        foreach (var v in collection.Values)
        {
            if (v != null && v.placed)
            {

                if (obj != null)
                {
                    var magA = (v.gameObject.transform.position - Position).magnitude;
                    var magB = (obj.transform.position - Position).magnitude;
                    if (magA < magB)
                    {
                        obj = v.gameObject;
                    }
                }
                var mag = (v.gameObject.transform.position - Position).magnitude;

                if (mag <= 1.5f)
                {

                    obj = v.gameObject;
                    distance = mag;
                }
            }
        }

        return obj;
    }

    //---------Dynamic Functions


    public void PlaceStructure(Vector3Int vci)
    {
        var clone = Instantiate(this.gameObject);
        clone.transform.position = (Vector3)vci + new Vector3(0.5f, 0.5f, 0f);
        GridManager.APMap.SetTile(vci, null);
        //  GridManager.ObMap.SetTile(vci, (Tile)Resources.Load("Tilemap/TDBlank"));
        var cthis = clone.GetComponent<Structures>();

        if (cthis != null)
        {
            var indx = StructureIndex + 1;
            StructureIndex++;
            cthis.strutId = indx;
            collection[indx] = cthis;
            collectionMap[vci] = indx;
            cthis.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            cthis.placed = true;
            cthis._defaultMaterial = cthis.gameObject.GetComponent<SpriteRenderer>().material;
            if (cthis._defaultMaterial == null)
            {
                Debug.LogWarning("No material");
            }
            else
            {
                Debug.Log("Material set for default");
            }
            StartCoroutine(RescanGraph());
        }
        else
        {
            Debug.Log("Not found!");
        }
    }

    void Update()
    {
        if (CanAttack && placed)
        {

            if (!debounce)
            {
                debounce = true;
                if (delta >= AttackSpeed)
                {
                    delta = 0f;
                    if (!_enemy)
                    {
                        _enemy = getEnemy();
                    }
                    if (InRange(_enemy))
                    {
                        Attack();
                    }
                    else
                    {
                        _enemy = getEnemy();
                        if (_enemy)
                        {
                            Attack();
                        }

                    }
                }
                else
                {
                    delta += Time.deltaTime;
                }
                debounce = false;
            }
        }
    }

    private bool InRange(GameObject _enemy)
    {
        bool val = false;
        if (_enemy != null)
        {
            if ((_enemy.transform.position - transform.position).magnitude <= Range)
            {
                val = true;
            }
        }
        return val;
    }

    private void Attack()
    {
        var shoot = Instantiate(Shoots).GetComponent<Projectiles>();
        shoot.transform.position = transform.position;
        shoot.SetTarget(_enemy);
        shoot.SetDamage(Random.Range(MinDamage, MaxDamage));
        shoot.Fire();
    }

    private GameObject getEnemy()
    {
        GameObject enemy = null;
        foreach (var v in MobHandler.Creatures.Values)
        {
            if (v)
            {
                var mag = (v.transform.position - transform.position).magnitude;
                if (mag <= Range)
                {
                    enemy = v;

                    break;
                }
            }
        }
        return enemy;
    }

}
