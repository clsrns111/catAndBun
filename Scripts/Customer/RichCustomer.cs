using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichCustomer : Customer
{

    public override void GenerateRandomRequire()
    {
        int randomRedBin = UnityEngine.Random.Range(0, 10);
        int randomCream = UnityEngine.Random.Range(0, 10);

        if (randomRedBin + randomCream == 0)
        {
            if (UnityEngine.Random.value < 0.5f)
                randomRedBin = UnityEngine.Random.Range(5, 10);
            else
                randomCream = UnityEngine.Random.Range(5, 10);
        }

        SetRequire(randomRedBin, randomCream);
    }

    protected override void HandleCoinGenerate()
    {
        for(int i =0; i< 5; i++)
        {
            Coin newCoint = PrefabFactory.Instance.Create(coin, transform.position);
            StartCoroutine(newCoint.PopOutEffect());
        }

        gm.Addcoin(5);
    }

    public override void ApplyRandomSprite()
    {
     
    }

}
