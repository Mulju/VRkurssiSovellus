using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    // Feikkaa n‰timp‰‰ nuolen liikett‰ pit‰en nuolen per‰n samassa linjassa kuin k‰rjen lentoradalla

    [SerializeField]
    private Rigidbody rb;

    private void FixedUpdate()
    {
        transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.fixedDeltaTime);
    }
}
