using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    
    // Data
    private float speed;
    private int health;
    private int rank;
    private float critChance;
    private RuntimeAnimatorController animatorController;
    
    // Movement stuff
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 oldMovement; // To check if player has changed position
    private bool isFacingRight = true;
    
    // Arrow indicator
    [SerializeField] private GameObject arrowIndicator;
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
    
    // Combat stuff
    [SerializeField] private IntVariable currentHealth;
    [SerializeField] private GameEvent onPlayerKilled;
    [SerializeField] private IntGameEvent playerTakeDamage;
    private bool isAlive;

    // Animation stuff
    private Animator animator;
    
    // For things that need reference to player
    [SerializeField] private Vector2Variable playerPosRef; // Where the player is
    [SerializeField] private Vector2Variable playerDirectionRef; // Direction of the player
    [SerializeField] private BoolVariable hasChangePos;

    // Const
    private float fixedDeltaTime;
    
    
    private void Awake()
    {
        LoadData(playerData); // Maybe try different approach (?)
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
        fixedDeltaTime = Time.fixedDeltaTime;
        
        // Assign GameEvent Listener
        playerTakeDamage.AddListener(TakeDamage);
        onPlayerKilled.AddListener(Dead);
    }

    // Start is called before the first frame update
    private void Start()
    {
        Vector2 playerPos = gameObject.transform.position;
        arrowIndicator.transform.position = playerPos + new Vector2(distanceFromPlayer, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerPosRef.Value = transform.position;
        // Handle inputs
        movement = new Vector2(x:Input.GetAxisRaw("Horizontal"),
            y:Input.GetAxisRaw("Vertical"));
        
        // Animation
        animator.SetBool("isMoving", isMoving(movement));
        
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
            hasChangePos.Value = true;
        }
        else
        {
            hasChangePos.Value = false;
        }
        
        // If player moves, then the arrow moves
        Vector2 offset = movement - oldMovement;
        // Check if last movement diff from this movement
        // And this movement can't be zero (omit last movement frame if stay still)
        if (offset != Vector2.zero && movement != Vector2.zero)
        {
            Vector3 rotationAngle = rotationMapping[movement];
            arrowIndicator.transform.rotation = Quaternion.Euler(rotationAngle);
            Vector2 playerPos = gameObject.transform.position;
            Vector2 arrowNewPos = playerPos + movement * distanceFromPlayer;
            arrowIndicator.transform.position = arrowNewPos;
        }
        
        oldMovement = movement;
    }
    
    private void FixedUpdate()
    {
        // Handle physics
        rb.MovePosition(rb.position + movement * speed * fixedDeltaTime);
    }

    public void LoadData(PlayerData data)
    {
        playerData = data;
        speed = data.speed;
        health = data.health;
        rank = data.rank;
        critChance = data.critChance;
        animatorController = data.animatorController;
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
    
    // Called by GameEvent playerTakeDamage
    public void TakeDamage(int damage)
    {
        currentHealth.Value -= damage;
        if (currentHealth.Value <= 0)
        {
            onPlayerKilled.Raise();
        }
    }
    
    public void Dead()
    {
        isAlive = false;
        gameObject.SetActive(false);
    }
}
