using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CharacterControlsScript : MonoBehaviour
{
    //Our character controller attached to the player
    private CharacterController controller;
    public Camera camera;

    //Floats of player stuff
    public float speed = 5f;
    public float normalSpeed = 5f;
    public float crouchSpeed = 2.5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -1f;

    private Vector3 playerMovement;

    //Axises
    static float xAxis;
    static float zAxis;

    //Bools
    private bool canSprint;
    private bool canCrouch;
    private bool isMoving;

    //Look Stuff
    public float lookSpeedX = 2.5f;
    public float lookSpeedY = 2.5f;

    public float lowerYLimit = -70.0f;
    public float higherYLimit = 70.0f;

    void Start()
    {
        //Get the charatcer controller from the player body
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        speed = 0;
    }

    private void Update()
    {
        CameraMovement();
        PlayerMovement();
    }

    void CameraMovement()
    {
        //Look up and down
        float rotationUp =- Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationUp = Mathf.Clamp(rotationUp, lowerYLimit, higherYLimit);
        camera.transform.localEulerAngles = Vector3.up * rotationUp;

        //Look left and right
        transform.Rotate(Vector3.right * (Input.GetAxis("Mouse X") * lookSpeedX));
       

    }

    void PlayerMovement()
    {
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        if(controller.isGrounded && playerMovement.y > 0)
        {
            playerMovement.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
            canSprint = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            canCrouch = false;
        }
        else
        {
            speed = normalSpeed;
        }


        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            playerMovement.y += Mathf.Sqrt(jumpForce * -2.0f * gravity);
        }

        playerMovement.y += gravity * Time.deltaTime;

        Vector3 move = new Vector3(xAxis, playerMovement.y, zAxis); 
        controller.Move(move * speed * Time.deltaTime);
    }

}
