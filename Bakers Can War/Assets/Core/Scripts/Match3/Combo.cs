using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo
{
    private List<Match> _matches;    
    public List<Match> Matches => _matches;

    public Combo(List<Match> matches)
    {
        _matches = matches;
    }

    public void AddMatch(Match match)
    {
        _matches.Add(match);
    }
}
