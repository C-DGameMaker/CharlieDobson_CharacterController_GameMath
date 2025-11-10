using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControlsScript : MonoBehaviour
{
    //Our character controller attached to the player
    private CharacterController controller;

    //Floats of player stuff
    public float speed = 5f;
    public float jumpForce = 5f;
    public float gravity = 1f;

    //Axises
    static float xAxis;
    static float yAxis;
    static float zAxis;

    void Start()
    {
        //Get the charatcer controller from the player body
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        
        Vector3 move = new Vector3(xAxis, yAxis, zAxis);
        controller.Move(move * speed * Time.deltaTime);
    }

}
