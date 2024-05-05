using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooltimeUI : MonoBehaviour
{
    public Image slotImage;
    public Image skillImage;
    public Image filterImage;

    [SerializeField] Sprite activeImage;
    [SerializeField] Sprite nonActiveImage;
    public void Fill(float value)
    {
        filterImage.fillAmount = value;
    }

    public void Castable(bool tf)
    {
        //if (tf)
        //{
        //    slotImage.sprite = activeImage;
        //}
        //else
        //{
        //    slotImage.sprite = nonActiveImage;
        //}
        slotImage.sprite = activeImage;

    }
}
