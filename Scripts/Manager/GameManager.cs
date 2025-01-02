using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum BunType
{
    cream,  
    redBin,
    Jam,
    both
}

public class GameManager : MonoBehaviour
{
    static public GameManager instance;    

    public PlayerController playerController;
    public CustomerManger customerManger;
    public List<WaitSpot> waitSpots;
    public WaitSpot waitSpotPrefab;
    public float waitSpacing = 0.3f;

    public bool feverMode = false;
    float statingPos = 3.3f;

    [SerializeField] public DynamicTimerBar timerbar;
    [SerializeField] public UnityEngine.UI.Slider feverSlider;
    [SerializeField] CoinImg coinImg;
    [SerializeField] CoinScore coinScore;
    
    public int coin;
    public TextMeshProUGUI coinText;
    public int comboCount = 0;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
            instance = this;    
    }
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        customerManger = GetComponent<CustomerManger>();

        waitSpots = new List<WaitSpot>();

        for(int i =0; i< 20; i++)
        {
            GenerateWaitSpot();
        }

        LineUpWaitSpot();
    }

    void Update()
    {
        
    }

    public void AddCombo()
    {
        comboCount++;
        int stepValue = Mathf.RoundToInt(comboCount);
        feverSlider.value = stepValue;
    }

    public void ResetCombo()
    {
        comboCount = 0;
        int stepValue = Mathf.RoundToInt(comboCount);
        feverSlider.value = stepValue;
    }

  

    public void Addcoin(int coin)
    {
        this.coin += coin;

        coinScore.UpdateCoinText(this.coin);
        StartCoroutine(coinImg.PopCoinImg());
        StartCoroutine(Shake(0.2f, 0.1f));
    }

    void GenerateWaitSpot()
    {
        WaitSpot newWaitSpot = PrefabFactory.Instance.Create(waitSpotPrefab,transform.position);
        waitSpots.Add(newWaitSpot);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position after shake
        transform.localPosition = originalPosition;
    }

    void LineUpWaitSpot()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2,  0, 0f);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

        for (int i = 0; i< waitSpots.Count; i++)
        {
            float objectHeight = waitSpots[i].GetComponent<SpriteRenderer>().bounds.size.y;

            float yOffset = statingPos + i * (objectHeight + waitSpacing);

            waitSpots[i].transform.position = new Vector3(
                worldCenter.x,        // x ÁÂÇ¥ °íÁ¤
                worldCenter.y + yOffset, // y ÁÂÇ¥¿¡ yOffset Ãß°¡
                0                     // z ÁÂÇ¥
            );
        }
    }
}


