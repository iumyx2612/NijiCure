using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deo : MonoBehaviour
{
    private DeoData baseData;

    private int stackPerSec;
    private int maxStacks;
    private float buffPercent;
    private float slowPercent;
    private DamageAbilityBase startingAbility;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(DeoData data)
    {
        if (baseData == null)
        {
            baseData = data;
        }

    }
}
