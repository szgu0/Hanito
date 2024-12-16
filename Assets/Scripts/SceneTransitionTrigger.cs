using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 检查进入触发器的物体是否是玩家
        if (other.CompareTag("Player"))
        {
            // 切换到目标场景
            SceneManager.LoadScene(0);
        }
    }
}
