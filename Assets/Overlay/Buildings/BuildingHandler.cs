using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private bool Follow = false;
    GameObject build;
    void Start()
    {
    }

    public void Test(Object obj)
    {
        build = Instantiate(obj as GameObject);
        Follow = true;
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (Follow)
        {
            build.transform.position = Camera.main.ScreenToWorldPoint((Vector3)mouse.position.ReadValue() + (Vector3.forward * 10));
            if (mouse.leftButton.wasPressedThisFrame)
            {
                Follow = false;
                build = null;
            }
        }

    }
}
