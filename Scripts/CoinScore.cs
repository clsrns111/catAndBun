using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image[] numberImageList;

    void Start()
    {
     
    }

    public int GetDigit(int number, int posNumber)
    {
        int digitCount = (int)Mathf.Floor(Mathf.Log10(number)) + 1;
        int digitNumber = (int)(number / Mathf.Pow(10, digitCount - posNumber)) % 10;

        return digitNumber;
    }

    public void UpdateCoinText(int coinNumber)
    {
        int coinNumberLength = coinNumber.ToString().Length;    

        for(int i = 0; i < coinNumberLength; i++)
        {
            int digit = GetDigit(coinNumber, i + 1);

            numberImageList[i].sprite = sprites[digit];
            ActiveNumberImage(numberImageList[i]);
        }
    }

    public void ActiveNumberImage(Image image) 
    {
        image.gameObject.SetActive(true);
    }

    public void SetNumberImage(Image image, int number)
    {
        image.sprite = sprites[number];
    }

    void Update()
    {
        
    }
}
