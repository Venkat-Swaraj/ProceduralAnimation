using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionContext
{
    public enum EBodySide
    {
        RIGHT,
        LEFT
    }

    
    private TwoBoneIKConstraint _LeftIKConstraint;
    private TwoBoneIKConstraint _RightIKConstraint;
    private MultiRotationConstraint _LeftMultiRotationConstraint;
    private MultiRotationConstraint _RightMultiRotationConstraint;
    private Rigidbody _Rigidbody;
    private CapsuleCollider _RootCollider;
    private Transform _RootTransform;


    public EnvironmentInteractionContext(TwoBoneIKConstraint leftIKConstraint,TwoBoneIKConstraint rightIKConstraint,
        MultiRotationConstraint leftMultiRotationConstraint,MultiRotationConstraint rightMultiRotationConstraint,
        Rigidbody rigidbody,CapsuleCollider rootCollider,Transform rootTransform)
    {
        _LeftIKConstraint = leftIKConstraint;
        _RightIKConstraint = rightIKConstraint;
        _LeftMultiRotationConstraint = leftMultiRotationConstraint;
        _RightMultiRotationConstraint = rightMultiRotationConstraint;
        _Rigidbody = rigidbody;
        _RootCollider = rootCollider;
        _RootTransform = rootTransform;
        CharacterShoulderHeight = leftIKConstraint.data.root.position.y;
    }

    

    public TwoBoneIKConstraint LeftIKConstraint =>_LeftIKConstraint;
    public TwoBoneIKConstraint RightIKConstraint=>_RightIKConstraint;
    public MultiRotationConstraint LeftMultiRotationConstraint =>_LeftMultiRotationConstraint;
    public MultiRotationConstraint RighttMultiRotationConstraint =>_RightMultiRotationConstraint;
    public Rigidbody Rb =>_Rigidbody;
    public CapsuleCollider RootCollider=>_RootCollider;
    public Transform RootTransform => _RootTransform;
    
    public Collider CurrentIntersectingCollider { get; set; }
    public TwoBoneIKConstraint CurrentIKConstraint { get; private set; }
    public MultiRotationConstraint CurrentMultiRotationConstraint { get; private set; }
    public Transform CurrentIKTargetTransform { get; private set; }
    public Transform CurrentShoulderTransform { get; private set; }
    public EBodySide CurrentBodySide { get; private set; }
    public Vector3 ClosestPointOnColliderFromShoulder { get; set; } = Vector3.positiveInfinity;
    public float CharacterShoulderHeight { get;}
    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = _LeftIKConstraint.data.root.position;
        Vector3 rightShoulder = _RightIKConstraint.data.root.position;

        bool isLeftCloser = Vector3.Distance(leftShoulder, positionToCheck) <
                            Vector3.Distance(rightShoulder, positionToCheck);
        if (isLeftCloser)
        {
            Debug.Log("Left side is closer");
            CurrentBodySide = EBodySide.LEFT;
            CurrentIKConstraint = _LeftIKConstraint;
            CurrentMultiRotationConstraint = _LeftMultiRotationConstraint;
        }
        else
        {
            Debug.Log("Right side is closer");
            CurrentBodySide = EBodySide.RIGHT;
            CurrentIKConstraint = _RightIKConstraint;
            CurrentMultiRotationConstraint = _RightMultiRotationConstraint;
        }

        CurrentShoulderTransform = CurrentIKConstraint.data.root.transform;
        CurrentIKTargetTransform = CurrentIKConstraint.data.target.transform;
    }
}
