using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidBody;
    
    private Vector3 playerMovementInput;
    private Vector2 playerLookInput;
    private Vector2  playerRot;
    private float mouseSensitivityMultiplier = 1;
    private Vector3 checkFrontPosTop;
    private Vector3 checkFrontPosBottom;
    private const float checkFrontRadius = 0.25f;
    private const float checkFrontDist = 0.5f;

    public Transform feetTransform;
    public LayerMask floorMask;
    public float moveSpeed = 20f;
    public float jumpForce = 2f;
    public Transform playerCamera;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        rigidBody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var rotEuler = transform.localRotation.eulerAngles;
        playerRot = new Vector2(rotEuler.x, rotEuler.y);

        if(Application.isEditor){
            mouseSensitivityMultiplier = 4;
        }
    }

    void FixedUpdate()
    {
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerLookInput = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        MovePlayer();
        LookPlayer();
    }

    private void MovePlayer()
    {       
        var moveVector = transform.TransformDirection(playerMovementInput);

        var dirVector = moveVector.normalized;
        dirVector *= checkFrontDist+checkFrontRadius;

        checkFrontPosTop = transform.position + dirVector + new Vector3(0f, +0.5f, 0f);
        checkFrontPosBottom = checkFrontPosTop + new Vector3(0f, -1f, 0f);

        if(!Physics.CheckCapsule(checkFrontPosBottom, checkFrontPosTop, checkFrontRadius, floorMask)){         
            var moveVectorVelocity = moveVector*moveSpeed;
            rigidBody.velocity = new Vector3(moveVectorVelocity.x, rigidBody.velocity.y, moveVectorVelocity.z);
        }

        if (Input.GetButton("Jump")){
            if(Physics.CheckSphere(feetTransform.position, 0.2f, floorMask)){
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void LookPlayer(){
        playerRot += playerLookInput * mouseSensitivity * mouseSensitivityMultiplier * Time.fixedDeltaTime;

        playerRot.x = Mathf.Clamp(playerRot.x, -clampAngle, clampAngle);

        rigidBody.rotation = Quaternion.Euler(new Vector3(0f, playerRot.y, 0f));
        playerCamera.localRotation = Quaternion.Euler(new Vector3(playerRot.x, 0f, 0f));
    }
}
