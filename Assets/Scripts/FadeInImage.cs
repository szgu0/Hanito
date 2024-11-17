using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    public Image targetImage;  // 要淡入的 Image
    public float fadeDuration = 1.0f;  // 渐变持续时间

    private void Start()
    {
        if (targetImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = targetImage.color;
        color.a = 0f;  // 初始透明度为 0
        targetImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Clamp01(elapsedTime / fadeDuration); // 计算当前透明度
            color.a = newAlpha;
            targetImage.color = color; // 更新 Image 的颜色
            yield return null;
        }

        // 确保最终透明度为 1
        color.a = 1f;
        targetImage.color = color;
    }
}
