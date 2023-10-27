using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class PickUpState : AIState
{
    public override void OnCollisionEnter(AIStateManager state, Collider collider)
    {
    }

    public override void OnEnterState(AIStateManager state)
    {
        Debug.Log("PickUpState");
        LookForObjectsToPick(state);
    }

    public override void OnUpdateState(AIStateManager state)
    {
    }

    private void LookForObjectsToPick(AIStateManager state)
    {
        Collider[] colliders = Physics.OverlapSphere(state.transform.position, state.sphereRadius,state.objectsToPick);

        if(colliders.Length > 0 )
        {
            state.objOnHand = colliders[0].gameObject;
            state.currentTarget = state.secondTarget;
            state.SwitchState(state.moveState);
        }
    }
}
