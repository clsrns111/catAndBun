using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundImg : MonoBehaviour
{
    AudioSource audioSource;    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = true;   // 루프 활성화
        audioSource.Play();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        Vector2 spriteSize = sr.sprite.bounds.size;
        transform.localScale = new Vector3(screenWidth / spriteSize.x, screenHeight / spriteSize.y, 1);
    }
}
