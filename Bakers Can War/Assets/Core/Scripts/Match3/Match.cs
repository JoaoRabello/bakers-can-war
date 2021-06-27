using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Match
{
    public string ToppingName;
    public int ToppingAmount;

    public Match(string toppingName, int toppingAmount)
    {
        ToppingName = toppingName;
        ToppingAmount = toppingAmount;
    }
}
