using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slider : MonoBehaviour
{
    [SerializeField] TMP_Text sliderValue;
    GridGenerator GM;

    private void Start()
    {
        GM = GridGenerator.Instance;
        OnSliderChanged(1);
    }

    public void OnSliderChanged(float value)
    {
        sliderValue.text = value.ToString();
        GM.ChangeSpeed(value);
    }
}
