using System.Collections.Generic;

public enum MonologueNodeName
{ 
    MapIntro1, Mountains1, Willow1, Farm1, Yacht1, Boat1, Camp1, Flag1, PaintingComplete1, 
    MapIntro2, Mountains2, Willow2, Farm2, Yacht2, Boat2, Camp2, Flag2, PaintingComplete2, 
    MapIntro3, Mountains3, Willow3, Farm3, Yacht3, Boat3, Camp3, Flag3, PaintingComplete3,
    MapIntro4, Mountains4, Willow4, Farm4, Yacht4, Boat4, Camp4, Flag4, PaintingComplete4, 
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
            "Tohle jsou mracne hory - koment 1.\n" +
            "Cas jit je namalovat" },
        
        { MonologueNodeName.Willow1, 
            "Vrba <3" },
        { MonologueNodeName.Farm1, 
            "Farma?" },
        { MonologueNodeName.Yacht1, 
            "Jachta" },
        { MonologueNodeName.Boat1, 
            "Lodicka" },
        { MonologueNodeName.Camp1, 
            "Kemp." },
        { MonologueNodeName.Flag1, 
            "Vyhlidka." },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete2, 
            "To jsem to ale krasne namaloval" },
        
        { MonologueNodeName.MapIntro2, 
            "Dost se toho na mape meni.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        { MonologueNodeName.Mountains2, 
            "Tohle jsou mracne hory - koment 2.\n" +
            "Cas jit je namalovat" },
        
        { MonologueNodeName.Willow2, 
            "Vrba <3" },
        { MonologueNodeName.Farm2, 
            "Farma?" },
        { MonologueNodeName.Yacht2, 
            "Jachta" },
        { MonologueNodeName.Boat2, 
            "Lodicka" },
        { MonologueNodeName.Camp2, 
            "Kemp." },
        { MonologueNodeName.Flag2, 
            "Vyhlidka." },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete3, 
            "To jsem to ale krasne namaloval" },
        
        { MonologueNodeName.MapIntro3, 
            "Opet se toho dost na mape meni.\n" +
            "Mam pocit, ze to pleso byvalo mensi." },
        
        { MonologueNodeName.Mountains3, 
            "Tohle jsou mracne hory - koment 3.\n" +
            "Cas jit je namalovat" },
        
        { MonologueNodeName.Willow3, 
            "Vrba <3" },
        { MonologueNodeName.Farm3, 
            "Farma?" },
        { MonologueNodeName.Yacht3, 
            "Jachta" },
        { MonologueNodeName.Boat3, 
            "Lodicka" },
        { MonologueNodeName.Camp3, 
            "Kemp." },
        { MonologueNodeName.Flag3, 
            "Vyhlidka." },
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.PaintingComplete4, 
            "Konecne jsem namaloval vrbu" },
        
        { MonologueNodeName.MapIntro4, 
            "Opet se toho dost na mape meni.\n" +
            "Mam pocit, ze to pleso byvalo mnohem mensi." },
        
        { MonologueNodeName.Mountains4, 
            "Tohle jsou mracne hory - koment 4.\n" +
            "Cas jit je namalovat" },
        
        { MonologueNodeName.Willow4, 
            "Vrba <3" },
        { MonologueNodeName.Farm4, 
            "Farma?" },
        { MonologueNodeName.Yacht4, 
            "Jachta" },
        { MonologueNodeName.Boat4, 
            "Lodicka" },
        { MonologueNodeName.Camp4, 
            "Kemp." },
        { MonologueNodeName.Flag4, 
            "Vyhlidka." },
        
        // ----------------------------------------------------------------------------------------------------

        { MonologueNodeName.Outro, 
            "A timto muj vstup konci.\n" +
            "Umiram." },

        // ----------------------------------------------------------------------------------------------------
    };

}
