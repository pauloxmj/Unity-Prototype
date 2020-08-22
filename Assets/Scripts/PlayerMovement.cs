using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    // Character movement speed modifier
    public float moveSpeed = 5f;

    // Creating vars to reference Vector3, RB and camera
    private Vector3 _movement;
    private Rigidbody _rb;
    [SerializeField]private Camera mainCamera;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Assigning the 
        _rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        // Creating a --new-- instance of a Vector3 object and assigning input axes
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0,Input.GetAxisRaw("Vertical"));
        MouseLook();
    }

    // FixedUpdate is called in a fixed interval regardless of framerate
    private void FixedUpdate()
    {
        // Movement
        // Call the MoveCharacter method and use the _movement values to forward instructions to the movementDirection parameter in the Method
        MoveCharacter(_movement);
    }

    void MouseLook()
    {
        // Creating a Ray (line) from the position of the mainCamera to the direction the mouse is pointing to
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Creating a Virtual plane that the Ray can intersect with so we can make the player look at the intersection point
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); //Making a plane that's facing Up and setting it to the origin/start/zero position

        // Casting/sending the ray we created at the mainCamera position to the groundPlane
        if (groundPlane.Raycast(cameraRay, out float rayLength)) // This float will be used to tell the engine the distance between the mainCamera and the groundPlane
        {
            //Create a variable of type Vector3 called pointToLook to store the transform position where the cameraRay is intersecting with the groundPlane
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            
            // Debug to visualize the Ray in the editor window while play mode is on
            // Debug.DrawLine(cameraRay.origin, pointToLook, Color.green);
            
            // Transform the player rotation so the forward direction points to the cameraRay intersection with the groundPlane
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            // We create a new Vector3 object so the player always looks forward at the same level using it's own y transform position instead of looking at the ground
        }
    }
    
    void MoveCharacter(Vector3 movementDirection)
    {
        // Use MovePosition to transform the position of the RB in the movementDirection and multiplies it by the moveSpeed and smooth it with Time.deltaTime
        _rb.MovePosition(transform.position + movementDirection * (moveSpeed * Time.deltaTime));
    }
}
