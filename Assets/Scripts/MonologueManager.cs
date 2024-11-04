using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour
{
    public CanvasGroup CanvasGroup;

    public RectTransform RectTransform;
    
    public float LeftPadding = 65;
    public float TopPadding = 50;
    
    public float MaxWidth = 1600;
    public float MinHeight = 150;
    
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

        var firstLine = true;
        // TODO show line by line
        foreach (var line in linesArr)
        {
            Text.text = line;
            Text.rectTransform.sizeDelta = new Vector2(MaxWidth, 150);
        
            // get bubble dimensions
            Text.ForceMeshUpdate(true, true);
            var textSize = Text.textBounds.size;
            textSize.y = Mathf.Max(MinHeight, textSize.y);
            Text.rectTransform.sizeDelta = textSize;
            Text.rectTransform.anchoredPosition = new Vector2(LeftPadding, TopPadding);
            
            var newSize = new Vector2(textSize.x + 2*LeftPadding, textSize.y + 2*TopPadding);
            Text.text = "";
            if (firstLine)
            {
                firstLine = false;
                RectTransform.sizeDelta = newSize;
            }
            else
            {
                RectTransform.DOSizeDelta(newSize, .3f);
                yield return new WaitForSeconds(.3f);
            }
            
            var clicking = false;
            var beginTime = 0f;
            var charTime = .03f;
            var lastCharTime = 0f;
            var lastCharIdx = 0;
            var finishedWriting = false;
            while (true)
            {
                if (!finishedWriting && Time.time - lastCharTime > charTime)
                {
                    lastCharTime = Time.time;
                    Text.text += line[lastCharIdx];
                    ++lastCharIdx;
                    finishedWriting = lastCharIdx >= line.Length;
                }
                
                var input = Utility.GetInput();
                if (input.Began)
                {
                    beginTime = Time.time;
                    clicking = RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, input.Position);
                }
                // check if clicking on buuble
                if (input.Ended)
                {
                    if (clicking && Time.time - beginTime < .5f &&
                        RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, input.Position))
                    {
                        if (!finishedWriting)
                        {
                            Text.text = line;
                            finishedWriting = true;
                        }
                        else
                        {
                            break; // go to next line
                        }
                    }
                }

                yield return null;
            }
        }
        
        Text.text = "";
        CanvasGroup.Hide();
    }
}
