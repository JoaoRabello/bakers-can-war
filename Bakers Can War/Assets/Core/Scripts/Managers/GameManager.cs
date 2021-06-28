using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TimerRenderer _timerRenderer;
    [SerializeField] private float _timeToEnd;
    [SerializeField] private int _scoreToWin;
    [SerializeField] private List<IngredientUIRenderObject> _ingredientRenderObjects = new List<IngredientUIRenderObject>();

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
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     var combo = new Combo(_placeholderMatches);
        //     
        //     MakeCombo(combo);
        // }

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
        var ingredientName = match.ToppingName.ToLower();
        if (!_table.IsIngredientInTable(ingredientName)) return;
        
        var ingredient = _table.GetIngredientByName(ingredientName);
        ingredient.CurrentSliceAmount++;

        var renderObject = _ingredientRenderObjects.Find(renderUIObject => renderUIObject.IngredientName.Equals(ingredient.IngredientName));
        renderObject.Image.fillAmount = ingredient.CurrentSliceAmount / ingredient.Slices;
        
        if (ingredient.IsComplete())
        {
            _table.SpawnIngredient(ingredient);
            
            var oldColor = renderObject.Image.color;
            renderObject.Image.color = new Color(oldColor.a, oldColor.b, oldColor.g, 1);
            
            renderObject.IngredientAmountLabel.text = $"x{ingredient.CurrentSliceAmount}";
        }
    }

    public void ResetScore()
    {
        _scoreManager.ResetScore();
    }
}

[Serializable]
public class IngredientUIRenderObject
{
    public string IngredientName;
    public Image Image;
    public TMP_Text IngredientAmountLabel;
}
