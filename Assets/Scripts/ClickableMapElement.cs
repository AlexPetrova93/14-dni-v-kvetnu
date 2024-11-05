using System;
using UnityEngine;

public class ClickableMapElement : MonoBehaviour
{
    public AnimatedSprite AnimatedSprite;
    public FloatingBoat MovingSprite;
    public FloatingBoat[] Birds;
    public AnimatedSprite[] BirdAnimations;
    public bool StartActive;
    
    public MonologueNodeName MonologueNodeName;

    // TODO pretty click effect
    public void BeginClick()
    {
    }
    
    // TODO hide click effect
    public void CancelClick()
    {
    }
    
    // TODO pretty click reaction
    public void EndClick()
    {
        if (AnimatedSprite == null) AnimatedSprite = GetComponentInChildren<AnimatedSprite>();
        AnimatedSprite.Activate();
        if (MovingSprite == null) MovingSprite = GetComponentInChildren<FloatingBoat>();
        if (MovingSprite != null) MovingSprite.Activate();
        if (Birds != null)
        {
            foreach (var bird in Birds)
            {
                bird.Activate();
            }
        }
        if (BirdAnimations != null)
        {
            foreach (var bird in BirdAnimations)
            {
                bird.Activate();
            }
        }

    }
}
