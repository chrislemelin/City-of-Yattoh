using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorEffectManager : MonoBehaviour {

    [SerializeField]
    private List<Image> targets = new List<Image>();

    public Color DefaultColor;
    private SpriteRenderer spriteRenderer;
    private object lastCalled;
    private bool stopOtherGlows = false;
    private float alpha;

    private Stack<Color> colorStack = new Stack<Color>();

    public void Start()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = DefaultColor.a;
       
    }

    public void SetAlpha(float alpha)
    {
        this.alpha = alpha;
        SetColor(GetCurrentColor());
    }

    private Color GetCurrentColor()
    {
        if (colorStack.Count > 0)
        {
            return GetColor(colorStack.Peek());
        }
        else
        {
            return GetColor(DefaultColor);
        }
    }

    private Color GetColor(Color col)
    {
        return new Color(col.r, col.g, col.b, alpha);
    }

    private void SetColor(Color col)
    {
        if(spriteRenderer != null)
            spriteRenderer.color = GetCurrentColor();
        foreach(Image image in targets)
        {
            image.color = GetCurrentColor();
        }
       
    }

    /// <summary>
    /// requires a reference to object calling to ensure caller has 'authority' to turn it off when the time comes,
    /// no longer using the stopOtherGlows, but I can see it being usefull in the future, hovering glows no using
    /// onlyAddIfStackEmpty
    /// </summary>
    /// <param name="lastCalled"></param>
    public void TurnOn(object caller, Color color, bool stopOtherGlows = false, bool putOnColorStack = true, bool onlyAddIfStackEmpty = false)
    {
        if (!onlyAddIfStackEmpty || colorStack.Count == 0)
        {
            if ((this.stopOtherGlows && lastCalled == caller) || !this.stopOtherGlows)
            {
                Color newCol = new Color(color.r, color.g, color.b, alpha);
                if (putOnColorStack)
                {
                    colorStack.Push(newCol);
                }
                this.lastCalled = caller;
                this.stopOtherGlows = stopOtherGlows;
                SetColor(newCol);
   
            }
        }

    }

    /// <summary>
    /// requires a reference to object calling to ensure caller has 'authority' to turn it off
    /// </summary>
    public void TurnOff(object caller, Color? lastColor = null)
    {
        if (lastCalled == null || this.lastCalled.Equals(caller) &&  (lastColor == null || colorStack.Peek().Equals(lastColor)))
        {
            if (colorStack.Count > 0)
            {
                colorStack.Pop();
            }
            SetColor(GetCurrentColor());
        }
    }

}
