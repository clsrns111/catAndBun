using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    private List<AudioSource> availableSources = new List<AudioSource>();
    private List<AudioSource> usedSources = new List<AudioSource>();
    private int poolSize = 100;

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            availableSources.Add(source);
        }
    }

    public AudioSource GetAudioSource()
    {
        AudioSource audioSource;

        if (availableSources.Count > 0)
        {
            audioSource = availableSources[0];
            availableSources.RemoveAt(0);
        }
        else
        {
            // 풀에 여유가 없으면 새로운 AudioSource 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 사용되기 전에 초기화
        InitializeAudioSource(audioSource);

        usedSources.Add(audioSource);
        return audioSource;
    }

    public void ReturnAudioSource(AudioSource source)
    {
        if (usedSources.Contains(source))
        {
            usedSources.Remove(source);
            availableSources.Add(source);
        }

        // 반환될 때 초기화
        InitializeAudioSource(source);
    }

    private void InitializeAudioSource(AudioSource source)
    {
        source.Stop(); // 이전에 재생된 오디오를 멈춤
        source.clip = null; // 이전 클립 초기화
        source.loop = false; // 반복 설정 초기화
        source.volume = 1f; // 기본 볼륨 설정
        source.pitch = 1f; // 기본 피치 설정
    }

    public void InitSoundPool()
    {
        availableSources = new List<AudioSource>();
        usedSources = new List<AudioSource>();

        // 초기화 과정에서 풀을 다시 구성할 때 기존 소스들을 초기화합니다.
        foreach (var source in usedSources)
        {
            InitializeAudioSource(source);
        }
    }
}
