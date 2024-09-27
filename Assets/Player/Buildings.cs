using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Buildings : MonoBehaviour
{
    private bool _follow;
    private PlayerHandler _plr;
    private List<int> _allowedBuildings;
    [SerializeField] GameObject ContentGui;
    private RectTransform contentRT;
    private Structures _build;
    [SerializeField] GameObject BuildingButton;
    public bool Follow { get { return _follow; } }
    // Start is called before the first frame update
    void Start()
    {
        _follow = false;
        _plr = gameObject.GetComponent<PlayerHandler>();
        _allowedBuildings = new List<int>();
        _allowedBuildings.Add(1);
        _allowedBuildings.Add(2);
        contentRT = ContentGui.GetComponent<RectTransform>();
        RefreshGui();
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (_follow)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint((Vector3)mouse.position.ReadValue() + (Vector3.forward * 10));
            var sr = _build.gameObject.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 5;
            var hover = false;
            Vector3Int vci = Vector3Int.FloorToInt(pos);
            _build.gameObject.transform.position = vci + new Vector3(0.5f, 0.5f, 0f);

            if (GridManager.APMap.HasTile(vci))
            {
                hover = GridManager.APMap.GetTile(vci);
                sr.color = Color.white;
            }
            else if (Structures.collectionMap.ContainsKey(vci))
            {
                sr.color = Color.red;
            }
            else
            {
                sr.color = Color.red;
            }
            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (_plr.Currency.Gold < _build.GoldCost)
                {
                    sr.color = Color.red;
                }
                else
                {

                    sr.color = Color.white;
                    if (hover)
                    {

                        _plr.Currency.RemoveGold(_build.GoldCost);
                        _build.PlaceStructure(_plr, vci);
                    }
                }
            }
            else if (mouse.rightButton.wasPressedThisFrame)
            {
                Destroy(_build.gameObject);
                _build = null;
                _follow = false;
            }

        }


    }
    public void Test(Structures obj)
    {
        if (_plr.Currency.Gold >= obj.GoldCost)
        {
            _build = Structures.NewStructure(obj);
            _follow = true;
            Handler.CloseAllPanels();
        }
    }

    private void RefreshGui()
    {
        var row = 1;
        var column = 1;
        var maxColumns = 3;
        var rowGap = 15f;
        var colGap = 15f;
        for (int i = 0; i < _allowedBuildings.Count; i++)
        {
            var v = Instantiate(BuildingButton);
            var btn = v.transform.GetChild(0).GetComponent<Button>();

            var builder = AllBuildings.Buildings[_allowedBuildings[i]];
            var img = btn.GetComponent<Image>();
            img.sprite = builder.gameObject.GetComponent<SpriteRenderer>().sprite;
            btn.onClick.AddListener(() => Test(builder));
            v.transform.SetParent(ContentGui.transform);
            v.transform.localScale = new Vector3(1, 1, 1);
            var rectV = v.GetComponent<RectTransform>();
            rectV.anchoredPosition = new Vector3(
               (rectV.rect.width * (column - 1) + (colGap * (column - 1))),
                (rectV.rect.height * (row - 1) + (rowGap * (row - 1))),
                0
            );
            column++;
            if (column >= maxColumns)
            {
                row++;
                column = 1;
            }
        }
    }

}
