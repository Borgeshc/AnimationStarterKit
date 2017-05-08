using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 distance;
    public Vector3 zoomedDistance;
    public float zoomSpd = 2.0f;
    public float speed;
    public float rotationSpeed;

    public float xSpeed;
    public float ySpeed;

    public int yMinLimit;
    public int yMaxLimit = 877;

    private float x = 0.0f;
    private float y = 0.0f;

    InputDevice inputDevice;
    float horizontal;
    float vertical;
    Vector3 position;
    Quaternion rotation;
    Animator anim;

    void Start()
    {
        anim = player.GetComponent<Animator>();
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
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

        if (inputDevice.LeftTrigger)                                                //If pressing left trigger we are aiming
            Aim();
        else
        {
            anim.SetBool("Aiming", false);
            position = rotation * distance + player.transform.position;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }

    void Turn()                                                                     //Character y rotation
    {   
        Quaternion desiredRotation = Quaternion.Euler(0, x, 0);
        player.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    void Aim()                                                                      //Camera aim
    {
        anim.SetBool("Aiming", true);
        position = rotation * zoomedDistance + player.transform.position;
    }
}