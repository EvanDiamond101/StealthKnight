﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator knightAnimator;

    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private float rotationSpeed = 0.0f;
    [SerializeField] private float maxWalkSpeed = 0.0f;
    [SerializeField] private float maxSprintSpeed = 0.0f;
    private float currentMaxSpeed = 0.0f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        setCurrentMaxSpeed();

        setVelocityComponent(ref velocity.x, Input.GetAxis("Horizontal"));
        setVelocityComponent(ref velocity.z, Input.GetAxis("Vertical"));

        if (velocity.magnitude > currentMaxSpeed)
        {
            velocity.Normalize();
            velocity *= maxWalkSpeed;
        }

        if (Input.GetAxis("Horizontal") != 0)
            velocity.x *= Mathf.Abs(Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Vertical") != 0)
            velocity.y *= Mathf.Abs(Input.GetAxis("Vertical"));

        knightAnimator.SetFloat("Walk Speed", velocity.magnitude);

        velocity.y = GetComponent<Rigidbody>().velocity.y;
        GetComponent<Rigidbody>().velocity = velocity;

        if (!(Mathf.Abs(velocity.x) <= 0.2 && Mathf.Abs(velocity.z) <= 0.2))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(velocity),
                Time.fixedDeltaTime * rotationSpeed
            );
        }
    }

    private void setCurrentMaxSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button1))
        {
            knightAnimator.SetBool("Sprinting", true);
            currentMaxSpeed = maxSprintSpeed;
        }
        else
        {
            knightAnimator.SetBool("Sprinting", false);
            currentMaxSpeed = maxWalkSpeed;
        }
    }

    private void setVelocityComponent(ref float component, float inputAxis)
    {
        if (inputAxis != 0)
        {
            component += moveSpeed * inputAxis * Time.fixedDeltaTime;
        }
        else
        {
            if (component > 0)
            {
                component -= moveSpeed * Time.fixedDeltaTime;
                if (component < 0)
                    component = 0;
            }
            else if (component < 0)
            {
                component += moveSpeed * Time.fixedDeltaTime;
                if (component > 0)
                    component = 0;
            }
        }
    }
}