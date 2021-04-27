using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private bool Follow = false;
    public static int ID = 4;
    GameObject build;
    [SerializeField] private GameObject Grid;
    private PlayerHandler player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();

    }

    public void Test(Object obj)
    {
        if (player.Currency.Gold > 0)
        {
            build = Structures.NewStructure(obj);
            Follow = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (Follow)
        {

            Vector3 pos = Camera.main.ScreenToWorldPoint((Vector3)mouse.position.ReadValue() + (Vector3.forward * 10));
            build.transform.position = pos;
            var sr = build.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 1;
            var hover = false;
            Vector3Int vci = Vector3Int.FloorToInt(pos);
            if (GridManager.APMap.HasTile(vci))
            {
                hover = GridManager.APMap.GetTile(vci);
                sr.color = Color.white;
            }
            else
            {
                sr.color = Color.red;
            }
            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (hover)
                {

                    player.Currency.RemoveGold(10);
                    Structures.PlaceStructure(build, vci);
                    Follow = false;
                    build = null;
                }
            }

        }

    }
}
