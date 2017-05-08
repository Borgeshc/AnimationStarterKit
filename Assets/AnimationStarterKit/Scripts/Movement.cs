using UnityEngine;
using System.Collections;
using InControl;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    float vertical;
    float horizontal;

    bool isCrouching;

    Vector3 direction;

    Animator anim;
    CharacterController cc;
    InputDevice inputDevice;

    private void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        inputDevice = InputManager.ActiveDevice;                    //Check which device is active

        vertical = inputDevice.LeftStick.Y;                         //Set the vertical and horizontal to the active device's Left Stick axes
        horizontal = inputDevice.LeftStick.X;
        direction = new Vector3(horizontal, 0, vertical);           //Store the input as the direction

        anim.SetFloat("Vertical", vertical);                        //Apply the input values to the animation parameters
        anim.SetFloat("Horizontal", horizontal);

        if (inputDevice.RightStickButton.WasPressed)                //Toggle crouching
            isCrouching = !isCrouching;

        if (inputDevice.LeftStickButton)                            //If the Leftstick is pressed then we are sprinting
            Sprint();

        if (!isCrouching && direction.Equals(Vector3.zero))         //If there is no input the character is Idle
            Idle();
        else if (isCrouching && direction.Equals(Vector3.zero))     //If there is no input and crouching is toggled character is IdleCrouching
            IdleCrouch();


        if (isCrouching && !direction.Equals(Vector3.zero))         //If there is input and crouching is toggled character is Crouching
            Crouch();
        else if(!isCrouching && !direction.Equals(Vector3.zero))    //If there is input and crouching is not toggled then we are moving
            Move();                                                 //If we are not idle and the left stick is not pressed then we are moving
    }

    void Idle()                                                     //Set up idle state in the animator
    {
        anim.SetBool("Sprinting", false);
        anim.SetBool("Crouching", false);
        anim.SetBool("Idle", true);
    }

    void IdleCrouch()                                               //Set up idle crouching state in the animator
    {
        print("CrouchIdle");
        anim.SetBool("Sprinting", false);
        anim.SetBool("Idle", true);
        anim.SetBool("Crouching", true);
    }

    void Move()                                                     //Set up movement state in the animator and apply movement
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Sprinting", false);
        anim.SetBool("Crouching", false);

        cc.SimpleMove(transform.forward * vertical * moveSpeed * Time.deltaTime);
        cc.SimpleMove(transform.right * horizontal * moveSpeed * Time.deltaTime);
    }

    void Sprint()                                                   //Set up sprinting state in the animator and apply sprint movement
    {
        isCrouching = false;
        anim.SetBool("Idle", false);
        anim.SetBool("Crouching", false);
        anim.SetBool("Sprinting", true);

        cc.SimpleMove(transform.forward * vertical * sprintSpeed * Time.deltaTime);
        cc.SimpleMove(transform.right * horizontal * sprintSpeed * Time.deltaTime);
    }

    void Crouch()                                                   //Set up crouching state in the animator and apply crouch movement
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Sprinting", false);
        anim.SetBool("Crouching", true);

        cc.SimpleMove(transform.forward * vertical * crouchSpeed * Time.deltaTime);
        cc.SimpleMove(transform.right * horizontal * crouchSpeed * Time.deltaTime);
    }
}
