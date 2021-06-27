using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerLabel;
    
    public void UpdateTimerRender(float time)
    {
        _timerLabel.text = time.ToString("F2");
    }
}
