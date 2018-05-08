using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    [Tooltip("In ms^-1")] [SerializeField] float controlSpeed = 20f;
    [SerializeField] float turnFactor = 5f;
    [SerializeField] GameObject[] guns;

    //Member variables
    Rigidbody rigidbody;
    float xThrow, yThrow;   //for 2-D, the y is actually the "z" direction
    bool isControlEnabled = true;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isControlEnabled)
        {
            processMovement();
            ProcessFiring();
        }
    }

    //Cool rotation for gravity pull!!!
    //transform.Rotate(rawXPos, 0, rawYPos);

    //continuous rotation for power-ups!)
    //transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);


    //NOTE: Because we're in 2-D, this works on the X and Z axes, Y never moves.
    private void processMovement()
    {
        Quaternion current = transform.localRotation;

        //TO-DO: Look into restricting player movement at the "edge of galaxy"
        //TO-DO: Smooth out the animation for rotation
        //Alert player about range and maybe self-destruct?

        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * controlSpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        //float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float rawYPos = transform.localPosition.z + yOffset;
        //float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        //Move player in the X and Z axis, and rotate
        //TO-DO: Make the movement circle based, and not cardinal direction based
        if(xThrow == 0 && yThrow == 0)
        {
            //do nothing
        }
        else if(xThrow > 0 && yThrow == 0) //East
        {
            movePlayer(current, rawXPos, rawYPos, 90f);
        }
        else if (xThrow < 0 && yThrow == 0) //West
        {
            movePlayer(current, rawXPos, rawYPos, -90f);
        }
        else if(xThrow == 0 && yThrow > 0) //North
        {
            movePlayer(current, rawXPos, rawYPos, 0f);
        }
        else if (xThrow == 0 && yThrow < 0) //South
        {
            movePlayer(current, rawXPos, rawYPos, -180f);
        }
        else if(xThrow > 0 && yThrow > 0) //NorthWest
        {
            movePlayer(current, rawXPos, rawYPos, 45f);
        }
        else if (xThrow < 0 && yThrow > 0) //NorthEast
        {
            movePlayer(current, rawXPos, rawYPos, -45f);
        }
        else if (xThrow < 0 && yThrow < 0) //SouthEast
        {
            movePlayer(current, rawXPos, rawYPos, -135f);
        }
        else if (xThrow > 0 && yThrow < 0) //SouthWest
        {
            movePlayer(current, rawXPos, rawYPos, 135f);
        }
    }

    private void movePlayer(Quaternion current, float rawXPos, float rawYPos, float degree)
    {
        transform.localPosition = new Vector3(rawXPos, 0f, rawYPos);
        Vector3 newDirection = new Vector3(-90f, degree, current.z);
        transform.localRotation = Quaternion.Euler(newDirection);
    }


    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire1"))
        {
            toggleGuns(true);
        }
        else
        {
            toggleGuns(false);
        }

    }

    private void toggleGuns(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(isActive);//----using to set the higher level component

            //Accessing lower level components
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }


}
