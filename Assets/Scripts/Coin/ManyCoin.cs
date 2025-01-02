using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManyCoin : Coin
{
    void Start()
    {
        SoundManager.Instance.PlayEffectSound(1, "coin_m");
    }
}
