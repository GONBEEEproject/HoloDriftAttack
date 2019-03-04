using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class SwipeGestureManager : MonoBehaviour
{
    [SerializeField]
    private GameObject handTarget;

    private Vector3 lastPos;

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

    private void SourceDetected(InteractionSourceDetectedEventArgs obj)
    {
        handTarget.GetComponent<MeshRenderer>().enabled = true;
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
        }


    }

    private void SourceLost(InteractionSourceLostEventArgs obj)
    {
        handTarget.GetComponent<MeshRenderer>().enabled = false;
    }

    private void SourcePressed(InteractionSourcePressedEventArgs obj)
    {
    }

    private void SourceReleased(InteractionSourceReleasedEventArgs obj)
    {
    }
}
