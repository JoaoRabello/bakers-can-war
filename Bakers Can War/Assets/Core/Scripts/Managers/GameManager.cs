using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private List<Match> _placeholderMatches = new List<Match>();

    [SerializeField] private Table _table;

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

    private void Start()
    {
        SetRecipe();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var combo = new Combo(_placeholderMatches);
            
            MakeCombo(combo);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResetScore();
        }
    }

    private void SetRecipe()
    {
        _scoreManager.SetCurrentRecipe(_table.CurrentRecipe);
    }

    public void MakeCombo(Combo combo)
    {
        _scoreManager.CountScore(combo);

        foreach (var match in combo.Matches)
        {
            AddSliceToIngredient(match);
        }
    }

    private void AddSliceToIngredient(Match match)
    {
        if (!_table.IsIngredientInTable(match.ToppingName)) return;
        
        var ingredient = _table.GetIngredientByName(match.ToppingName);
        ingredient.CurrentSliceAmount++;

        if (ingredient.IsComplete())
        {
            Debug.Log($"{ingredient.IngredientName} complete");
            _table.SpawnIngredient(ingredient);
        }
    }

    public void ResetScore()
    {
        _scoreManager.ResetScore();
    }
}
