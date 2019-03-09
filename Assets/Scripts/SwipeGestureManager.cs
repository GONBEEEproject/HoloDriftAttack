using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class SwipeGestureManager : MonoBehaviour
{
    [SerializeField]
    private GameObject handTarget;
    private MeshRenderer handMesh;

    private Vector3 lastPos;

    private float diffX;
    private bool moving;

    // Use this for initialization
    void OnEnable()
    {
        InteractionManager.InteractionSourceDetected += SourceDetected;
        InteractionManager.InteractionSourceUpdated += SourceUpdated;
        InteractionManager.InteractionSourceLost += SourceLost;
        InteractionManager.InteractionSourcePressed += SourcePressed;
        InteractionManager.InteractionSourceReleased += SourceReleased;
    }

    private void OnDisable()
    {
        InteractionManager.InteractionSourceDetected -= SourceDetected;
        InteractionManager.InteractionSourceUpdated -= SourceUpdated;
        InteractionManager.InteractionSourceLost -= SourceLost;
        InteractionManager.InteractionSourcePressed -= SourcePressed;
        InteractionManager.InteractionSourceReleased -= SourceReleased;
    }

    private void Start()
    {
        handMesh = handTarget.GetComponent<MeshRenderer>();
        handMesh.enabled = false;
    }

    private void SourceDetected(InteractionSourceDetectedEventArgs obj)
    {
        handMesh.enabled = true;
        SphereMove.Instance.TextColorChange(Color.red);
    }

    private void SourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        // 手の位置に置くオブジェクトがnullでなければ
        if (handTarget != null)
        {
            // 手の位置を取得して
            Vector3 p;
            if (obj.state.sourcePose.TryGetPosition(out p))
            {
                // オブジェクトの位置を手の位置に更新
                handTarget.transform.position = p;
            }
        }

        Vector3 handPosition;
        if (obj.state.sourcePose.TryGetPosition(out handPosition))
        {
            var diff = handPosition - lastPos;
            lastPos = handPosition;

            float dx = Vector3.Dot(Camera.main.transform.right, diff);
            float dy = Vector3.Dot(Camera.main.transform.up, diff);

            if (moving)
            {
                SphereMove.Instance.PositionUpdate(dx * 4);
            }
        }
    }

    private void SourceLost(InteractionSourceLostEventArgs obj)
    {
        handMesh.enabled = false;
        MoveEnd();
        SphereMove.Instance.TextColorChange(Color.white);
    }

    private void SourcePressed(InteractionSourcePressedEventArgs obj)
    {
        MoveStart();
        SphereMove.Instance.TextColorChange(Color.green);
    }

    private void SourceReleased(InteractionSourceReleasedEventArgs obj)
    {
        MoveEnd();
        SphereMove.Instance.TextColorChange(Color.red);
    }

    private void MoveStart()
    {
        moving = true;
    }

    private void MoveEnd()
    {
        moving = false;
    }

}
