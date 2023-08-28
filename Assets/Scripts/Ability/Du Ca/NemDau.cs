using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;
using AngleCalculation;

public class NemDau : MonoBehaviour
{
    private NemDauData baseData;
    public NemDauData data;
    
    // Data
    private int damage;
    private float critChance;
    private float multiplier;
    private Vector2 scale;
    private Vector2 baseScale;
    private Vector2 direction;
    private Vector2 defaultPos;
    private Vector2 curDist;

    
    // Put in Awake
    private Rigidbody2D rb;

    [SerializeField] private Vector2Variable directionRef;
    [SerializeField] private Vector2Variable playerPosRef;
    [SerializeField] private Vector2 maxDist;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float angle;
    [SerializeField] private float rotationSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseScale = transform.localScale;
        if (data != null)
        {
            LoadData(data);
        }
    }
    
    private void OnEnable()
    {
        // Scale
        transform.localScale = baseScale * scale;
        // Position
        transform.position = playerPosRef.Value;
        defaultPos = playerPosRef.Value;
        // Calculate direction
        int mult = 1;
        if (directionRef.Value.x < 0)
        {
            mult = -1;
        }
        else if (directionRef.Value.x == 0)
        {
            mult = 0;
        }
        direction = AngleCal.DegreeToVector2(directionRef.Value, angle * mult);
        // Pick force
        float force = Random.Range(minForce, maxForce);
        // Apply force
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0 ,0, rotationSpeed * Time.deltaTime);
        Vector2 dist = DistCal(defaultPos, transform.position);
        if (dist.x >= maxDist.x || dist.y >= maxDist.y)
        {
            ResetBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= critChance)
            {
                multiplier = 1.5f;
            }
            else
            {
                multiplier = 1f;
            }
            collider.GetComponent<EnemyCombat>().TakeDamage(damage, multiplier, Vector2.zero, 0f);
        }
    }

    public void LoadData(NemDauData _data)
    {
        if (baseData == null)
        {
            baseData = _data;
        }

        damage = _data.currentDamage;
        critChance = _data.currentCritChance;
        scale = _data.currentScale;
    }

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        baseData.state = AbilityBase.AbilityState.cooldown;
    }

    private Vector2 DistCal(Vector2 defaultPos, Vector2 currentPos)
    {
        float xDist = Mathf.Sqrt(Mathf.Pow(defaultPos.x - currentPos.x, 2));
        float yDist = Mathf.Sqrt(Mathf.Pow(defaultPos.y - currentPos.y, 2));
        curDist = new Vector2(xDist, yDist);
        return curDist;
    }

    private void Spin()
    {
        
    }
}
