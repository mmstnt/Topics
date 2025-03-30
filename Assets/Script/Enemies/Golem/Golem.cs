using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Golem : MonoBehaviour
{
    private Rigidbody2D rb;
    private GolemAnimation ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;
    private Transform golemTransform;

    public GameObject aniGameObject;
    public LayerMask GroundLayer;

    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("����Ѽ�")]
    public GameObject laser;
    public GameObject rock;
    public float moveTimeMin;
    public float moveTimeMax;
    public RaycastHit2D back;

    [Header("���⪬�A")]
    public float distanceToPlayerY1;
    public float distanceToPlayerY2;
    public bool action;
    public enum actionKind { move, transmit ,shoot,laser}
    public actionKind actionMode;
    public List<actionKind> actionList;
    private float moveDuring;

    private void Awake()
    {
        golemTransform = this.transform;
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<GolemAnimation>();
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
        CliffTurn();
        jump();
        characterAction();
        distanceToPlayerY1 = Mathf.Abs(player.transform.position.y - transform.position.y);
        distanceToPlayerY2 = player.transform.position.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        if (character.isDead || player == null) return;
        move();
    }

    public void characterAction()
    {
        if (action || !physicsCheck.isGround) return;
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
            case actionKind.transmit:
                if (distanceToPlayerY1 < 0.5f)
                    break;
                action = true;
                getPlayerSite();
                ani.Golemtransmit();
                break;
            case actionKind.shoot:
                if (distanceToPlayerY1 > 0.5f)
                    break;
                action = true;
                getPlayerSite();
                ani.Golemshoot();
                break;
            case actionKind.laser:
                if (distanceToPlayerY1 > 0.5f)
                    break;
                action = true;
                getPlayerSite();
                ani.Golemlaser();
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
                actionList.Add(actionKind.transmit);
                break;
            case 2:
                actionList.Add(actionKind.shoot);
                break;
            case 3:
                actionList.Add(actionKind.laser);
                break;
        }
    }

    private void getPlayerSite()
    {
        float dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    public void transmit()
    {
        aniGameObject.GetComponent<SpriteRenderer>().enabled = false;
        golemTransform = this.transform;

       float randomY = player.transform.position.y + 0.2f;

        float randomX = player.transform.position.x + Random.Range(-3.0f, 3.0f); // �H���ͦ� -20 �� 20 ���ƭ�
        Vector3 newPosition = new Vector3(randomX, randomY, golemTransform.position.z); // �O�� y �M z ����
        golemTransform.position = newPosition; // ��s��m
    }

    public void shootlaser()
    {
        GameObject laserObject = Instantiate(laser, transform.position, transform.rotation);
        laserObject.GetComponent<AttackSource>().attackSource = this.transform;
        laserObject.GetComponent<Laser>().site = this.transform;
    }  

    public void shootrock()
    {
        GameObject rockObject = Instantiate(rock, transform.position, transform.rotation);
        rockObject.GetComponent<AttackSource>().attackSource = this.transform;
        rockObject.transform.localScale = this.transform.localScale;
    }

    public void move()
    {
        if (back.collider != null)
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(transform.localScale.x * character.speed * Time.deltaTime, rb.velocity.y); // �Ȧb X �b�W����
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        moveDuring -= Time.deltaTime;
    }

    public void CliffTurn()
    {
        // �p��g�u����V
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // �g�u���_�I
        Vector3 rayStart = transform.position + new Vector3(0, 1, 0);
        back = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // ø�s�g�u�]�Ω�ոաA�i���Ʈg�u�^
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red);
    }

    private void jump()
    {
        if (physicsCheck.isGround)
        {
            aniGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
