using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRunner : MonoBehaviour
{
    public MonologueManager MonologueManager;
    public TexturePainter TexturePainter;

    public Button DoorHandle;
    
    // Level runner
    public IEnumerator StartLevel()
    {
        yield return MonologueManager.RunMonologue(MonologueNodeName.Intro);
        
        // allow looking around + returning back to painting (after a while?)
    }
    
    
    // General functions - delete TODO
    public void RunPainting(int phaseNumber)
    {
        TexturePainter.OnPaintingComplete.AddListener(ActivateDoorHandle);
    }

    public void ActivateDoorHandle()
    {
        //DoorHandle.onClick.AddListener(ShowWorld());
    }

    public void ShowWorld(MonologueNodeName introNode)
    {
        MonologueManager.RunMonologue(introNode);
    }
}
