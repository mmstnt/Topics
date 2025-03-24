using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkeletonAnimation ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;
    public LayerMask GroundLayer;

    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("����Ѽ�")]
    public GameObject skeleton2;
    public float moveTimeMin;
    public float moveTimeMax;
    public float cutDistance;
    public RaycastHit2D hit;
    public RaycastHit2D hitup;

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayer;
    public float distanceToPlayerY;
    public bool action;
    public enum actionKind { move, call, cut, wave }
    public actionKind actionMode;
    private float moveDuring;
    public bool isDead;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<SkeletonAnimation>();
        direction = Random.Range(0, 2) * 2 - 1; // �H����l�Ƥ�V�]-1 �� 1�^
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
        if (isDead || player == null) return;
        CliffTurn();
        updateCharacterFacing();
        characterAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        distanceToPlayerY = player.transform.position.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        if (isDead || player == null) return;
        move();
    }

    public void characterAction()
    {
        if (action) return;
        actionMode = (actionKind)Random.Range(0, 4);

        switch (actionMode)
        {
            case actionKind.move:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                moveDuring = Random.Range(moveTimeMin, moveTimeMax);
                break;
            case actionKind.call:
                if (distanceToPlayer < cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.Skeletoncall();
                break;

            case actionKind.cut:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.Skeletoncut();
                break;

            case actionKind.wave:
                if (distanceToPlayer > cutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.Skeletonwave();
                break;
        }
    }

    public void CliffTurn()
    {
        // �p��g�u����V
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        Vector3 rayDirectionup = transform.localScale.x * new Vector3(1f,0, 0);

        // �g�u���_�I
        Vector3 rayStart = transform.position + new Vector3(0, -1, 0);
        Vector3 rayStartup = transform.position + new Vector3(0, 0.3f, 0);
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // ø�s�g�u�]�Ω�ոաA�i���Ʈg�u�^
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red);  
        hitup = Physics2D.Raycast(rayStartup, rayDirectionup, 4, GroundLayer);
        Debug.DrawRay(rayStartup, rayDirectionup * 4, Color.green);


    }

    public void move()
    {
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(direction * character.speed * Time.deltaTime, rb.velocity.y);
            if ((hit.collider != null && physicsCheck.isGround) ||(hitup.collider != null && physicsCheck.isGround && distanceToPlayerY > 0.5))
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 3, 10), ForceMode2D.Impulse);
            } 
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0 || distanceToPlayer < cutDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        moveDuring -= Time.deltaTime;
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

    public void call()
    {
        int times = 1;
        for (int i = 0; i < times; i++)
        {
            Vector3 newPosition = transform.position;

            float x = Random.Range(-10f, 10f);
            newPosition.x = player.transform.position.x + x;
            newPosition.y = 10;
            GameObject skeleton2Object = Instantiate(skeleton2, newPosition, transform.rotation);
            skeleton2Object.GetComponent<AttackSource>().attackSource = this.transform;
        }
    }
    public void SkeletonDead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }

}