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
            // Ǯ�� ������ ������ ���ο� AudioSource �߰�
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ���Ǳ� ���� �ʱ�ȭ
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

        // ��ȯ�� �� �ʱ�ȭ
        InitializeAudioSource(source);
    }

    private void InitializeAudioSource(AudioSource source)
    {
        source.Stop(); // ������ ����� ������� ����
        source.clip = null; // ���� Ŭ�� �ʱ�ȭ
        source.loop = false; // �ݺ� ���� �ʱ�ȭ
        source.volume = 1f; // �⺻ ���� ����
        source.pitch = 1f; // �⺻ ��ġ ����
    }

    public void InitSoundPool()
    {
        availableSources = new List<AudioSource>();
        usedSources = new List<AudioSource>();

        // �ʱ�ȭ �������� Ǯ�� �ٽ� ������ �� ���� �ҽ����� �ʱ�ȭ�մϴ�.
        foreach (var source in usedSources)
        {
            InitializeAudioSource(source);
        }
    }
}
