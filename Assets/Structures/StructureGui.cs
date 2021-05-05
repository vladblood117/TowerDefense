using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StructureGui : TDSystem
{
    public static GameObject _structurePanel;
    public static RectTransform _panelRect;
    private static Button _closeBtn;
    private static Button _upgradeBtn;
    private static TMP_Text _strutName;
    private static TMP_Text _strutDesc;
    private static StructureController activeStructure;

    // Start is called before the first frame update
    void Start()
    {
        _structurePanel = transform.GetChild(0).gameObject;
        _panelRect = _structurePanel.GetComponent<RectTransform>();
        _closeBtn = WaitForChild(_structurePanel, 0).GetComponent<Button>();
        _upgradeBtn = WaitForChild(_structurePanel, 3).GetComponent<Button>();
        _strutName = WaitForChild(_structurePanel, 1).GetComponent<TMP_Text>();
        _strutDesc = WaitForChild(_structurePanel, 2).GetComponent<TMP_Text>();
        _closeBtn.onClick.AddListener(() => CloseActiveStructure());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void CloseActiveStructure()
    {
        _structurePanel.SetActive(false);
        if (activeStructure != null)
        {
            activeStructure.OnGuiClose();
            activeStructure = null;
        }
    }
    public static void OpenStructure(StructureController structure)
    {
        if (activeStructure != null)
        {
            CloseActiveStructure();
        }

        activeStructure = structure;
        RefreshGui();
        _structurePanel.SetActive(true);
    }

    public static void RefreshGui()
    {
        if (activeStructure)
        {
            _strutName.SetText(activeStructure.GetStructure().StructureName);
            _strutDesc.SetText(activeStructure.GetStructure().Description);
            _upgradeBtn.transform.GetChild(0).GetComponent<TMP_Text>().SetText("Cannot Upgrade");
            var point = Camera.main.WorldToScreenPoint(activeStructure.GetStructure().transform.position);
            var x = 0f;
            var y = 0f;
            if (point.y >= .7f)
            {
                y = _panelRect.rect.height / 2;
            }
            else
            {
                y = -_panelRect.rect.height / 2;
            }

            _panelRect.position = point + new Vector3(x, y, 0);
        }
    }

}
