using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color blinkColor;
    public float duration;
    public int blinkCount;

    public void Trigger()
    {
        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        Color OGColor = spriteRenderer.color;

        int index = 0;
        while (index <= blinkCount)
        {
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = OGColor;
            yield return new WaitForSeconds(duration);
            index++;
        }
        spriteRenderer.color = OGColor;
    }
}
