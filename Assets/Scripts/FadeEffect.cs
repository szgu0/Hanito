using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Image targetImage; // 拖入目标 Image
    public float minAlpha = 100f; // 最低透明度（单位：百分比）
    public float maxAlpha = 200f; // 最高透明度（单位：百分比）
    public float duration = 2f; // 完成一次透明度变化的时间

    private float alphaRange; // 透明度范围
    private float alphaOffset; // 偏移量，用于将曲线变化调整到正确的透明范围

    void Start()
    {
        // 计算透明度范围
        alphaRange = (maxAlpha - minAlpha) / 255f; // 转换为 [0, 1] 范围
        alphaOffset = minAlpha / 255f;
    }

    void Update()
    {
        if (targetImage == null) return;

        // 通过正弦曲线计算透明度
        float alpha = alphaOffset + (Mathf.Sin(Time.time * Mathf.PI * 2f / duration) + 1f) / 2f * alphaRange;

        // 设置图片颜色的 Alpha 通道
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
}