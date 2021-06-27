using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private Recipe _recipe;
    [SerializeField] private Transform _ingredientSpawnTransform;
    [SerializeField] private List<Ingredient> _ingredientsInTable = new List<Ingredient>();
    [SerializeField] private List<string> _ingredientNames = new List<string>();

    public Recipe CurrentRecipe => _recipe;
    public List<Ingredient> IngredientsInTable => _ingredientsInTable;

    public bool IsIngredientInTable(string ingredientName)
    {
        return _ingredientsInTable.Any(ingredient => ingredient.IngredientName.Equals(ingredientName));
    }

    public bool IsIngredientInRecipe(string ingredientName)
    {
        return _recipe.Ingredients.Any(ingredientName.Equals);
    }

    public string GetRandomIngredientName()
    {
        return _ingredientNames[Random.Range(0, _ingredientNames.Count)];
    }

    public Ingredient GetIngredientByName(string ingredientName)
    {
        return _ingredientsInTable.Find(ingredient => ingredient.IngredientName.Equals(ingredientName));
    }

    public void SpawnIngredient(Ingredient ingredient)
    {
        Instantiate(ingredient.Prefab, _ingredientSpawnTransform.position, Quaternion.identity);
    }
}
