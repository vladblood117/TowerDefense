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
    public static int currentScan = 1;
    public static Dictionary<int, GameObject> collection = new Dictionary<int, GameObject>();

    private BoxCollider2D _collider;
    private static bool destructionDebounce;

    void Start()
    {
        destructionDebounce = false;
        _collider = this.gameObject.GetComponent<BoxCollider2D>();
    }
    public static GameObject NewStructure(Object strut)
    {
        GameObject obj = (GameObject)Instantiate(strut) as GameObject;
        collection[collection.Count + 1] = obj;
        return obj;
    }

    public static void PlaceStructure(GameObject obj, Vector3Int vci)
    {
        obj.transform.position = (Vector3)vci + new Vector3(0.5f, 0.5f, 0f);
        GridManager.APMap.SetTile(vci, null);
        GridManager.ObMap.SetTile(vci, (Tile)Resources.Load("Tilemap/TDBlank"));
        Structures strut = obj.GetComponent<Structures>();
        strut.StructurePlaced();

        AstarPath.active.Scan();
        currentScan++;
    }
    //---------Dynamic Functions
    public void StructurePlaced()
    {

        this._collider.enabled = true;
    }

    public static void MobDestruction()
    {
        if (!destructionDebounce)
        {
            destructionDebounce = true;

            print(collection.Count);

            var r = Random.Range(1, collection.Count);
            print(r);
            var strut = collection[r];
            collection.Remove(r);
            var vci = Vector3Int.FloorToInt(strut.transform.position);
            GridManager.APMap.SetTile(vci, (Tile)Resources.Load("Tilemap/TDBlank"));
            GridManager.ObMap.SetTile(vci, null);
            AstarPath.active.Scan();
            Destroy(strut);
            destructionDebounce = false;
        }
    }

}
