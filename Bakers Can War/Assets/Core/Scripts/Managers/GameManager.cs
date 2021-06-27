using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TimerRenderer _timerRenderer;
    [SerializeField] private float _timeToEnd;
    [SerializeField] private int _scoreToWin;
    [SerializeField] private List<Match> _placeholderMatches = new List<Match>();

    [SerializeField] private Table _table;
    [SerializeField] private AudioSource _matchSource;
    [SerializeField] private AudioSource _hmmBoloSource;

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
        StartCoroutine(TimerToEndLevel());
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

    private IEnumerator TimerToEndLevel()
    {
        float timer = _timeToEnd;
        
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            _timerRenderer.UpdateTimerRender(timer);
            yield return null;
        }

        if (_scoreManager.CurrentScore >= _scoreToWin)
        {
            Debug.Log("Venceu");
        }
        else
        {
            Debug.Log("Perdeu");
        }
    }

    public void MakeCombo(Combo combo)
    {
        _scoreManager.CountScore(combo);

        foreach (var match in combo.Matches)
        {
            AddSliceToIngredient(match);
            if (match.ToppingAmount >= 5)
            {
                _hmmBoloSource.Play();
            }
            else
            {
                _matchSource.Play();
            }
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
