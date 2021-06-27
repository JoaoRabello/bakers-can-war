using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Table _table;
    [Range(1, 10)]
    [SerializeField] private int _difficulty;
    [SerializeField] private float _minChance;
    [SerializeField] private float _maxChance;
    [SerializeField] private float _scoreBase;
    [SerializeField] private float _wrongRecipeModifier;
    [SerializeField] private float _rightRecipeModifier;

    private int _currentScore;
    private float _timer;

    private void Score()
    {
        var randomIngredient = _table.GetRandomIngredientName();
        var modifier = _table.IsIngredientInRecipe("morango") ? _rightRecipeModifier : _wrongRecipeModifier;
        var score = Random.Range(_minChance, _maxChance) * (1 + _difficulty/10) * _scoreBase * _rightRecipeModifier;

        _currentScore = Mathf.CeilToInt(score);
        Debug.Log(_currentScore);
    }

    private void Update()
    {
        if (_timer >= 2)
        {
            _timer = 0;
            Score();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}
