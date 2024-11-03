using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRandomMover : MonoBehaviour
{
    public float moveRange = 5f; // 移動範圍
    public float changeLightSizeInterval = 2f; // 改變光線大小的間隔
    public Light lightSource; // 需要改變大小的光源
    public float moveSpeed = 2f; // 移動速度

    public bool isRight; // 在右邊

    private Vector3 targetPosition;

    void Start()
    {
        SetRandomTargetPosition();
        lightSource = GetComponent<Light>();
        InvokeRepeating("ChangeLightSize", 0, changeLightSizeInterval);
    }

    void FixedUpdate()
    {
        SmoothMove();
    }

    void SmoothMove()
    {
        // 平滑移動到目標位置
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
        
        // 如果接近目標位置，就設置新的目標位置
        if (Vector3.Distance(transform.localPosition, targetPosition) < 0.05f)
        {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);
        float randomZ = Random.Range(-moveRange, moveRange);
        if(transform.localPosition.x+randomX>moveRange ||
        transform.localPosition.x+randomX<-moveRange)
        {
            randomX *= -1;
        }
        if(transform.localPosition.y+randomY>moveRange ||
        transform.localPosition.y+randomY<-moveRange)
        {
            randomY = 0;
        }
        if(isRight)
        {
            if(transform.localPosition.z+randomZ>moveRange ||
            transform.localPosition.z+randomZ<0)
            {
                randomZ  *= -1;
            }
        }
        else
        {
            if(transform.localPosition.z+randomZ>0 ||
            transform.localPosition.z+randomZ<-moveRange)
            {
                randomZ  *= -1;
            }
        }
        

        targetPosition = transform.localPosition + new Vector3(randomX, 0, randomZ);
    }

    void ChangeLightSize()
    {
        if (lightSource != null)
        {
            lightSource.range = Random.Range(1f, 10f); // 隨機改變光源大小
        }
    }
}
