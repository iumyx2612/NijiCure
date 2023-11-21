using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoImage : MonoBehaviour
{
    [HideInInspector] public AbilityBase ability;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TMP_Text abilitySummary;
    [SerializeField] private TMP_Text abilityName;

    public void SetUp(AbilityBase _ability)
    {
        ability = _ability;
        abilityIcon.sprite = _ability.abilityIcon;
        abilitySummary.text = _ability.summary;
        abilityName.text = _ability.abilityName;
    }
}
