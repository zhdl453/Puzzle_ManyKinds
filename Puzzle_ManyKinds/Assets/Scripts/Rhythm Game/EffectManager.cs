using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator;
    [SerializeField] Animator judgementAnimator;
    [SerializeField] Sprite[] judgementSprites;
    [SerializeField] Image judgementImage;

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger("Hit");
    }
    public void JudgementEffect(int index)
    {
        judgementAnimator.SetTrigger("Hit");
        judgementImage.sprite = judgementSprites[index];
    }
}
