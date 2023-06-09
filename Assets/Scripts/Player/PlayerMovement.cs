using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerData playerData;
    
    // Movement stuff
    private float speed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 oldMovement; // To check if player has changed position
    private bool isFacingRight = true;
    private RuntimeAnimatorController animatorController;
    private Animator animator;
    
    // Direction stuff
    private bool directionLock;
    
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private Sprite[] arrowSprites;
    private Vector2 playerPos;
    private Vector2 arrowNewPos;
    private float distanceFromPlayer = 1f;
    private readonly Dictionary<Vector2, Vector3> rotationMapping =
        new Dictionary<Vector2, Vector3>
        {
            {Vector2.right, new Vector3(0, 0, 0)},
            {Vector2.left, new Vector3(0, 0, 180)},
            {Vector2.up, new Vector3(0, 0, 90)},
            {Vector2.down, new Vector3(0, 0, -90)},
            {new Vector2(-1, 1), new Vector3(0, 0, 135)},
            {new Vector2(-1, -1), new Vector3(0, 0, -135)},
            {new Vector2(1, 1), new Vector3(0, 0, 45)},
            {new Vector2(1, -1), new Vector3(0, 0, -45)}
        };
    
    // For things that need reference to player
    [SerializeField] private Vector2Variable playerPosRef; // Where the player is
    [SerializeField] private Vector2Variable playerDirectionRef; // Direction of the player

    [SerializeField] private Canvas playerUICanvas; // For player UI element
    

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        playerPosRef.Value = Vector2.zero;
        playerDirectionRef.Value = Vector2.right;
        directionIndicator.GetComponent<SpriteRenderer>().sprite = arrowSprites[0];
        rb.gravityScale = 0f;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Vector2 playerPos = transform.position;
        directionIndicator.transform.position = playerPos + new Vector2(distanceFromPlayer, 0);
    }

    private void OnEnable()
    {
        if (playerData != null)
        {
            LoadData(playerData);
        }
        
        animator.runtimeAnimatorController = animatorController;
    }

    // Update is called once per frame
    void Update()
    {
        playerUICanvas.transform.position = transform.position;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            directionLock = true;
            directionIndicator.GetComponent<SpriteRenderer>().sprite = arrowSprites[1];
        }
        playerPosRef.Value = transform.position;
        // Handle inputs
        movement = new Vector2(x:Input.GetAxisRaw("Horizontal"),
            y:Input.GetAxisRaw("Vertical"));
        
        // Animation
        animator.SetBool("isMoving", isMoving(movement));
        
        if (movement.x > 0 && !isFacingRight && !directionLock) // going right and facing left
        {
            Flip();
        }
        else if (movement.x < 0 && isFacingRight && !directionLock) // going left and facing right
        {
            Flip();
        }

        if (movement != Vector2.zero && !directionLock)
        {
            playerDirectionRef.Value = movement;
        }
        
        // If player moves, then the arrow moves
        Vector2 offset = movement - oldMovement;
        // Check if last movement diff from this movement
        // And this movement can't be zero (omit last movement frame if stay still)
        if (offset != Vector2.zero && movement != Vector2.zero && !directionLock)
        {
            Vector3 rotationAngle = rotationMapping[movement];
            directionIndicator.transform.rotation = Quaternion.Euler(rotationAngle);
            playerPos = gameObject.transform.position;
            arrowNewPos = playerPos + movement * distanceFromPlayer;
            directionIndicator.transform.position = arrowNewPos;
        }
        
        oldMovement = movement;

        if (Input.GetKeyUp(KeyCode.Z))
        {
            directionLock = false;
            directionIndicator.GetComponent<SpriteRenderer>().sprite = arrowSprites[0];
        }
    }
    
    private void FixedUpdate()
    {
        // Handle physics
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
    
    private bool isMoving(Vector2 movement)
    {
        if (movement != Vector2.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);

        isFacingRight = !isFacingRight;
    }

    public void LoadData(PlayerData data)
    {
        playerData = data;
        speed = data.speed;
        animatorController = data.animatorController;
    }
}
