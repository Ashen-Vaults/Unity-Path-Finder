using UnityEngine;
using System.Collections;

public class PlayerMovementTemp : MonoBehaviour {

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;

    void Update()
    {
        // is the controller on the ground?
        //if (controller.isGrounded)
        //{
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping

      //  }
        //Applying gravity to the controller       //Making the character move
        controller.Move(moveDirection * Time.deltaTime);
    }
}