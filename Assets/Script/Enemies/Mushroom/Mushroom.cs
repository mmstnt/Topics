using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D rb;
    private MushroomAnimation ani;
    private GameObject player;
    private Character character;
    private PhysicsCheck physicsCheck;
    public LayerMask GroundLayer;

    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO cameraLensEvent;

    [Header("����Ѽ�")]
    public GameObject spore;
    public GameObject spore2;
    public float moveTimeMin;
    public float moveTimeMax;
    public float biteDistance;
    public RaycastHit2D hit;

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayerX;
    public float distanceToPlayerY;
    public bool action;
    public bool isDead;
    public enum actionKind {move, controlMushroom,bite, throwSpore }
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        character = transform.GetComponent<Character>();
        physicsCheck = GetComponent<PhysicsCheck>();
        ani = transform.Find("Ani").GetComponent<MushroomAnimation>();
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
        distanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
        distanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
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
            case actionKind.controlMushroom:
                if (distanceToPlayerX < biteDistance)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.attack1();
                break;
            case actionKind.bite:
                if (distanceToPlayerX > biteDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.attack2();
                break;
            case actionKind.throwSpore:
                if (distanceToPlayerX < biteDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                ani.attack3();
                break;
        }
    }

    public void ThrowSpore()
    {
        int times = 3;
        for (int i = 0; i < times; i++)
        {
            GameObject sporeObject = Instantiate(spore, transform.position, transform.rotation);
            sporeObject.GetComponent<AttackSource>().attackSource = this.transform;
            sporeObject.GetComponent<Spore>().attack3(direction);
        }
    }

    public void ControlMushroom()
    {
        Vector3 newPosition = transform.position;
        Vector3 newPosition2 = transform.position;
        newPosition.x = 1;
        newPosition.y = 1;
        newPosition2.x = 28;
        newPosition2.y = 0.5f;
        int times = 3;
        for (int i = 0; i < times; i++)
        {
            GameObject spore2Object = Instantiate(spore2, newPosition, transform.rotation);
            spore2Object.GetComponent<AttackSource>().attackSource = this.transform;
            spore2Object.GetComponent<Spore2>().attack3(direction);
        }

        for (int i = 0; i < times; i++)
        {
            GameObject spore2Object = Instantiate(spore2, newPosition2, transform.rotation);
            spore2Object.GetComponent<AttackSource>().attackSource = this.transform;
            spore2Object.GetComponent<Spore2>().attack3(direction);
        }
    }


    public void move()
    {
        if (hit.collider != null&& physicsCheck.isGround&& distanceToPlayerY < 1)
            direction = direction * -1;
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {
            rb.velocity = new Vector2(direction * character.speed * Time.deltaTime, rb.velocity.y); // �Ȧb X �b�W����
            if (hit.collider != null && physicsCheck.isGround && distanceToPlayerY>1)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 3, 20), ForceMode2D.Impulse);
            }
            if (moveDuring < 0)
            {
                action = false;
            }
        }
        else if (moveDuring < 0 )
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

    public void CliffTurn()
    {
        // �p��g�u����V
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // �g�u���_�I
        Vector3 rayStart = transform.position + new Vector3(0, -1, 0);
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // ø�s�g�u�]�Ω�ոաA�i���Ʈg�u�^
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red);

    }    


    public void MushroomDead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
