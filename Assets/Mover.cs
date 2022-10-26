using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    // reference this in the Inspector if possible
    [SerializeField] private Rigidbody2D rigidbody;

    // keep track of the target rotation
    // for Rigidbody2D this is a simple float for the rotation angle
    private float angle = 0;

    private void Awake()
    {
        // as fallback get the Rigidboy2D on runtime
        if(!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
        
    }

    private void Update ()
    {      
        // Still get user input in Update to not miss a frame

        // evtl you would need to swap the sign according to your needs
        angle += Time.deltaTime * rotationSpeed * Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        // rotate the Rigidbody applying the configured interpolation
        rigidbody.MoveRotation(angle);
        speed = GameManager.speed;
        // assuming if the object is not rotated you want to go right
        // otherwise simply change this vector
        rigidbody.velocity = rigidbody.GetRelativeVector(Vector3.down).normalized * speed;
    }


}
