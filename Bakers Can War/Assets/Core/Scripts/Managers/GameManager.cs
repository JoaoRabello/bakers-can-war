using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private List<Match> _placeholderMatches = new List<Match>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // var matchList = new List<Match>();
            // var match = new Match("morango", 3);
            // matchList.Add(match);
            var combo = new Combo(_placeholderMatches);
            
            MakeCombo(combo);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResetScore();
        }
    }

    public void MakeCombo(Combo combo)
    {
        _scoreManager.CountScore(combo);
    }

    public void ResetScore()
    {
        _scoreManager.ResetScore();
    }
}
