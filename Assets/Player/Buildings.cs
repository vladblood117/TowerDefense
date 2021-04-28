using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    private bool _follow;
    private PlayerHandler _plr;
    private List<int> _allowedBuildings;
    [SerializeField] GameObject ContentGui;
    [SerializeField] GameObject BuildingButton;
    // Start is called before the first frame update
    void Start()
    {
        _follow = false;
        _plr = gameObject.GetComponent<PlayerHandler>();
        _allowedBuildings = new List<int>();
        _allowedBuildings.Add(1);
        _allowedBuildings.Add(2);
        RefreshGui();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RefreshGui()
    {
        var row = 1;
        var column = 1;
        var maxColumns = 3;
        var rowGap = .05f;
        var colGap = .1f;
        var baseX = -ContentGui.transform.localScale.x;
        var baseY = -ContentGui.transform.localScale.y;
        for (int i = 0; i < _allowedBuildings.Count; i++)
        {
            var v = Instantiate(BuildingButton);
            v.transform.SetParent(ContentGui.transform);
            v.transform.localPosition = new Vector3(
                ((baseX + (v.transform.localScale.x * 2) * column)) + colGap,
                ((baseY + (v.transform.localScale.y * 2) * row)) + rowGap,
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
