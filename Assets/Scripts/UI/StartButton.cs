using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnClick()
    {
        StartCoroutine(GameManager.instance.InitGameStart());
    }

    private void OnMouseDown()
    {
        OnClick();
    }
}
