using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAterPlay : StateMachineBehaviour {

    public string Attribute;
    public int ResetValue;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger(Attribute, ResetValue);	
	}

}
