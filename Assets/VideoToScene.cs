using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class VideoToScene : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 指向 VideoPlayer 组件
    public InputAction jumpAction; // 输入动作，用于检测 Jump

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // 监听 loopPointReached 事件
        videoPlayer.loopPointReached += OnVideoEnd;
        jumpAction.Enable();
    }

    void Update()
    {
        // 检测输入动作的触发
        if (jumpAction.triggered)
        {
            // 直接切换到下一个场景
            SceneManager.LoadScene(2);
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 视频播放完成时切换场景
        SceneManager.LoadScene(2);
    }

    void OnDestroy()
    {
        // 禁用输入动作以释放资源
        jumpAction.Disable();
    }
}
