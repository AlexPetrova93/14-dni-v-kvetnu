using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    
    public Button Next;
    public Image Bubble;
    public TextMeshProUGUI Text;

    public void Init()
    {
        CanvasGroup.Hide(0);
    }
    
    public IEnumerator RunMonologue(MonologueNodeName nodeName)
    {
        if (!MonologueContainer.Monologues.ContainsKey(nodeName))
        {
            #if UNITY_EDITOR
            Debug.LogError("Node " + nodeName + " not found in monologue dictionary!");
            #endif

            yield break;
        }
        
        CanvasGroup.Show();
        
        var lines = MonologueContainer.Monologues[nodeName];
        var linesArr = lines.Split("\n");

        // TODO show line by line
        foreach (var line in linesArr)
        {
            Text.text = line;
            yield return Utility.WaitUntilEvent(Next.onClick);  // TODO wait for click
        }
        Text.text = "";
        CanvasGroup.Hide();
    }
}
