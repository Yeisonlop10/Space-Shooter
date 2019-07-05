using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    [Header("General")]
    // Defining x and y speeds
    [Tooltip("In ms^-1")][SerializeField] float xSpeed = 15f;
    [Tooltip("In ms^-1")] [SerializeField] float ySpeed = 15f;
    // Defining x and y boundaries
    [Tooltip("In ms^-1")] [SerializeField] float xRange = 15f;
    [Tooltip("In ms^-1")] [SerializeField] float yRange = 8f;

    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    // To define pitch rotation
    [SerializeField] float positionPitchFactor = -2f;
    // To define yaw rotation
    [SerializeField] float positionYawFactor = 2f;

    [Header("Control-throw Based")]
    [SerializeField] float controlPitchFactor = -20f;
    // To define roll rotation
    [SerializeField] float controlRollFactor = -20f;

    float xThrow, yThrow;
    bool isControlEnabled = true;

    // Use this for initialization
    void Start () {
		
	}
    
    // Update is called once per frame
    void Update() {

        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void OnPlayerDeath() // Called by String reference in CollisionHandler
    {
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
        // To calculate the pitch
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        // To calculate the yaw
        float yaw = transform.localPosition.x * positionYawFactor;

        // To calculate the roll
        float roll = xThrow * controlRollFactor;


        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    { 

        // Get the input from horizontal controller or keyboard
         xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        //print(xThrow);
        // Get the input from vertical controller or keyboard
         yThrow = CrossPlatformInputManager.GetAxis("Vertical");
        // Calculate the xOffset per frame
        float xOffset = xThrow * xSpeed * Time.deltaTime;
        // Calculate the xOffset per frame
        float yOffset = yThrow * ySpeed * Time.deltaTime;
        // Calculate the new x position
        float rawXPos = transform.localPosition.x + xOffset;
        // Calculate the new y position
        float rawYPos = transform.localPosition.y + yOffset;
        // Establishing X boundaries
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        // Establishing Y boundaries
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        // After getting the offset for the X axis, move the ship 
        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);

    }

    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire1"))
        {
            SetGunsActive(true);
            
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {

        foreach (GameObject gun in guns)
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

  

    
}
