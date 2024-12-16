using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterAssets
{
    public class LightController : MonoBehaviour
    {
        public bool isOn; // 燈的狀態
        private Light lightComponent;

        public GameObject indicatorPrefab; // 生成的物件

        private GameObject[] indicators = new GameObject[5]; // 0: 本燈, 1: 上, 2: 下, 3: 左, 4: 右

        public Material newMaterial; // 新材質
        public GameObject objectToActivate; // 要啟動的另一個物件
        public GameObject objectToNotActivate; // 要啟動的另一個物件
        public Transform LayTransform;



        void Start()
        {
            lightComponent = GetComponent<Light>();
            UpdateLight();
        }

        public void ToggleLight()
        {
            isOn = !isOn;
            UpdateLight();
        }

        public void UpdateLight()
        {
            lightComponent.enabled = isOn;
        }

        public void GoodLight()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && newMaterial != null)
            {
                renderer.material = newMaterial;
            }

            // 啟動另一個物件
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                if (objectToNotActivate != null)
                {
                    Rigidbody rb = objectToNotActivate.GetComponent<Rigidbody>();
                    rb.AddForce(Vector3.up * 30f, ForceMode.Impulse);
                }


            }
        }
        public void BadLight()
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                if (objectToNotActivate != null)
                {
                    Rigidbody rb = objectToNotActivate.GetComponent<Rigidbody>();
                    rb.AddForce(Vector3.up * 30f, ForceMode.Impulse);
                }


            }
        }

        public void ToggleAdjacentLights()
        {
            // 獲取相鄰燈的參考
            LightController[] allLights = FindObjectsOfType<LightController>();
            foreach (LightController light in allLights)
            {
                if (Vector3.Distance(transform.position, light.transform.position) == 2) // 假設燈之間距離為1
                {
                    light.ToggleLight();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GamePlayerController player = other.GetComponent<GamePlayerController>();
                if (player != null)
                {
                    player.SetCurrentLight(this); // 設定當前的燈
                    ShowIndicators(); // 顯示指示物件
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GamePlayerController player = other.GetComponent<GamePlayerController>();
                if (player != null)
                {
                    player.ClearCurrentLight(); // 清除當前的燈
                    HideIndicators(); // 隱藏指示物件
                }
            }
        }

        private void ShowIndicators()
        {

            if (this.tag != "Interact" && this.tag != "GoodLight")
            {
                indicators[0] = Instantiate(indicatorPrefab, transform.position + new Vector3(0, -0.99f, 0), Quaternion.identity); // 本燈
                indicators[1] = Instantiate(indicatorPrefab, transform.position + (Vector3.forward * 2) + new Vector3(0, -0.99f, 0), Quaternion.identity); // 上
                indicators[2] = Instantiate(indicatorPrefab, transform.position + (Vector3.back * 2) + new Vector3(0, -0.99f, 0), Quaternion.identity); // 下
                indicators[3] = Instantiate(indicatorPrefab, transform.position + (Vector3.left * 2) + new Vector3(0, -0.99f, 0), Quaternion.identity); // 左
                indicators[4] = Instantiate(indicatorPrefab, transform.position + (Vector3.right * 2) + new Vector3(0, -0.99f, 0), Quaternion.identity); // 右
            }
            else if (this.tag == "GoodLight")
            {
                Debug.Log("s");
                indicators[0] = Instantiate(indicatorPrefab, transform.position + new Vector3(0, 0, 0.05f), new Quaternion(0, 0, 0, 1), transform); // 本燈
            }
        }

        private void HideIndicators()
        {
            foreach (GameObject indicator in indicators)
            {
                if (indicator != null)
                {
                    Destroy(indicator);
                }
            }
        }
    }
}