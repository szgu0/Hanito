using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    public Material newMaterial; // 新材質
    public GameObject objectToActivate; // 要啟動的另一個物件

    // 碰撞事件觸發
    private void OnTriggerEnter(Collider other)
    {
        // 確認玩家標籤或其他條件
        if (other.CompareTag("Player"))
        {
            // 改變當前物件的材質
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && newMaterial != null)
            {
                renderer.material = newMaterial;
            }

            // 啟動另一個物件
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
