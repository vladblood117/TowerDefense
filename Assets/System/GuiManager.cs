using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    // Start is called before the first frame update
    public interface IGui
    {
        public string GuiName { get; }
        void RefreshGui();
        void OpenGui();
    }



}
