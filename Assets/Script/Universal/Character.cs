using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("�ƥ��ť")]
    public VoidEventSO newGameEvent;
    [Header("�ݩ�")]
    public float maxHp;
    public float currentHp;
    public float damage;
    [Header("�L��")]
    public bool invulnerable;
    public float invulnerableTime;
    public float invulnerableDuration;
    [Header("�ƥ�")]
    public UnityEvent<Character> onHealthChange;
    public UnityEvent<Transform> onTakeDamage;
    public UnityEvent onDead;
    public UnityEvent<Transform> onHitEvent;


    private void newGame()
    {
        currentHp = maxHp;
        onHealthChange?.Invoke(this);
        onHitEvent = new UnityEvent<Transform>();
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += newGame;
        ISaveable saveable = this;
        saveable.registerSaveDate();
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= newGame;
        ISaveable saveable = this;
        saveable.unregisterSaveDate();
    }

    private void Update()
    {
        if (invulnerable) 
        {
            invulnerableDuration -= Time.deltaTime;
            if (invulnerableDuration <= 0) 
            {
                invulnerable = false;
            }
        }
    }

    public void takeDamage(Attack attacker) 
    {
        if (invulnerable || currentHp == 0) 
            return;
        onHitEvent?.Invoke(this.transform);
        float attackDamage = (attacker.damageSource.damage * attacker.damageRatio) + (attacker.damageBasic) + (attacker.damagePercentage * maxHp);
        if (currentHp - attackDamage > 0) 
        {
            //����L��
            currentHp -= attackDamage;
            triggerInvulnerable();
            //�������
            onTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            currentHp = 0;
            //���`
            onDead?.Invoke();
        }
        onHealthChange?.Invoke(this);
    }

    
    private void triggerInvulnerable() 
    {
        if (!invulnerable) 
        {
            invulnerableDuration = invulnerableTime;
            invulnerable = true;
        }
    }

    public DataDefinition getDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void getSaveDate(Data data)
    {
        if (data.characterPosDict.ContainsKey(getDataID().ID))
        {
            data.characterPosDict[getDataID().ID] = transform.position;
            data.floatSaveData[getDataID().ID + "health"] = this.currentHp;
        }
        else 
        {
            data.characterPosDict.Add(getDataID().ID, transform.position);
            data.floatSaveData.Add(getDataID().ID + "health", this.currentHp);
        }
    }

    public void loadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(getDataID().ID)) 
        {
            transform.position = data.characterPosDict[getDataID().ID];
            this.currentHp = data.floatSaveData[getDataID().ID + "health"];

            //�q��UI��s
            onHealthChange?.Invoke(this);
        }
    }
}
