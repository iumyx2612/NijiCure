using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Movement stuff
    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private Rigidbody2D rb2D;
    private Vector2 movement;
    private Vector2 oldMovement;
    private bool isFacingRight = true;
    
    // Direction Indicator stuff
    [SerializeField] private GameObject directionIndicator; // The indicator
    [SerializeField] private float scaledDistanceFromPlayer; // Indicator dist from player
    private Transform dITransform; // Indicator transform to change it's position
    // Currently need this mapping cuz the indicator is not facing the right direction when start
    private readonly Dictionary<Vector2, Vector3> rotationMapping =
        new Dictionary<Vector2, Vector3>
        {
            {Vector2.right, new Vector3(0, 0, -90)},
            {Vector2.left, new Vector3(0, 0, 90)},
            {Vector2.up, new Vector3(0, 0, 0)},
            {Vector2.down, new Vector3(0, 0, 180)},
            {new Vector2(-1, 1), new Vector3(0, 0, 45)},
            {new Vector2(-1, -1), new Vector3(0, 0, 135)},
            {new Vector2(1, 1), new Vector3(0, 0, -45)},
            {new Vector2(1, -1), new Vector3(0, 0, -135)}
        };
    
    // For bullet stuff
    [SerializeField] private Vector2Variable playerPosRef; // Where to fire the bullet
    [SerializeField] private Vector2Variable playerDirectionRef; // Direction to fire the bullet
    
    // Const
    private float fixedDeltaTime; // For faster * Time.fixedDeltaTime
    
    private void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        
        dITransform = directionIndicator.GetComponent<Transform>();
    }

    private void Start()
    {
        Vector2 playerPos = gameObject.transform.localPosition;
        dITransform.position = playerPos + new Vector2(scaledDistanceFromPlayer, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerPosRef.Value = transform.position;
        // Handle inputs
        movement = new Vector2(x:Input.GetAxisRaw("Horizontal"),
            y:Input.GetAxisRaw("Vertical"));
        if (movement.x > 0 && !isFacingRight) // going right and facing left
        {
            Flip();
        }
        else if (movement.x < 0 && isFacingRight) // going left and facing right
        {
            Flip();
        }

        if (movement != Vector2.zero)
        {
            playerDirectionRef.Value = movement;
        }
        
        // If player moves, then the arrow moves
        Vector2 offset = movement - oldMovement;
        // Check if last movement diff from this movement
        // And this movement can't be zero (omit last movement frame if stay still)
        if (offset != Vector2.zero && movement != Vector2.zero)
        {
            Vector3 rotationAngle = rotationMapping[movement];
            directionIndicator.transform.rotation = Quaternion.Euler(rotationAngle);
            Vector2 playerPos = gameObject.transform.position;
            Vector2 dINewPos = playerPos + movement * scaledDistanceFromPlayer;
            directionIndicator.transform.position = dINewPos;
        }

        oldMovement = movement;

    }

    private void FixedUpdate()
    {
        // Handle physics
        rb2D.MovePosition(rb2D.position + movement * moveSpeed * fixedDeltaTime);
    }

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);

        isFacingRight = !isFacingRight;
    }
}
