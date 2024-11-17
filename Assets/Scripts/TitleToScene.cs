using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleToScene : MonoBehaviour
{
    public InputAction jumpAction; // 输入动作，用于检测 Jump
    public int sceneNum;

    void Start()
    {
        jumpAction.Enable();
    }

    void Update()
    {
        // 检测输入动作的触发
        if (jumpAction.triggered)
        {
            // 直接切换到下一个场景
            SceneManager.LoadScene(sceneNum);
        }
    }

    void OnDestroy()
    {
        // 禁用输入动作以释放资源
        jumpAction.Disable();
    }
}
