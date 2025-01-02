using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DynamicTimerBar : MonoBehaviour
{
    public Slider timerSlider;         // 타이머 바를 표시할 Slider
    public float timerDuration = 30f;   // 타이머의 총 지속 시간
    private float remainingTime;       // 현재 남은 시간
    private bool isTimerRunning = false;

    private void Start()
    {
        StartTimer(timerDuration);
    }

    public void StartTimer(float duration)
    {
        timerDuration = duration;
        remainingTime = duration;
        timerSlider.maxValue = duration;
        timerSlider.value = duration;
        isTimerRunning = true;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            remainingTime -= Time.deltaTime;
            timerSlider.value = remainingTime;

            if (remainingTime <= 0)
            {
                isTimerRunning = false;
                TimerEnded();
            }
        }
    }

    public void AddRemainingTime(float time)
    {
        remainingTime += time;
    }

    private void TimerEnded()
    {
        SceneManager.LoadScene("EndScene"); // "MainScene"은 메인 씬 이름으로 변경하세요
    }
}