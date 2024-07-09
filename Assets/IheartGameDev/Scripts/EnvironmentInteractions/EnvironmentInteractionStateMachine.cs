using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Reset,
        Rise,
        Touch
    }

    private EnvironmentInteractionContext m_Context;
    
    [SerializeField] private TwoBoneIKConstraint _LeftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _RightIKConstraint;
    [SerializeField] private MultiRotationConstraint _LeftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _RighttMultiRotationConstraint;

    [SerializeField] private Rigidbody _Rigidbody;
    [SerializeField] private CapsuleCollider _RootCollider;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (m_Context != null && m_Context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(m_Context.ClosestPointOnColliderFromShoulder,0.03f);
        }
    }

    private void Awake()
    {
        ValidateConstraints();
        m_Context = new EnvironmentInteractionContext(_LeftIKConstraint, _RightIKConstraint,
            _LeftMultiRotationConstraint, _RighttMultiRotationConstraint,
            _Rigidbody, _RootCollider,transform.root);
        InitializeStates();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_LeftIKConstraint, "_LeftIKConstraint == null");
        Assert.IsNotNull(_RightIKConstraint, "_RightIKConstraint == null");
        Assert.IsNotNull(_LeftMultiRotationConstraint, "_LeftMultiRotationConstraint == null");
        Assert.IsNotNull(_RighttMultiRotationConstraint, "_RighttMultiRotationConstraint == null");
        Assert.IsNotNull(_Rigidbody, "_Rigidbody == null");
        Assert.IsNotNull(_RootCollider, "_RootCollider == null");
    }

    private void InitializeStates()
    {
        // Add states to inherited StateManager "States" dictionary and Set Initial State
        States.Add(EEnvironmentInteractionState.Reset,new ResetState(m_Context,EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Approach,new ApproachState(m_Context,EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise,new RiseState(m_Context,EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Search,new SearchState(m_Context,EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Touch,new TouchState(m_Context,EEnvironmentInteractionState.Touch));

        CurrentState = States[EEnvironmentInteractionState.Reset];
        ConstructEnvironmentDetectionCollider();
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        float wingspan = _RootCollider.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_RootCollider.center.x, (float)(_RootCollider.center.y + (.25 * wingspan)),(float)(_RootCollider.center.z + (.5 * wingspan)));
        boxCollider.isTrigger = true;
    }
}
