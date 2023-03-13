using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringController : MonoBehaviour
{
    // T‰n avulla saadaan keskipiste v‰litetty‰ piirtoa varten
    [SerializeField]
    private BowString bowString;
    // Keskipistett‰ mallintava objecti, jolle jouduing kirjoittamaan overridetyn garb funktion
    // koska p‰‰tin k‰ytt‰‰ OVR:‰‰ eik‰ niiden grab objecteissa ollut unity eventti‰ tarttumiselle
    private OVRGrabbableExtended ovrGrabbable;

    [SerializeField]
    private Transform midPointObject, midPointVisualObject, midPointParent;

    // Kuinka paljon annetaan jousen veny‰
    [SerializeField]
    private float bowStringStretchLimit = 0.4f;

    // K‰si joka liikuttaa jousta
    public Transform interactor;
    private bool isGrabbed = false;
    private float strength, previousStrength;

    [SerializeField]
    private float stringSoundThreshold = 0.001f;

    [SerializeField]
    private AudioSource audioSource;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;

    public int nmrOfHitTargets;

    [SerializeField]
    private int nmrOfTargets;

    [SerializeField]
    private GameObject winningText;

    private void Awake()
    {
        ovrGrabbable = midPointObject.GetComponent<OVRGrabbableExtended>();
    }

    private void Start()
    {
        ovrGrabbable.OnGrabBegin.AddListener(PrepareBowString);
        ovrGrabbable.OnGrabEnd.AddListener(ResetBowString);
        nmrOfHitTargets = 0;
    }

    private void Update()
    {
        if(nmrOfHitTargets >= nmrOfTargets)
        {
            // Pelip‰‰ttyi
            winningText.SetActive(true);
        }

        if(isGrabbed)
        {
            // Maailma koordinaatistosta parentin lokaaliin koordinaatistoon
            Vector3 midPointLocalSpace = midPointParent.InverseTransformPoint(midPointObject.position);

            // Offset
            float stringOffset = MathF.Abs(midPointLocalSpace.z);

            previousStrength = strength;

            // Funktio kutsut
            // Yritet‰‰n menn‰ liian eteen
            if(midPointLocalSpace.z >= 0)
            {
                StringPushedBeyondStart(midPointLocalSpace);
            }
            
            // Kutsutaan funktio jos ollaan vedetty j‰nnett‰ taakse ja yli rajan
            if(midPointLocalSpace.z < 0 && stringOffset >= bowStringStretchLimit)
            {
                StringPulledPastLimit(stringOffset, midPointLocalSpace);
            }
            
            // Kutsutaan funktio vain jos j‰nnett‰ on vedetty (z < 0) ja veto on m‰‰ritetyiss‰ rajoissa
            if(midPointLocalSpace.z < 0 && stringOffset < bowStringStretchLimit)
            {
                PullString(stringOffset, midPointLocalSpace);
            }

            // Jos on tartuttu, piirret‰‰n jousi kolmella pisteell‰
            bowString.CreateString(midPointVisualObject.position);
        }
    }

    // Vedet‰‰n j‰nnett‰
    private void PullString(float stringOffset, Vector3 midPointLocalSpace)
    {
        if(audioSource.isPlaying == false && strength <= 0.01f)
        {
            audioSource.Play();
        }

        strength = CalculateStrength(stringOffset, bowStringStretchLimit);
        midPointVisualObject.localPosition = new Vector3(0, 0, midPointLocalSpace.z);

        PlayStringPullingSound();
    }

    private void PlayStringPullingSound()
    {
        // Jos ollaan vedetty j‰nnett‰ riitt‰v‰sti
        if(MathF.Abs(strength - previousStrength) > stringSoundThreshold)
        {
            if(strength < previousStrength)
            {
                // ƒ‰ni k‰‰nteisesti jos j‰nne menossa eteenp‰in
                audioSource.pitch = -1;
            }
            else
            {
                // J‰nne menossa "normaaliin" suuntaan eli taaksep‰in
                audioSource.pitch = 1;
            }
            audioSource.UnPause();
        }
        else
        {
            // Jos lopetettiin liike, pys‰ytet‰‰n ‰‰nentoisto
            audioSource.Pause();
        }
    }

    // Vahtii ettei j‰nnett‰ vedet‰ liian pitk‰lle
    private void StringPulledPastLimit(float stringOffset, Vector3 midPointLocalSpace)
    {
        audioSource.Pause();
        strength = 1;
        // minus koska veto suunta on negatiivinen
        midPointVisualObject.localPosition = new Vector3(0, 0, -bowStringStretchLimit);
    }

    // Vahtii ettei j‰nnett‰ tyˆnnet‰ leporajan yli
    private void StringPushedBeyondStart(Vector3 midPointLocalSpace)
    {
        // Resetoidaan ‰‰ni
        audioSource.pitch = 1;
        audioSource.Stop();

        strength = 0;
        midPointVisualObject.localPosition = Vector3.zero;
    }

    private float CalculateStrength(float offset, float limit)
    {
        return offset / limit;
    }

    private void PrepareBowString()
    {
        isGrabbed = true;
        OnBowPulled?.Invoke();
    }

    private void ResetBowString()
    {
        // Laukaistaan nuoli
        OnBowReleased?.Invoke(strength);
        strength = 0;
        previousStrength = 0;
        // Resetoidaan pitch jotta se kuulostaa taas normaalilta kun emme soita sit‰ takaperin
        audioSource.pitch = 1;
        audioSource.Stop();

        isGrabbed = false;
        // Keskipiste nollakohtaan
        midPointObject.localPosition = Vector3.zero;
        midPointVisualObject.localPosition = Vector3.zero;
        // Piirret‰‰n perus jousi
        bowString.CreateString(null);
    }
}
