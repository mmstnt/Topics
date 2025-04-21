using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cthulu : MonoBehaviour
{
    public Rigidbody2D rb;
    private CthuluAni ani;
    private GameObject player;
    private Character character;

    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("角色參數")]
    public GameObject CthuluSkill01Cast;
    public GameObject CthuluBullet01Cast;
    public GameObject CthuluSkill02;
    public float moveTimeMin;
    public float moveTimeMax;
    public float attackMinDistance;
    public float attackMaxDistance;
    public int skill02MinCount;
    public int skill02MaxCount;

    [Header("角色狀態")]
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, attack01, attack02, attack03 }
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        ani = transform.Find("Ani").GetComponent<CthuluAni>();
    }

    private void OnEnable()
    {
        afterSceneLoadEvent.onEventRaised += onAfterSceneLoadEvent;
        cameraLensEvent.onEventRaised += onCameraLensEvent;
    }

    private void OnDisable()
    {
        afterSceneLoadEvent.onEventRaised -= onAfterSceneLoadEvent;
        cameraLensEvent.onEventRaised -= onCameraLensEvent;
    }

    private void onAfterSceneLoadEvent()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    private void onCameraLensEvent()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        if (character.isDead || player == null) return;
        characterAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        if (character.isDead || player == null) return;
        move();
    }

    public void characterAction()
    {
        if (action) return;
        if (actionList.Count == 0)
        {
            getActionMode();
            return;
        }
        actionMode = actionList[0];
        actionList.RemoveAt(0);
        switch (actionMode)
        {
            case actionKind.move:
                action = true;
                getPlayerSite();
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.attack01:
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack01();
                break;
            case actionKind.attack02:
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack02();
                break;
            case actionKind.attack03:
                action = true;
                rb.velocity = Vector2.zero;
                getPlayerSite();
                ani.isAttack03();
                break;
        }
    }

    private void getActionMode()
    {
        int mode = Random.RandomRange(0, 4);
        switch (mode)
        {
            case 0:
                actionList.Add(actionKind.move);
                break;
            case 1:
                actionList.Add(actionKind.attack01);
                break;
            case 2:
                actionList.Add(actionKind.attack02);
                break;
            case 3:
                actionList.Add(actionKind.attack03);
                break;
        }
    }

    private void getPlayerSite()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    private void move()
    {
        if (actionMode != actionKind.move)
            return;
        rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y);
        if (moveDuring < 0)
        {
            action = false;
        }
        moveDuring -= Time.deltaTime;
    }

    public void skill01() 
    {
        GameObject skill01Cast = Instantiate(CthuluSkill01Cast,transform.position,transform.rotation);
        skill01Cast.transform.GetComponent<CthuluSkill01Cast>().attackSource = this.transform;
        GameObject bullet01Cast = Instantiate(CthuluBullet01Cast, transform.position, transform.rotation);
        bullet01Cast.transform.GetComponent<CthuluBullet01Cast>().attackSource = this.transform;
    }

    public void skill02() 
    {
        int skill02Count = Random.Range(skill02MinCount, skill02MaxCount + 1);
        Vector2 vector = transform.position;
        vector.x += transform.localScale.x * 6;
        for(int i = 0; i < skill02Count; i++) 
        {
            GameObject skill02 = Instantiate(CthuluSkill02, vector, transform.rotation);
            skill02.transform.GetComponent<AttackSource>().attackSource = this.transform;
            skill02.transform.GetComponent<CthuluSkill02>().isThrow(transform.localScale.x);
        }
    }
}
