using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DynamicTimerBar : MonoBehaviour
{
    public Slider timerSlider;         // Ÿ�̸� �ٸ� ǥ���� Slider
    public float timerDuration = 30f;   // Ÿ�̸��� �� ���� �ð�
    private float remainingTime;       // ���� ���� �ð�
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
        SceneManager.LoadScene("EndScene"); // "MainScene"�� ���� �� �̸����� �����ϼ���
    }
}