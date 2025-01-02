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
    [SerializeField] CoinScore coinScore;
    [SerializeField] Title title;
    [SerializeField] public GameObject endOverlay;
    [SerializeField] public TextMeshProUGUI resultCoinText;
    [SerializeField] public GameOverBoard gameOverBoard;
    [SerializeField] SoundManager soundManager;
    public AdManager adManager;

    public int coin;
    public int bestCoinRecord;
    public int comboCount = 0;

    public bool isPaused = false; 
    public bool isGameStart = false;

    public bool isVibrationMode = true;
    public bool isFirstTouch = true;

    public bool isGameOver = false;

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
        SoundManager.Instance.PlayBackGroundSound(0);
    }

    public void BeforeGameStart()
    {
        
    }

    public void GameStart()
    {
        
    }

    public IEnumerator InitGameStart()
    {
        yield return new WaitForEndOfFrame();

        ResetGame();
    }

    public void ResetGame()
    {
        title.gameObject.SetActive(false);
        endOverlay.SetActive(false);
        SetInGameUI(true);

        for (int i = 0; i < customerManger.CustomerList.Count; i++)
        {
            Destroy(customerManger.CustomerList[i].gameObject);
        }

        customerManger.CustomerList = new List<Customer>();

        for (int i = 0; i < waitSpots.Count; i++)
        {
            customerManger.AllCustomerShowSpeechBubble(customerManger.SpawnCustomer(i), true);
            customerManger.SetOrderLayer(i);
        }

        customerManger.UpdateOrderCustomer();

        isGameStart = true;
        isFirstTouch = true;
        isGameOver = false;
        coin = 0;
        comboCount = 0;

        timerbar.InitTimer(timerbar.timerDuration);
        coinScore.InitCoinImage();
        gameOverBoard.InitCoinScore();


        soundManager.InitSound();
        soundManager.PlayUiSound(0);
        soundManager.PlayBackGroundSound(1);
    }

    public void SetInGameUI(bool active)
    {
        timerbar.gameObject.SetActive(active);
        coinScore.gameObject.SetActive(active);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        SoundManager.Instance.PlayUiSound(0);
    }

    public void HandleRequestFail()
    {
        timerbar.AddRemainingTime(-3f);
        ResetCombo();

        if(isVibrationMode)
            Handheld.Vibrate();

        customerManger.SpawnCustomer(19);
    }

    public void HandleRequestSuccess()
    {
        timerbar.AddRemainingTime(0.5f);
        AddCombo();
        customerManger.SpawnCustomer(19);
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

    public void SetVibrationMode()
    {
        isVibrationMode = !isVibrationMode;
    }

    public void Addcoin(int coin)
    {
        this.coin += coin;

        coinScore.UpdateCoinText(this.coin);
        StartCoroutine(Shake(0.2f, 0.1f));
    }

    void GenerateWaitSpot()
    {
        WaitSpot newWaitSpot = PrefabFactory.Instance.Create(waitSpotPrefab,transform.position);
        waitSpots.Add(newWaitSpot);
    }

    public void TimerEnded()
    {
        endOverlay.SetActive(true);
        SetInGameUI(false);
        isGameStart = false;
        isGameOver = true;

        for (int i = 0; i < customerManger.CustomerList.Count; i++)
        {
            customerManger.AllCustomerShowSpeechBubble(customerManger.CustomerList[i], false);
        }

        gameOverBoard.UpdateCoinResult();

        StartCoroutine(adManager.ShowInterstitialAd());
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
                worldCenter.x,        // x ��ǥ ����
                worldCenter.y + yOffset, // y ��ǥ�� yOffset �߰�
                0                     // z ��ǥ
            );
        }
    }
}


