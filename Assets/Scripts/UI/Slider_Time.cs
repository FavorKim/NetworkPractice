using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Time : MonoBehaviour
{
    [SerializeField] Slider slider_Timebar;

    private void FixedUpdate()
    {
        slider_Timebar.value -= Time.deltaTime / 60;
    }
}
