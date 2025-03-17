using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathSpellCast : MonoBehaviour
{
    public Transform attackSource;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("角色參數")]
    public GameObject bringerOfDeathSpell;
    public float spellInterval;
    public int spellMinCount;
    public int spellMaxCount;
    private int spellCount;
    private float time;
    
    public void Awake()
    {
        spellCount = Random.Range(spellMinCount, spellMaxCount+1);
    }

    private void OnEnable()
    {
        afterSceneLoadEvent.onEventRaised += onAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadEvent.onEventRaised -= onAfterSceneLoadEvent;
    }

    private void onAfterSceneLoadEvent()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (attackSource == null) return;
        if (time < 0) 
        {
            time = spellInterval;
            Vector3 vector = attackSource.position;
            vector.x += Random.Range(-15.0f, 15.0f);
            vector.y += 5.5f;
            GameObject bringerOfDeathSpellObject = Instantiate(bringerOfDeathSpell, vector,transform.rotation);
            bringerOfDeathSpellObject.GetComponent<AttackSource>().attackSource = this.attackSource;
            spellCount -= 1;
            if (spellCount <= 0) 
            {
                Destroy(this.gameObject);
            }
        }
        else 
        {
            time -= Time.deltaTime;
        }
    }
}
