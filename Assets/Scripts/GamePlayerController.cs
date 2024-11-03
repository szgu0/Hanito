using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayerController : MonoBehaviour
{
    private LightController currentLight; // 當前燈

    public int LightControllLastStep = 3;

    public TextMeshProUGUI LastStepText;
    
    void Update()
    {
        InteractWithLight();
    }

    private void InteractWithLight()
    {
        if (Input.GetKeyDown(KeyCode.J) && currentLight != null)
        {
            if(currentLight.tag == "Interact")
            {
                LightControllLastStep = 3;
                ResetLights();
            }
            else if(currentLight.tag == "GoodLight")
            {
                currentLight.GoodLight();
            }
            else if(LightControllLastStep>0)
            {
                currentLight.ToggleLight();
                currentLight.ToggleAdjacentLights();
                LightControllLastStep--;
            }
            LastStepText.text = LightControllLastStep+"";
            
        }
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
            if(light.name=="Cube" || light.name=="Cube (2)" || light.name=="Cube (5)" || light.name=="Cube (6)")
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
