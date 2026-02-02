using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContDownPanel : MonoBehaviour
{
    public RawImage countDownRawImage;
 
    public Texture[] countTextures;
    int lastValue = -1;

    public void SetCount(int value) 
    {
        if (value == lastValue) return;
        lastValue = value;

        if (value <= 0 || value > countTextures.Length)
        {
            countDownRawImage.enabled = false;
            return;
        }

        countDownRawImage. enabled = true;
        countDownRawImage.texture = countTextures[value - 1];
    }
}
