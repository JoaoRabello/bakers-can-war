using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreRenderer _renderer;
    
    private Dictionary<string, int> _scoreDictionary = new Dictionary<string, int>
    {
        {"morango", 100},
        {"kiwi", 150},
    };

    private int _currentScore;

    public void ResetScore()
    {
        _currentScore = 0;
        _renderer.RenderScore(_currentScore);
    }
    
    public void CountScore(Combo combo)
    {
        foreach (var match in combo.Matches)
        {
            var scoreToAdd = _scoreDictionary[match.ToppingName] * match.ToppingAmount;
            _currentScore += scoreToAdd;
        }
        _renderer.RenderScore(_currentScore);
    }
}
