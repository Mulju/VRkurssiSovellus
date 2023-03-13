using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private GameObject midPointVisual, arrowPrefab, arrowSpawnPoint;

    [SerializeField]
    private float maxSpeed = 10;

    [SerializeField]
    private AudioSource bowReleaseAudioSource;

    public void PrepareArrow()
    {
        midPointVisual.SetActive(true);
    }

    public void ReleaseArrow(float strength)
    {
        bowReleaseAudioSource.Play();
        midPointVisual.SetActive(false);

        // Ammutaan nuoli
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = arrowSpawnPoint.transform.position;
        arrow.transform.rotation = midPointVisual.transform.rotation;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(midPointVisual.transform.forward * strength * maxSpeed, ForceMode.Impulse);
    }
}
