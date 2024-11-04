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
        // POZOR intro tam ted nikde neni !!!
        { MonologueNodeName.Intro, 
            "Bydlim na tomhle ostrove od jakziva.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },  // todo tohle nekam zapracovat
        
        
        // ----------------------------------------------------------------------------------------------------
        
        { MonologueNodeName.MapIntro1, 
            "Je krasný den.\n" +
            "Porozhlédnu se po okolí." },
        
        { MonologueNodeName.Mountains1, 
            "Ty hory dneska vypadají obzvlášť namalovatelně.\n" +
            "Jednou bych se na ně chtěl jít podívat, ale bojím se, že bych tam urmz." },
        
        { MonologueNodeName.Willow1, 
            "O téhle vrbě psal strýc v deníku, co mi nechal.\n" +
            "Prý se k tomu pojí nějaké náboženství nebo co.\n" +
            "Z jeho zápisků to zní jak kraviny." },
        { MonologueNodeName.Farm1, 
            "Tenhle pozemek býval dřív opuštěný, ale teď to vypadá, že je tam celkem rušno.\n" +
            "Zajímalo by mě, co hezkého si tam pěstují." },
        { MonologueNodeName.Yacht1, 
            "Už zas jezdí dokola jak magor.\n" +
            "Pravidelně se snažím upoutat jeho pozornost, ale asi mě neslyší přes ten svůj motor." },
        { MonologueNodeName.Boat1, 
            "Na týhle lodi jsem nikdy neviděl jediného člověka.\n" +
            "Jak kdyby se tam plavila samovolně." },
        { MonologueNodeName.Camp1, 
            "Často odsud slyším hudbu, jak kdyby tam někdo pravidelně trénoval na kytaru.\n" +
            "Přes tu vodu se to hezky nese." },
        
        { MonologueNodeName.PaintingComplete1, 
            "Skvělá práce, to bude jeden z mých lepších.\n" +
            "Mohl bych se podívat, jak to vypadá venku." }, // todo nejakz lepsi hint na otevreni dveri
        
        /*{ MonologueNodeName.Flag1, 
            "Jednou bych se tam chtěl podívat, ale bojím se, že bych tam urmz." },*/
        
        
        // ----------------------------------------------------------------------------------------------------
        
        { MonologueNodeName.MapIntro2, 
            "Dost se toho na mape meni.\n" +
            "Neni tu moc co delat, takze vetsinu casu travim malbou." },
        
        { MonologueNodeName.Mountains2, 
            "Tohle jsou mracne hory - koment 2.\n" +
            "Cas jit je namalovat" },
        
        { MonologueNodeName.Willow2, 
            "Včera jsem zahlédl nějaký průvod" },
        { MonologueNodeName.Farm2, 
            "Rajčata? Možná jablka?\n" +
            "Dal bych si jablka, jediné ovoce, co mi tady vozí, jsou třešně." },
        { MonologueNodeName.Yacht2, 
            "jachta" },
        { MonologueNodeName.Boat2, 
            "Lodicka" },
        { MonologueNodeName.Camp2, 
            "Kemp." },

        { MonologueNodeName.PaintingComplete2, 
            "Hmm...\n" +
            "Tu velikost toho plesa jsem asi trochu přehnal." },
        
        /*{ MonologueNodeName.Flag2, 
            "Vyhlidka." },*/
        
        
        // ----------------------------------------------------------------------------------------------------
        
        { MonologueNodeName.MapIntro3, 
            "Opet se toho dost na mape meni.\n" +
            "..." },
        
        { MonologueNodeName.Mountains3, 
            "Tohle jsou mracne hory - koment 3.\n" +
            "Cas jit je namalovat" },
        
        { MonologueNodeName.Willow3, 
            "Vrba <3" },
        { MonologueNodeName.Farm3, 
            "Včera jsem konečně dostal docela velkou nadílku jablek.\n" +
            "Zajímalo by mě, jestli byly od nich." },
        { MonologueNodeName.Yacht3, 
            "Jachta" },
        { MonologueNodeName.Boat3, 
            "Lodicka" },
        { MonologueNodeName.Camp3, 
            "Kemp." },
        
        { MonologueNodeName.PaintingComplete3, 
            "Nemá na těch horách být víc sněhu?\n" +
            "Asi že je léto." },
        
        /*{ MonologueNodeName.Flag3, 
            "Vyhlidka." },*/
        
        
        // ----------------------------------------------------------------------------------------------------
        
        
        { MonologueNodeName.MapIntro4, 
            "Opet se toho dost na mape meni.\n" +
            "Mám pocit, ze to pleso bývalo menší." },
        
        { MonologueNodeName.Mountains4, 
            "Tohle jsou mracne hory - koment 4.\n" +
            "Teď už to vlastně vypadá, že už by tam nemusela být až taková zima." },
        
        { MonologueNodeName.Willow4, 
            "Vrba <3" },
        { MonologueNodeName.Farm4, 
            "Farma?" },
        { MonologueNodeName.Yacht4, 
            "Už se ani nesnažím na ně volat, stejně mě neuslyší." },
        { MonologueNodeName.Boat4, 
            "Kéž bych tak měl takovou plachetnici." +
            "Kéž by ji sem vítr zavál." },
        { MonologueNodeName.Camp4, 
            "Kemp." },
        
        { MonologueNodeName.PaintingComplete4, 
            "Možná že na těch blábolech ze strýcova deníku něco bude." +
            "" },
        
        /*{ MonologueNodeName.Flag4, 
            "Teď už to vlastně vypadá, že už by tam nemusela být až taková zima." },*/
        
        // ----------------------------------------------------------------------------------------------------

        { MonologueNodeName.Outro, 
            "Dostanu se někdy pryč?\n" +
            "Umřu tady?" },

        // ----------------------------------------------------------------------------------------------------
    };

}
