using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRJump : MonoBehaviour
{
    [SerializeField]
    private OVRPlayerController controller;

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            controller.Jump();
        }
    }
}
