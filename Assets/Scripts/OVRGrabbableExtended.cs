using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Overridetty OVRGrabbable jotta saa Unity Eventin kun 
// funktioita GrabBegin ja GrabEnd kutsutaan (kun tartutaan tai irrotetaan objektista)
public class OVRGrabbableExtended : OVRGrabbable
{
    [HideInInspector] public UnityEvent OnGrabBegin;
    [HideInInspector] public UnityEvent OnGrabEnd;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        OnGrabBegin.Invoke();
        base.GrabBegin(hand, grabPoint);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        OnGrabEnd.Invoke();
    }
}
