using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Leveling/Exp Data")]
public class ExpPickerData : ScriptableObject
{
    public int type;
    public int exp;
    public Sprite sprite;
}
