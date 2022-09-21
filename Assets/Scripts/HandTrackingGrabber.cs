using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class HandTrackingGrabber : OVRGrabber
{
    public OVRHand Hand;
    public float PinchThreshold = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }

    private void CheckIndexPinch()
    {
        float pinchStrength = Hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        Debug.Log($"Zahlenwelten [HandTrackingGrabber]: Pinch Strength {pinchStrength}");
        bool isPinching = pinchStrength > PinchThreshold;

        if (isPinching)
        {
            if (!m_grabbedObj && isPinching && m_grabCandidates.Count > 0)
                GrabBegin();
        }
        else if (m_grabbedObj && !isPinching)
            GrabEnd();
    }
}
