using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace StarterAssets
{
public class UIControl : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;
    public Button[] buttons; // 將你的按鈕拖拽到這裡
    private int currentIndex = 0; // 當前選中按鈕的索引

    public PlayerInput m_PlayerInput;
    public InputAction m_GoodAction;
    public InputAction m_EscAction;

    private bool NotRepeat;

    public GameObject Menu;

    

    void Start()
    {
        // 初始化選中第一個按鈕
        SelectButton(currentIndex);
        if (m_PlayerInput == null)
        {
            m_GoodAction = m_PlayerInput.actions["Good"];
            m_EscAction = m_PlayerInput.actions["Esc"];
        }
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical"); // 接收 WS 或搖桿上下的輸入

        if (NotRepeat&& Mathf.Abs(verticalInput) > 0.7f)
        {
            if (verticalInput > 0) Navigate(-1); // 向上移動
            else if (verticalInput < 0) Navigate(1); // 向下移動
            NotRepeat = false;
        }
        else
        {
            NotRepeat = true;
        }
        Debug.Log(verticalInput);

        // 檢測選擇按鍵
        if (Input.GetKeyDown(KeyCode.J) || m_GoodAction.triggered)
        {
            buttons[currentIndex].onClick.Invoke(); // 觸發按鈕點擊事件
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) || m_EscAction.triggered)
        {
            Esc();
        }
    }

    void Navigate(int direction)
    {
        currentIndex += direction;

        // 確保索引不超出範圍
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        else if (currentIndex >= buttons.Length) currentIndex = 0;

        SelectButton(currentIndex);
    }

    void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
    }

    public void Esc()
    {
        
        if(Menu.activeSelf)
            {
                Menu.SetActive(false);
                thirdPersonController.enabled = true;
            }
            else
            {
                Menu.SetActive(true);
                thirdPersonController.enabled = false;
            }
    }

}
}