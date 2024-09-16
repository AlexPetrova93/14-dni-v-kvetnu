using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour
{
    public Image Bubble;
    public TextMeshProUGUI Text;

    public void Init()
    {
        Bubble.enabled = false;
        Text.text = "";
    }
    
    public IEnumerator RunMonologue(MonologueNodeName nodeName)
    {
        if (MonologueContainer.Monologues.ContainsKey(nodeName))
        {
            #if UNITY_EDITOR
            Debug.LogError("Node " + nodeName + " not found in monologue dictionary!");
            #endif

            yield break;
        }
        
        var lines = MonologueContainer.Monologues[nodeName];
        var linesArr = lines.Split("\n");

        Bubble.enabled = true;
        
        // TODO show line by line
        foreach (var line in linesArr)
        {
            Text.text = line;
            yield return new WaitForSeconds(5);  // TODO wait for click
        }
        Text.text = "";
        Bubble.enabled = false;
    }
}
