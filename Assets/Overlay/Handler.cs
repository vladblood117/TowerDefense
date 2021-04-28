using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    private GameObject BasicBuildingPanel;
    // Start is called before the first frame update 
    public static int ID = 5;
    public static Handler Overlay;
    void Start()
    {
        BasicBuildingPanel = gameObject.transform.GetChild(1).gameObject;
        Overlay = this;
    }

    public static void OpenBasicPanel()
    {

        Overlay.BasicBuildingPanel.SetActive(!Overlay.BasicBuildingPanel.activeSelf);
    }

    public static void CloseAllPanels()
    {
        Overlay.BasicBuildingPanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
