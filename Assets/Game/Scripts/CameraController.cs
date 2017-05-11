using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject reticle;
    public Vector3 distance;
    public Vector3 zoomedDistance;
    public Vector3 crouchZoomedDistance;
    public float zoomSpd = 2.0f;
    public float speed;
    public float rotationSpeed;

    public float xSpeed;
    public float ySpeed;

    public int yMinLimit;
    public int yMaxLimit;

    [HideInInspector]
    public bool isAiming;

    private float x = 0.0f;
    private float y = 0.0f;

    InputDevice inputDevice;
    float horizontal;
    float vertical;
    Vector3 position;
    Quaternion rotation;
    Animator anim;
    int aimLimit;

    void Start()
    {
        anim = player.GetComponent<Animator>();
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        aimLimit = yMaxLimit;
    }

    public void LateUpdate()
    {
        inputDevice = InputManager.ActiveDevice;                                    //Check which device is active

        horizontal = inputDevice.RightStick.X;                                      //Set the vertical and horizontal to the active device's Right Stick axes
        vertical = inputDevice.RightStick.Y;    

        anim.SetFloat("Horizontal2", horizontal);                                   //apply the input value to the horizontal2 parameter
       

        x -= -horizontal * xSpeed * 0.02f;
        y += -vertical * ySpeed * 0.02f;                                      

        y = ClampAngle(y, yMinLimit, yMaxLimit);                             

        rotation = Quaternion.Euler(y, x, 0.0f);                                    //Clamp the y axis

        anim.SetFloat("Vertical2", transform.localRotation.x);

        if (inputDevice.LeftTrigger)                                                //If pressing left trigger we are aiming
        {
            if (Movement.isCrouching)
                CrouchAim();
            else
                Aim();
        }
        else
        {
            reticle.SetActive(false);
            isAiming = false;
            anim.SetBool("CrouchAiming", false);
            anim.SetBool("Aiming", false);
            position = Vector3.Lerp(position, rotation * distance + player.transform.position, zoomSpd * Time.deltaTime);
        }

        if (horizontal != 0)                                                        //If there is input from the right stick rotate the player accordingly
        {
            Turn();
        }

        CameraFollow();                                                             //Adjust the camera's position
    }

    public static float ClampAngle(float angle, float min, float max)               //Adjust clamp angle
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }

    void CameraFollow()                                                             //Camera follow
    {
        transform.rotation =  rotation;
        transform.position = position;
    }

    void Turn()                                                                     //Character y rotation
    {   
        Quaternion desiredRotation = Quaternion.Euler(0, x, 0);
        player.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    void Aim()                                                                      //Camera aim
    {
        yMaxLimit = aimLimit;
        reticle.SetActive(true);
        isAiming = true;
        anim.SetBool("CrouchAiming", false);
        anim.SetBool("Aiming", true);
        position = rotation * zoomedDistance + player.transform.position;
    }

    void CrouchAim()                                                                //Camera aim
    {
        yMaxLimit = (aimLimit - 10);
        reticle.SetActive(true);
        isAiming = true;
        anim.SetBool("Aiming", false);
        anim.SetBool("CrouchAiming", true);
        position = rotation * crouchZoomedDistance + player.transform.position;
    }
}