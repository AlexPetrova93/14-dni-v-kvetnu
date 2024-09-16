using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonologueNodeName
{
    Intro, PaintingCompleteFirst, WillowFirst, BankFirst 
}

public static class MonologueContainer
{
    public static Dictionary<MonologueNodeName, string> Monologues = new Dictionary<MonologueNodeName, string>()
    {
        { MonologueNodeName.Intro, 
            "This is the intro." },
        
        { MonologueNodeName.PaintingCompleteFirst, 
            "PaintingCompleteFirst.\nAnd after new line" },
        
        { MonologueNodeName.WillowFirst, 
            "WillowFirst" },
        
        { MonologueNodeName.BankFirst, 
            "BankFirst" },
    };

}
