using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private GolemAnimation golemAnimation;
    private GameObject player;
    private Transform golemTransform;
    private float timer;
    private float moveDuring;
    public GameObject ani;
    private PhysicsCheck physicsCheck;
    public LayerMask GroundLayer;

    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public GameObject laser;
    public GameObject rock;
    public float MoveSpeed; // ���Ⲿ�ʳt��
    public float moveTimeMin;
    public float moveTimeMax;
    public RaycastHit2D back;

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayerY1;
    public float distanceToPlayerY2;
   
    public bool action;
    public bool isDead;
    public enum actionKind { move, transmit ,shoot,laser}
    public actionKind actionMode;

    private void Awake()
    {
        golemTransform = this.transform;
        rb = transform.GetComponent<Rigidbody2D>();
        golemAnimation = transform.Find("Ani").GetComponent<GolemAnimation>();
        direction = Random.Range(0, 2) * 2 - 1; // �H����l�Ƥ�V�]-1 �� 1�^
        physicsCheck = GetComponent<PhysicsCheck>();
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
        player = GameObject.FindGameObjectWithTag("Player");  // ��� Player ����
    }

    private void Update()
    {
        if (isDead) return;
        CliffTurn();
        jump();
        updateCharacterFacing();
        mushroomAction();
        distanceToPlayerY1 = Mathf.Abs(player.transform.position.y - transform.position.y);
        distanceToPlayerY2 = player.transform.position.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        move();
    }
    public void transmit()
    {
        ani.GetComponent<SpriteRenderer>().enabled = false;
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
        rockObject.GetComponent<Rock>().Move(direction);
    }


    public void move()
    {
        if (back.collider != null)
            direction = direction * -1;
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // �Ȧb X �b�W����
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

    public void updateCharacterFacing()
    {
        // �ھڲ��ʤ�V���ܨ��⪺���V
        if (direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ���V��
        }
        else if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1); // ���V�k
        }
    }

    private void jump()
    {
        if (physicsCheck.isGround)
        {
            ani.GetComponent<SpriteRenderer>().enabled = true;
        }

    }
        public void mushroomAction()
    {
        if (action || !physicsCheck.isGround) return;
        actionMode = (actionKind)Random.Range(0, 4);

        switch (actionMode)
        {

            case actionKind.move:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
             
            case actionKind.transmit:
                if (distanceToPlayerY1 < 0.5f)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemtransmit();
                break;

            case actionKind.shoot:
                if (distanceToPlayerY1 > 0.5f)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemshoot();
                break;
            
            case actionKind.laser:
                if (distanceToPlayerY1 > 0.5f)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemlaser();
                break;







        }
    }
    public void GolemDead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
