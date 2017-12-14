using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowManager : MonoBehaviour {

    private SpriteGlow spriteGlow;
    private object lastCalled;
    private bool stopOtherGlows = false;

    private Stack<Color> colorStack = new Stack<Color>();

    public void Start()
    {
        spriteGlow = GetComponent<SpriteGlow>();

    }

    /// <summary>
    /// requires a reference to object calling to ensure caller has 'authority' to turn it off when the time comes,
    /// no longer using the stopOtherGlows, but I can see it being usefull in the future, hovering glows no using
    /// onlyAddIfStackEmpty
    /// </summary>
    /// <param name="lastCalled"></param>
    public void TurnOn(object caller, Color glowColor, bool stopOtherGlows= false, bool putOnColorStack = true, bool onlyAddIfStackEmpty = false)
    {
        if (!onlyAddIfStackEmpty || colorStack.Count == 0)
        {
            if ((this.stopOtherGlows && lastCalled == caller) || !this.stopOtherGlows)
            {
                if (putOnColorStack)
                {
                    colorStack.Push(glowColor);
                }
                this.lastCalled = caller;
                this.stopOtherGlows = stopOtherGlows;
                spriteGlow.GlowColor = glowColor;
                spriteGlow.enabled = true;
            }
        }

    }

    /// <summary>
    /// requires a reference to object calling to ensure caller has 'authority' to turn it off
    /// </summary>
    public void TurnOff(object caller, bool putOnColorStack = true)
    {
        // if the effect we are taking off wasnt put on the stack, we have to check if it was overriden
        // by another effect
        if(putOnColorStack || colorStack.Count == 0)
        {
            // checks the stop other glows block
            if(lastCalled == null || !this.stopOtherGlows ||this.lastCalled.Equals(caller))
            {
                if(colorStack.Count > 0)
                {
                    colorStack.Pop();
                }
                if (colorStack.Count > 0)
                {
                    spriteGlow.GlowColor = colorStack.Peek();
                }
                else
                {
                    spriteGlow.enabled = false;
                }
            }
        }
    }


}
