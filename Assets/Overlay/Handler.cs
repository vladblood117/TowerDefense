using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject BasicBuildingPanel;
    void Start()
    {

    }

    public void OpenBasicPanel()
    {

        BasicBuildingPanel.SetActive(!BasicBuildingPanel.activeSelf);
    }

    public void CloseAllPanels()
    {
        BasicBuildingPanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
