using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] public AudioClip[] effectClips;
    [SerializeField] public AudioClip[] uiClips;
    [SerializeField] public AudioClip[] bgmClips;
    public AudioSourcePool audioSourcePool;

    public bool isMuteSFXSound = false;
    public bool isMuteBackGroundSound = false;

    private Dictionary<string, AudioSource> playingSounds = new Dictionary<string, AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var clip in bgmClips)
        {
            if (clip.loadState != AudioDataLoadState.Loaded)
            {
                clip.LoadAudioData(); // ����� �����͸� �̸� �ε�
            }
        }

        foreach (var clip in effectClips)
        {
            if (clip.loadState != AudioDataLoadState.Loaded)
            {
                clip.LoadAudioData(); // ����� �����͸� �̸� �ε�
            }
        }

        foreach (var clip in uiClips)
        {
            if (clip.loadState != AudioDataLoadState.Loaded)
            {
                clip.LoadAudioData(); // ����� �����͸� �̸� �ε�
            }
        }
    }

    public void PlayEffectSound(int idx, string soundKey)
    {
        if (isMuteSFXSound) return;

        AudioSource source = audioSourcePool.GetAudioSource();
        if (source == null)
        {
            Debug.LogWarning("PlayEffectSound: Unable to get an AudioSource from the pool.");
            return;
        }

        source.clip = effectClips[idx];
        source.Play();
        playingSounds[soundKey] = source; // Dictionary�� �߰�
        StartCoroutine(ReturnToPoolAfterPlaying(source, source.clip.length));
    }

    public void PlayBackGroundSound(int idx)
    {
        if (isMuteBackGroundSound) return;


        if (playingSounds.ContainsKey("BGM"))
        {
            AudioSource currentBGM = playingSounds["BGM"];
            if (currentBGM.isPlaying)
            {
                currentBGM.Stop();
            }
            audioSourcePool.ReturnAudioSource(currentBGM);
            playingSounds.Remove("BGM");
        }

        AudioSource source = audioSourcePool.GetAudioSource();
        if (source == null)
        {
            Debug.LogWarning("PlayBackGroundSound: Unable to get an AudioSource from the pool.");
            return;
        }

        // Ŭ�� ���� �� ���
        source.clip = bgmClips[idx];
        source.loop = true;
        source.Play();
        playingSounds["BGM"] = source; // Dictionary�� �߰�
    }

    public void PlayUiSound(int idx)
    {
        // ȿ������ ���Ұ� ���¶�� ��ȯ
        if (isMuteSFXSound) return;

        // �ε����� ������ ����� �ʾҴ��� Ȯ��
        if (idx < 0 || idx >= uiClips.Length)
        {
            Debug.LogWarning($"PlayUiSound: Invalid index {idx}. Ensure it's within the range of uiClips.");
            return;
        }

        if (playingSounds.ContainsKey("UI") && playingSounds["UI"] != null)
        {
            AudioSource currentSource = playingSounds["UI"];
            if (currentSource.clip == uiClips[idx] && currentSource.isPlaying)
            {
                return;
            }
        }

        AudioSource source = audioSourcePool.GetAudioSource();
        if (source == null)
        {
            Debug.LogWarning("PlayUiSound: Unable to get an AudioSource from the pool.");
            return;
        }

        source.clip = uiClips[idx];
        source.Play();
        playingSounds["UI"] = source;

        StartCoroutine(ReturnToPoolAfterPlaying(source, source.clip.length));
    }

    public void SetMuteSFXSound()
    {
        isMuteSFXSound = !isMuteSFXSound;
    }

    public void SetMuteBGMSound()
    {
        isMuteBackGroundSound = !isMuteBackGroundSound;

        if (isMuteBackGroundSound)
        {
            if (playingSounds.ContainsKey("BGM"))
            {
                AudioSource currentBGM = playingSounds["BGM"];

                if (currentBGM != null) 
                    currentBGM.Stop();
            }
        } else
        {
            if (GameManager.instance.isGameOver)
                return;

            if (playingSounds.ContainsKey("BGM"))
            {
                AudioSource currentBGM = playingSounds["BGM"];

                if (currentBGM != null)
                    currentBGM.Play();
            } else
            {
                if(GameManager.instance.isGameStart)
                {
                    PlayBackGroundSound(1);
                } else
                {
                    PlayBackGroundSound(0);
                }
            }
        }
    }

    public void StopSound(string soundKey)
    {
        if (playingSounds.ContainsKey(soundKey))
        {
            AudioSource source = playingSounds[soundKey];
            if (source != null && source.isPlaying)
            {
                source.Stop();
                audioSourcePool.ReturnAudioSource(source); // Ǯ�� ��ȯ
            }
            playingSounds.Remove(soundKey); // Dictionary���� ����
        }
    }

    private IEnumerator ReturnToPoolAfterPlaying(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        if (source != null && source.gameObject != null)
        {
            source.Stop();
            audioSourcePool.ReturnAudioSource(source); // Ǯ�� ��ȯ
        }
    }

    public void InitSound()
    {
        foreach (var audioSource in playingSounds)
        {
            audioSource.Value.Stop();
        }

        playingSounds = new Dictionary<string, AudioSource>();
    }
}
