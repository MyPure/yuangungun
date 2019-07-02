using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public GameObject levelPanel;

    public void ClassicBtn()
    {
        levelPanel.SetActive(true);
    }
}
