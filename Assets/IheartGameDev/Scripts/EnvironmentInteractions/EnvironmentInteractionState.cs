using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    protected EnvironmentInteractionContext m_Context;

    public EnvironmentInteractionState(EnvironmentInteractionContext context,
        EnvironmentInteractionStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        m_Context = context;
    }

    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }

    protected void StartIKTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider.gameObject.layer != LayerMask.NameToLayer("Interactable") ||
            m_Context.CurrentIntersectingCollider == null)
        {
            m_Context.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot =
                GetClosestPointOnCollider(intersectingCollider, m_Context.RootTransform.position);
            m_Context.SetCurrentSide(closestPointFromRoot);
            
            SetIKTargetPosition();
        }
    }

    protected void UpdateIKTargetPosition(Collider intersectingCollider)
    {
        if (intersectingCollider == m_Context.CurrentIntersectingCollider)
        {
            SetIKTargetPosition();
        }
        
    }

    protected void ResetIKTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == m_Context.CurrentIntersectingCollider)
        {
            m_Context.CurrentIntersectingCollider = null;
            m_Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }

    private void SetIKTargetPosition()
    {
        m_Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(m_Context.CurrentIntersectingCollider,
            new Vector3(m_Context.CurrentShoulderTransform.position.x,m_Context.CharacterShoulderHeight,m_Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection =
            m_Context.CurrentShoulderTransform.position - m_Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = .05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;
        Vector3 offsetPosition = m_Context.ClosestPointOnColliderFromShoulder + offset;
        m_Context.CurrentIKTargetTransform.position = offsetPosition;
    }
}
