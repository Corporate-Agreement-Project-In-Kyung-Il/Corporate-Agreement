using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        CombatSystem.instance.Events.OnCombatEvent += PrintDamageText;
    }

    private void PrintDamageText(CombatEvent combatEvent)
    {
        if ((combatEvent.Receiver is MonsterController).Equals(false))
            return;
        
        float textDuration = 1.0f;
        string damageString = Mathf.FloorToInt(combatEvent.Damage).ToString();
        
        DamageText damageText = ObjectPoolSystem.Instance.GetObjectOrNull("DamageText") as DamageText;

        
        damageText.transform.position = combatEvent.Receiver.GameObject.transform.position;
        damageText.gameObject.SetActive(true);
        damageText.Set(combatEvent.Receiver.mainCollider.transform,
            damageString,
            textDuration,
            Color.red);
    }
    private void OnDisable()
    {
        CombatSystem.instance.Events.OnCombatEvent -= PrintDamageText;
    }

    private void OnDestroy()
    {
        CombatSystem.instance.Events.OnCombatEvent -= PrintDamageText;
    }
}
