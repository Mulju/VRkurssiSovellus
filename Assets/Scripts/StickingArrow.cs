using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickingArrow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider sphereCollider;
    [SerializeField]
    private GameObject stickingArrow;

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        sphereCollider.isTrigger = true;

        GameObject arrow = Instantiate(stickingArrow);
        arrow.transform.position = transform.position;
        arrow.transform.forward = transform.forward;

        if(collision.collider.attachedRigidbody != null)
        {
            // Osuttiin objektiin jolla on rigidbody, eli se mahdollisesti liikkuu
            // Parentataan nuoli t‰h‰n objektiin jotta se liikkuu sen mukana
            // T‰s on pieni ongelma jos rigidbody johon kiinnityt‰‰n on skaalattu hassusti, mutta p‰‰tet‰‰n ettei niin ole :D
            arrow.transform.parent = collision.collider.attachedRigidbody.transform;
        }

        collision.collider.GetComponent<IHittable>()?.GetHit();

        Destroy(gameObject);
    }
}
