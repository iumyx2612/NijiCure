using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

[RequireComponent(typeof (EdgeCollider2D))]
public class ChuaTeNuaBai : MonoBehaviour
{
    public ChuaTeNuaBaiData abilityData;
    private ChuaTeNuaBaiData baseData;

    [SerializeField] private Vector2Variable playerPosRef;
    
    // Runtime Data
    private float critChance; 
    private Vector2 baseScale;
    private SpriteRenderer spriteRenderer;
    private EdgeCollider2D selfCollider;
    private ParticleSystem _particleSystem;
    private float scale;
    private int damage;
    private float multiplier;
    
    private enum Direction
    {
        top,
        right,
        bottom,
        left
    }
    private readonly Dictionary<Direction, int> directionToRotationMapping = new Dictionary<Direction, int>
    {
        {Direction.bottom, 0},
        {Direction.right, 90},
        {Direction.top, 180},
        {Direction.left, 270}
    };


    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        selfCollider = gameObject.GetComponent<EdgeCollider2D>();
        selfCollider.isTrigger = true;
        _particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        // Set alpha back to default since we set it to 0
        Color temp = spriteRenderer.color;
        temp.a = 0.7f;
        spriteRenderer.color = temp;

        transform.position = playerPosRef.Value;

        int rotation = GetRotation();
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector2(scale, scale), 1.5f));
        sequence.Join(DOTween.To(ParticleTween, 0, 1, _particleSystem.main.duration));
        sequence.Append(spriteRenderer.DOFade(0f, 0.2f));
        sequence.OnComplete(ResetBullet);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= critChance)
            {
                multiplier = 2f;
            }
            else
            {
                multiplier = 1f;
            }
            other.GetComponent<EnemyCombat>().TakeDamage(damage, multiplier);
        }
    }
    public void LoadData(ChuaTeNuaBaiData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }
        abilityData = data;
        spriteRenderer.sprite = data.sprite;
        // Things can changes during runtime
        scale = data.currentScale;
        damage = data.currentDamage;
        critChance = data.currentCritChance;
    }

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        transform.localScale = baseScale;
        baseData.state = AbilityBase.AbilityState.cooldown;
        _particleSystem.Stop();
    }

    private int GetRotation()
    {
        int randomInt = Random.Range(1, 4);
        Direction direction = (Direction) randomInt;
        return directionToRotationMapping[direction];
    }

    public void ModifyDamage(int value)
    {
        damage += value;
    }

    private void ParticleTween(float duration)
    {
        if (!_particleSystem.isPlaying)
        {
            _particleSystem.Play();
        }
    }
}
