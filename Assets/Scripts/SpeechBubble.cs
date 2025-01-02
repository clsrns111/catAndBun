using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Image bunImage;
    [SerializeField] Sprite bunSpriteImage_red;
    [SerializeField] Sprite bunSpriteImage_cream;

    [SerializeField] SpriteRenderer NumberSpriteRender;
    [SerializeField] SpriteRenderer NumberSpriteRender_cream;

    BunType bunType;


    private void Start()
    {
        
    }

    public virtual void UpdateText(int requireRedBin, int requireCream)
    {
        if(requireRedBin < 0 || requireCream < 0) return;

        switch (bunType)
        {
            case BunType.cream:
                NumberSpriteRender.sprite = numberSprites[requireCream];
                break;
            case BunType.redBin:
                NumberSpriteRender.sprite = numberSprites[requireRedBin];
                break;
            case BunType.both:
                NumberSpriteRender.sprite = numberSprites[requireRedBin];
                NumberSpriteRender_cream.sprite = numberSprites[requireCream];
                break;
        }
    }

    public virtual void SetRandomBunImage(BunType bunType)
    {
        this.bunType = bunType; 

        if(bunType == BunType.redBin)
            bunImage.sprite = bunSpriteImage_red;
        else if (bunType == BunType.cream)
            bunImage.sprite = bunSpriteImage_cream;
    }
}
