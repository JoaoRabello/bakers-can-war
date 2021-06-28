using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ingredient
{
    public string IngredientName;
    public GameObject Prefab;
    
    public float Slices;
    public float CurrentSliceAmount;

    public bool IsComplete()
    {
        return CurrentSliceAmount >= Slices;
    }
}
