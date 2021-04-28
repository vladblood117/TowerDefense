using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Structures : MonoBehaviour
{
    // Start is called before the first frame update
    public static int ID = 3;
    [SerializeField] public int SID;
    [SerializeField] public string StructureName;
    [SerializeField] public string Description;
    [SerializeField] public int GoldCost;
    [SerializeField] public bool CanAttack;
    //IF CanAttack
    [SerializeField] public int Damage;
    [SerializeField] public float Range;
    [SerializeField] public int AttackSpeed;
    //ENDIF
    public static int currentScan = 1;
    public static Dictionary<int, GameObject> collection = new Dictionary<int, GameObject>();
    private HealthHandler _health;
    private int strutId;
    private bool placed = false;
    private static int StructureIndex = 0;
    public HealthHandler.OnDeath _method;
    private BoxCollider2D _collider;
    private bool debounce;
    private float delta;
    private GameObject _enemy;

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
        //  GridManager.APMap.SetTile(vci, (Tile)Resources.Load("Tilemap/TDBlank"));
        //  GridManager.ObMap.SetTile(vci, null);
        collection.Remove(strutId);
        Destroy(gameObject);
        StartCoroutine(RescanGraph());

    }

    public static Structures NewStructure(Structures strut)
    {
        GameObject obj = (GameObject)Instantiate(strut.gameObject) as GameObject;
        var indx = StructureIndex + 1;
        StructureIndex++;
        collection[indx] = obj;
        var s = obj.GetComponent<Structures>();
        s.strutId = indx;
        return s;
    }
    public static GameObject GetNearest(Vector3 Position)
    {
        GameObject obj = null;
        foreach (var v in collection.Values)
        {
            if ((v.transform.position - Position).magnitude <= 1.5f)
            {
                obj = v;
                break;
            }

        }

        return obj;
    }

    public void PlaceStructure(Vector3Int vci)
    {
        transform.position = (Vector3)vci + new Vector3(0.5f, 0.5f, 0f);
        //  GridManager.APMap.SetTile(vci, null);
        //  GridManager.ObMap.SetTile(vci, (Tile)Resources.Load("Tilemap/TDBlank"));
        StructurePlaced();
    }
    //---------Dynamic Functions
    public void StructurePlaced()
    {

        this._collider.enabled = true;
        placed = true;
        StartCoroutine(RescanGraph());
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
                    print(delta);
                    delta = 0f;
                    if (!_enemy)
                    {
                        Debug.Log("Set enemy");
                        _enemy = getEnemy();
                    }
                    if (InRange(_enemy))
                    {
                        Debug.Log("Enemy in range, attack!");
                        Attack(_enemy);
                    }
                    else
                    {
                        Debug.Log("They left range, find a new one and... attack!");
                        _enemy = getEnemy();
                        if (_enemy)
                        {
                            Attack(_enemy);
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

    private void Attack(GameObject _enemy)
    {
        HealthHandler h = _enemy.GetComponent<HealthHandler>();
        h.TakeDamage(Damage);
        Debug.Log("pew pew!");
        if (!_enemy)
        {
            Debug.Log("Enemy is dead");
        }
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
                    if (mag <= 3f)
                    {
                        break;
                    }
                }
            }
        }
        return enemy;
    }

}
