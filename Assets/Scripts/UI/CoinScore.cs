using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image[] numberImageList;
    [SerializeField] float xOffset = -28 - 2.5f;
    [SerializeField] float initPosX = 0;
    [SerializeField] float initPosY = 0;


    void Start()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
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

            if (digit < 0)
                digit = 0;

            numberImageList[i].sprite = sprites[digit];
            ActiveNumberImage(numberImageList[i], true);

            if (i > 0)
            {
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

                Vector2 newPos = new Vector2(-xOffset * i, initPosY);
                rectTransform.anchoredPosition = newPos;
            }
        }
    }

    public void ActiveNumberImage(Image image, bool active) 
    {
        image.gameObject.SetActive(active);
    }

    public void SetNumberImage(Image image, int number)
    {
        image.sprite = sprites[number];
    }

    public void InitCoinImage()
    {
        UpdateCoinText(0);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 newPos = new Vector2(initPosX, initPosY);
        rectTransform.anchoredPosition = newPos;

        for (int i=1; i<3; i++)
        {
            ActiveNumberImage(numberImageList[i], false);
        }

    }
}
