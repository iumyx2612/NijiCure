using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemDau : MonoBehaviour
{
    private NemDauData baseData;
    
    // Data
    private int damage;
    private float critChance;
    private float multiplier;
    private float angle;
    private Vector2 scale;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(NemDauData _data, int angleIndex)
    {
        if (baseData == null)
        {
            baseData = _data;
        }

        damage = _data.currentDamage;
        critChance = _data.currentCritChance;
        scale = _data.scale;
        angle = _data.currentAngles[angleIndex];
    }
        
}
