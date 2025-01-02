using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DynamicTimerBar : MonoBehaviour
{
    public Slider timerSlider;         // Ÿ�̸� �ٸ� ǥ���� Slider
    public float timerDuration = 30f;   // Ÿ�̸��� �� ���� �ð�
    private float remainingTime;       // ���� ���� �ð�
    private bool isTimerRunning = false;
    GameManager gm;

    private float spendTime = 0.3f;
    public float setTime = 0;


    private void Start()
    {
        InitTimer(timerDuration);
        gm = GameManager.instance;
    }

    public void InitTimer(float duration)
    {
        timerDuration = duration;
        remainingTime = duration;
        timerSlider.maxValue = duration;
        timerSlider.value = duration;
        isTimerRunning = true;
        spendTime = 0;
        setTime = 0;
    }

    private void Update()
    {
        if (isTimerRunning && !gm.isFirstTouch)
        {
            setTime += Time.deltaTime;

            if (setTime >= 1)
            {
                setTime = 0;
                UpdateSpendTime(Time.deltaTime);
            }

            remainingTime -= (spendTime * Time.deltaTime);
            timerSlider.value = remainingTime;

            if (remainingTime <= 0)
            {
                isTimerRunning = false;
                gm.TimerEnded();
            }
        }
    }

    public void AddRemainingTime(float time)
    {
        remainingTime += time;
    }

    public void UpdateSpendTime(float time)
    {
        if(spendTime <= 1)
            spendTime += time; 
    }
}