using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEffectManager : MonoBehaviour {

    public Color DefaultColor;
    private SpriteRenderer spriteRenderer;
    private object lastCalled;
    private bool stopOtherGlows = false;

    private Stack<Color> colorStack = new Stack<Color>();

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                if (putOnColorStack)
                {
                    colorStack.Push(color);
                }
                this.lastCalled = caller;
                this.stopOtherGlows = stopOtherGlows;
                spriteRenderer.color = color;
            }
        }

    }

    /// <summary>
    /// requires a reference to object calling to ensure caller has 'authority' to turn it off
    /// </summary>
    public void TurnOff(object caller)
    {
        if (lastCalled == null || this.lastCalled.Equals(caller))
        {
            if (colorStack.Count > 0)
            {
                colorStack.Pop();
            }
            if (colorStack.Count > 0)
            {
                spriteRenderer.color = colorStack.Peek();
            }
            else
            {
                spriteRenderer.color = DefaultColor;
            }
        }
    }

}
