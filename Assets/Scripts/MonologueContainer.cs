using System.Collections.Generic;

public enum MonologueNodeName
{ 
    MapIntro1, Mountains1, Willow1, Shore1, PaintingComplete1, 
    MapIntro2, Mountains2, Willow2, Shore2, PaintingComplete2, 
    MapIntro3, Mountains3, Willow3, Shore3, PaintingComplete3,
    PaintingComplete4, 
    Outro,
    Intro,
    None
}

public static class MonologueContainer
{
    public static Dictionary<MonologueNodeName, string> Monologues = new Dictionary<MonologueNodeName, string>()
    {
        // ----------------------------------------------------------------------------------------------------
        
        { MonologueNodeName.Intro, 
            "Bydlim na tomhle ostrove od jakziva.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete1, 
            "To jsem to ale krasne namaloval.\n" +
            "Mohl bych se podivat, jak to vypada venku" },
        
        { MonologueNodeName.MapIntro1, 
            "Krasny den.\n" +
            "Porozhlednu se po okoli." },
        
        { MonologueNodeName.Mountains1, 
            "Tohle jsou mracne hory - koment 1" },
        
        { MonologueNodeName.Willow1, 
            "Tohle je pamatna vrba - koment 1" },
        
        { MonologueNodeName.Shore1, 
            "Tohle je breh jezera - koment 1" },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete2, 
            "To jsem to ale krasne namaloval" },
        
        { MonologueNodeName.MapIntro2, 
            "Dost se toho na mape meni.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        { MonologueNodeName.Mountains2, 
            "Tohle jsou mracne hory - koment 2" },
        
        { MonologueNodeName.Willow2, 
            "Tohle je pamatna vrba - koment 2" },
        
        { MonologueNodeName.Shore2, 
            "Tohle je breh jezera - koment 2" },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete3, 
            "To jsem to ale krasne namaloval" },
        
        { MonologueNodeName.MapIntro3, 
            "Opet se toho dost na mape meni.\n" +
            "Mam pocit, ze to pleso byvalo mensi." },
        
        { MonologueNodeName.Mountains3, 
            "Tohle jsou mracne hory - koment 3" },
        
        { MonologueNodeName.Willow3, 
            "Tohle je pamatna vrba - koment 3" },
        
        { MonologueNodeName.Shore3, 
            "Tohle je breh jezera - koment 3" },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete4, 
            "Konecne jsem namaloval vrbu" },

        { MonologueNodeName.Outro, 
            "A timto muj vstup konci.\n" +
            "Umiram." },

        // ----------------------------------------------------------------------------------------------------
    };

}
