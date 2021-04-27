using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public GameObject _apGrid;
    [SerializeField] public GameObject _obGrid;

    private static Tilemap _apMap;
    private static Tilemap _obMap;
    public static Tilemap APMap { get { return _apMap; } }
    public static Tilemap ObMap { get { return _obMap; } }
    // Start is called before the first frame update
    void Start()
    {
        _apMap = _apGrid.GetComponent<Tilemap>();
        _obMap = _obGrid.GetComponent<Tilemap>();
        for (int i = -_obMap.size.x; i < _obMap.size.x; i++)
        {
            for (int z = -_obMap.size.y; z < _obMap.size.y; z++)
            {
                if (_obMap.HasTile(new Vector3Int(i, z, 0)))
                {
                    _apMap.SetTile(new Vector3Int(i, z, 0), null);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
