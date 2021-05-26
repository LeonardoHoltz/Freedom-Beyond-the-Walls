using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    // Option to follow an unit
    public Transform followTransform;

    // Zoom of the camera
    public Transform cameraTransform;

    // Camera Movement
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    
    // Camera Rotation Speed
    public float rotationAmount;

    // Camera Zoom Speed
    public Vector3 zoomAmount;
    public int zoomMin = 10;
    public int zoomMax = 300;

    // Storage of the camera:
    // Vectors stores positions and quaternions stores rotations
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player wants the camera to follow an unit or if wants to controll it freely
        if(followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleMovementInput();
        }



        if (Input.GetKey(KeyCode.Escape))
        {
            //transform.position = followTransform.position; // When the camera stops following, it should stay in the same position and not in the last position, before the following
            followTransform = null;
        }
    }


    void HandleMovementInput()
    {
        // Change in the Camera movement speed
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        // newPosition based on WASD (for now)
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        // newRotation based on QE
        if(Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        // newZoom based on Mouse ScrollWheel
        if (Input.mouseScrollDelta.y > 0)
        {
            if(newZoom.y > zoomMin)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            if (newZoom.y < zoomMax)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
        }

        /* Camera Rig new position and rotation based on an interpolation between the last position
         * and the new postion regarding the time between each frame */
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
