using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class CharacterControlsScript : MonoBehaviour
{
    //Our character controller attached to the player
    private CharacterController controller;
    public Camera fpsCamera;

    //Floats of player stuff
    public float speed = 5f;
    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -1f;

    //Vector3
    private Vector3 playerJumpMovement;

    //CrouchingStuff
    public float crouchSpeed = 2.5f;
    public float crouchHeight;
    public float cameraOffset = -0.2f;
    private float normalHeight;
    private Vector3 cameraStand;
    private Vector3 cameraCrouch;


    //Axises
    static float xAxis;
    static float zAxis;

    //Bools
    private bool canSprint;
    private bool canCrouch;
    private bool steepSlope;

    //Look Stuff
    public float lookSpeed = 2.5f;
    static float rotationUp = 0;
    static float lowerLookLimit = -89;
    static float upperLookLimit = 89;

    void Start()
    {
        //Get the charatcer controller from the player body
        controller = GetComponent<CharacterController>();

        //I dunno why it made me put UnityEngine in front of both, but it did. I feel it was kinda stupid, but it wouldn't work otherwise.
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        //Look at me, setting speed to 0
        speed = 0;

        //Crouch stuff, getting the heigt in the beginning.
        normalHeight = controller.height;

        cameraStand = fpsCamera.transform.localPosition;
        cameraCrouch = cameraStand + new Vector3(0, cameraOffset, 0);


    }

    private void Update()
    {
        //Camera movement. I did try and have it in a seperate script but it was jittery and not very pretty to look at.

        //Getting MouseX and MouseY axises. With speed inculded. 
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        //Rotates the character instead of just the camera, this way the player forwards changes. 
        transform.Rotate(Vector3.up * mouseX);
        
        //Some looking up stuff. Clamping it so you can't play upside down. 
        rotationUp -= mouseY;
        rotationUp = Mathf.Clamp(rotationUp, lowerLookLimit, upperLookLimit);

        //Up and down looking stuff so cool. Sets the camera rotation rather than the player. 
        fpsCamera.transform.localRotation = Quaternion.Euler(rotationUp, 0, 0);

        //Player movement stuff, setting the y to that. 
        playerJumpMovement.y += gravity * Time.deltaTime;
        PlayerMovement();

        
    }

   

    void PlayerMovement()
    {
        //Getting more Axies, this time being x and z
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        //You may be asking, why make another vector 3 when you have one. It breaks if I use the other one instead of this one. 
        Vector3 move = (transform.right * xAxis) + (transform.forward * zAxis);
        

        steepSlope = false;
        //Using Character controller to check if the player is grounded and also checking if the y is greater than zero to set as zero.
        if (controller.isGrounded && playerJumpMovement.y > 0)
        {
            playerJumpMovement.y = 0;
        }

        //This checks if we're on a slope, espically a steep slope
        if (!controller.isGrounded && (controller.collisionFlags & CollisionFlags.Sides) != 0) steepSlope = true;

        //My set speeds. Run and crouch. I just made it so if you run you can't crouch and vice versa, then reset them both to true no matter what if nothing is held. 
        if (Input.GetKey(KeyCode.LeftControl) && canCrouch == true)
        {
            fpsCamera.transform.localPosition = Vector3.Lerp(fpsCamera.transform.localPosition, cameraCrouch, 1);
            controller.height = crouchHeight;
            speed = crouchSpeed;
            canSprint = false;
            //this is so you don't look into the ground while crouched
            lowerLookLimit = 0;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && canSprint == true)
        {
            speed = sprintSpeed;
            canCrouch = false;
        }
        else
        {
            fpsCamera.transform.localPosition = Vector3.Lerp(fpsCamera.transform.localPosition, cameraStand, 1);
            controller.height = normalHeight;
            speed = normalSpeed;
            canCrouch = true;
            canSprint = true;
            lowerLookLimit = -89;
        }

        //My jump! Checks if the player is grounded and if space is pressed
        if (Input.GetKey(KeyCode.Space) && controller.isGrounded && !steepSlope)
        {
                playerJumpMovement.y = jumpForce;
        }


        //This is why I needed the other vector3. If I just set it to one, then the player can float upon going up slopes. No one wants that. 
        move.y = playerJumpMovement.y;

        //Moves the player with the controller!
        controller.Move(move * speed * Time.deltaTime);

    }

}
