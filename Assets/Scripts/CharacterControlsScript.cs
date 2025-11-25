using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

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
    private RaycastHit hit;

    //Axises
    static float xAxis;
    static float zAxis;

    //Bools
    private bool canSprint;
    private bool canCrouch;
    private bool isMoving;

    //Look Stuff
    public float lookSpeed = 2.5f;
    static float rotationUp = 0;

    void Start()
    {
        //Get the charatcer controller from the player body
        controller = GetComponent<CharacterController>();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        speed = 0;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);
        
        rotationUp -= mouseY;
        rotationUp = Mathf.Clamp(rotationUp, -89, 89);

        camera.transform.localRotation = Quaternion.Euler(rotationUp, 0, 0);

        PlayerMovement();

        playerMovement.y += gravity * Time.deltaTime;
    }

   

    void PlayerMovement()
    {
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        if (controller.isGrounded && playerMovement.y > 0)
        {
            playerMovement.y = 0;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
            canSprint = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            
            speed = sprintSpeed;
            canCrouch = false;
        }
        else
        {
            speed = normalSpeed;
            canCrouch = true;
            canSprint = true;
        }

        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

        if (Input.GetKey(KeyCode.Space) && controller.isGrounded && slopeAngle <= controller.slopeLimit)
        {
            playerMovement.y += jumpForce;
        }



        Vector3 move = (transform.right * xAxis) + (transform.forward * zAxis);
        move.y = playerMovement.y;
        controller.Move(move * speed * Time.deltaTime);
    }

}
