using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBoard : MonoBehaviour
{
    [SerializeField] public CoinScore scoreCoin;
    [SerializeField] public CoinScore bestCoin;
    [SerializeField] public CoinScore getCoin;
    [SerializeField] public CoinImgPositioning coinImgPositioning;
    public static GameOverBoard instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        UpdateCoinResult();
    }
    public void UpdateCoinResult()
    {
        SoundManager.Instance.StopSound("BGM");

        int bestScore = PlayerPrefs.GetInt("bestScore", 0);
        int getScore = GameManager.instance.coin;

        if (bestScore < getScore)
        {
            bestScore = getScore;
            SoundManager.Instance.PlayEffectSound(4, "newRecord");

            PlayerPrefs.SetInt("bestScore", bestScore);
            PlayerPrefs.Save();
        }

        scoreCoin.UpdateCoinText(getScore);
        getCoin.UpdateCoinText(getScore);
        bestCoin.UpdateCoinText(bestScore);

        coinImgPositioning.UpdatePosition();
    }

    public void InitCoinScore()
    {
        scoreCoin.InitCoinImage();
        getCoin.InitCoinImage();    
        bestCoin.InitCoinImage();
        coinImgPositioning.UpdatePosition();
    }
}
