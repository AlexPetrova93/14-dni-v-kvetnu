using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonologueNodeName
{ // TODO Intro node for each phase? - or is this just PaintingComplete
    Intro1, Mountains1, Willow1, Shore1, PaintingComplete1, 
    Intro2, Mountains2, Willow2, Shore2, PaintingComplete2, 
    Intro3, Mountains3, Willow3, Shore3, PaintingComplete3, 
    Outro,
    None
}

public static class MonologueContainer
{
    public static Dictionary<MonologueNodeName, string> Monologues = new Dictionary<MonologueNodeName, string>()
    {
        { MonologueNodeName.Intro1, 
            "Bydlim na tomhle ostrove od jakziva.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        { MonologueNodeName.Mountains1, 
            "Tohle jsou mracne hory - koment 1" },
        
        { MonologueNodeName.Willow1, 
            "Tohle je pamatna vrba - koment 1" },
        
        { MonologueNodeName.Shore1, 
            "Tohle je breh jezera - koment 1" },
        
        { MonologueNodeName.PaintingComplete1, 
            "To jsem to ale krasne namaloval" },
        
        
        { MonologueNodeName.Intro2, 
            "Dost se toho na mape meni.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        { MonologueNodeName.Mountains2, 
            "Tohle jsou mracne hory - koment 2" },
        
        { MonologueNodeName.Willow2, 
            "Tohle je pamatna vrba - koment 2" },
        
        { MonologueNodeName.Shore2, 
            "Tohle je breh jezera - koment 2" },
        
        { MonologueNodeName.PaintingComplete2, 
            "To jsem to ale krasne namaloval" },
        
        
        { MonologueNodeName.Intro3, 
            "Opet se toho dost na mape meni.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        { MonologueNodeName.Mountains3, 
            "Tohle jsou mracne hory - koment 3" },
        
        { MonologueNodeName.Willow3, 
            "Tohle je pamatna vrba - koment 3" },
        
        { MonologueNodeName.Shore3, 
            "Tohle je breh jezera - koment 3" },
        
        { MonologueNodeName.PaintingComplete3, 
            "To jsem to ale krasne namaloval" },
        
        
        { MonologueNodeName.Outro, 
            "Konecne jsem namaloval vrbu.\n" +
            "A timto muj vstup konci.\n" +
            "Umiram." },
    };

}
