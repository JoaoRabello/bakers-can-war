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
        {"cereja", 200},
        {"trufa", 250},
        {"macaron", 300},
    };

    private int _currentScore;
    public int CurrentScore => _currentScore;
    private Recipe _currentRecipe;

    public void ResetScore()
    {
        _currentScore = 0;
        _renderer.RenderScore(_currentScore);
    }

    public void SetCurrentRecipe(Recipe recipe)
    {
        _currentRecipe = recipe;
    }
    
    public void CountScore(Combo combo)
    {
        foreach (var match in combo.Matches)
        {
            var matchName = match.ToppingName.ToLower();
            var correctRecipeBonus = _currentRecipe.Ingredients.Contains(matchName) ? 1.5f : 1;
            var scoreToAdd = _scoreDictionary[matchName] * match.ToppingAmount * correctRecipeBonus;
            _currentScore += Mathf.CeilToInt(scoreToAdd);
        }
        _renderer.RenderScore(_currentScore);
    }
}
