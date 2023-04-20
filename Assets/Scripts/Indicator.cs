using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Indicator : MonoBehaviour
{
    public GameObject arrowIndicator;
    public GameObject centerPoint;
    public Vector2 oldMovement;
    
    [SerializeField] private Rigidbody2D rb2D;

    public Vector2 movement;

    public float moveSpeed;

    public readonly Dictionary<Vector2, Vector3> rotationMapping =
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // If player moves, then the arrow moves
        Vector2 offset = movement - oldMovement;

        // Check if last movement diff from this movement
        // And this movement can't be zero (omit last movement frame if stay still)
        if (offset != Vector2.zero && movement != Vector2.zero)
        {
            Vector3 rotationAngle = rotationMapping[movement];
            arrowIndicator.transform.rotation = Quaternion.Euler(rotationAngle);
            Vector2 playerPos = gameObject.transform.position;
            Vector2 dINewPos = playerPos + movement * 1f;
            arrowIndicator.transform.position = dINewPos;
        }
        oldMovement = movement;
    }

    private void FixedUpdate()
    {
        // Handle physics
        rb2D.MovePosition(rb2D.position + movement * Time.fixedDeltaTime * moveSpeed);
    }
}
