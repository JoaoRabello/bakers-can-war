using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreRenderer : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreLabel;

    public void RenderScore(int score)
    {
        _scoreLabel.text = $"Score: {score.ToString()}";
    }
}
