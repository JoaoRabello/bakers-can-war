using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private Recipe _recipe;
    [SerializeField] private Transform _ingredientSpawnTransform;
    [SerializeField] private List<Ingredient> _ingredientsInTable = new List<Ingredient>();

    public Recipe CurrentRecipe => _recipe;
    public List<Ingredient> IngredientsInTable => _ingredientsInTable;

    public bool IsIngredientInTable(string ingredientName)
    {
        return _ingredientsInTable.Any(ingredient => ingredient.IngredientName.Equals(ingredientName));
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
