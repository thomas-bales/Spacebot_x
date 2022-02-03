using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreEntry
{
    public static int nameLengthLimit = 15;
    public static int scoreDigitLimit = 3;

    public int Rank { get; set; }
    string name;
    public string Name
    {
        get {return name;}
    }
    public int Score { get; set; }

    public void SetName(string _name)
    {
        if (_name.Length > nameLengthLimit)
        {
            name = _name.Substring(0, 15);
        }
        else
        {
            name = _name;
        }
    }
}
