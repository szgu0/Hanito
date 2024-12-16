using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
namespace StarterAssets
{
    public class GamePlayerController : MonoBehaviour
    {
        private PlayerInput m_PlayerInput;
        private InputAction m_GoodAction;
        private LightController currentLight; // 當前燈

        public int LightControllLastStep = 3;

        public TextMeshProUGUI LastStepText;

        public ThirdPersonController thirdPersonController;

        public DeerController deerController;
        public AudioSource m_MyAudioSource;
        public AudioClip[] m_MyAudioSources;
        public Transform BarImageTrans;
        public Transform TargetBarImageTrans;
        public Image cooldown;
        public float CooldownTime = 5;
        public float CooldownLastTime = 5;
        public GameObject OnCooldown;


        void Start()
        {
            if (m_PlayerInput == null)
            {
                m_PlayerInput = GetComponent<PlayerInput>();
                m_GoodAction = m_PlayerInput.actions["Good"];

            }
        }
        void Update()
        {
            InteractWithLight();
            TargetBarImageTrans.position = BarImageTrans.position;
        }

        void FixedUpdate()
        {
            if (thirdPersonController._input.sprint)
            {
                if (CooldownLastTime > 0)
                {
                    CooldownLastTime -= 1f * Time.deltaTime;
                    
                }
                else
                {
                    thirdPersonController.isCooldown = true;
                    if (!m_MyAudioSource.isPlaying)
                    {
                        m_MyAudioSource.clip = m_MyAudioSources[0];
                        m_MyAudioSource.Play();
                        cooldown.color = Color.red;
                        OnCooldown.SetActive(true);
                    }
                }
            }
            else
            {
                if (CooldownLastTime < CooldownTime)
                {
                    CooldownLastTime += 0.7f * Time.deltaTime;

                    if (CooldownLastTime / CooldownTime > 0.3f)
                    {
                         m_MyAudioSource.Stop();
                         cooldown.color = Color.white;
                         thirdPersonController.isCooldown = false;
                         OnCooldown.SetActive(false);
                    }
                }
            }
            cooldown.fillAmount = CooldownLastTime / CooldownTime;

        }

        private void InteractWithLight()
        {
            if ((Input.GetKeyDown(KeyCode.J) || m_GoodAction.triggered) && currentLight != null)
            {
                if (currentLight.tag == "Interact")
                {
                    LightControllLastStep = 3;
                    ResetLights();
                }
                else if (currentLight.tag == "GoodLight")
                {
                    currentLight.GoodLight();
                    deerController.GoodLightOn();
                    m_MyAudioSource.PlayOneShot(m_MyAudioSources[1]);
                }
                else if (currentLight.tag == "BadLight")
                {
                    currentLight.BadLight();
                    deerController.MoveToExtendedPoint();
                }
                else if (LightControllLastStep > 0)
                {
                    currentLight.ToggleLight();
                    currentLight.ToggleAdjacentLights();
                    LightControllLastStep--;
                }
                LastStepText.text = LightControllLastStep + "";

            }
        }

        public void Dead()
        {
            thirdPersonController.enabled = false;
        }

        public void SetCurrentLight(LightController light)
        {
            currentLight = light;
        }

        public void ClearCurrentLight()
        {
            currentLight = null;
        }

        public void ResetLights()
        {
            // 獲取相鄰燈的參考
            LightController[] allLights = FindObjectsOfType<LightController>();
            foreach (LightController light in allLights)
            {
                if (light.name == "Cube" || light.name == "Cube (2)" || light.name == "Cube (5)" || light.name == "Cube (6)")
                {
                    light.isOn = false;
                    light.UpdateLight();
                }
                else
                {
                    light.isOn = true;
                    light.UpdateLight();
                }
            }
        }

    }
}