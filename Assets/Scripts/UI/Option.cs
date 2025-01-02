using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField] GameObject optionOverlay;
    GameManager gm;
    public Camera mainCamera;
    public bool toggleOn = false;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void ClickOptionHandler()
    {
        gm.TogglePause();
        optionOverlay.gameObject.SetActive(true);
        toggleOn = true;
    }

    public void CancleOptionHandler()
    {
        gm.TogglePause();
        optionOverlay.gameObject.SetActive(false);
        toggleOn = false;
    }

    public void ToggleOption()
    {
        if (toggleOn)
        {
            CancleOptionHandler();
        }   else
        {
            ClickOptionHandler();
        }
    }
}
