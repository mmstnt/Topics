using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firemancer : MonoBehaviour
{
    private Rigidbody2D rb;
    private FiremancerAnimation firemancerAnimation;
    private GameObject player;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;

    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public GameObject fire;
    public GameObject fire2;
    public float MoveSpeedX; // ���Ⲿ�ʳt��
    public float MoveSpeedY;
    public float moveTimeMin;
    public float moveTimeMax;
    public float spoutDistance;
    public float flyDistance;
    public RaycastHit2D up;
    public RaycastHit2D back;
    public float y;


    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayerX;
    public float distanceToPlayerY;
    public bool action;
    public bool isDead;
    public enum actionKind { move, fire,fire2,spout }
    public actionKind actionMode;
    private float moveDuring;
    public float upDuring;
    public float downDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        firemancerAnimation = transform.Find("Ani").GetComponent<FiremancerAnimation>();
        player = GameObject.Find("Game/Player");
        direction = Random.Range(0, 2) * 2 - 1; // �H����l�Ƥ�V�]-1 �� 1�^
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
        updateCharacterFacing();
        firemancerAction();
        distanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
        distanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
    }

    public void dofire()
    {
            // �p���H����m
            Vector3 newPosition = transform.position;
            float x = Random.Range(-5f, 5f);
            newPosition.x = player.transform.position.x + x;
            newPosition.y = player.transform.position.y + 5;

            // �ͦ� Fire ����
            GameObject fireGameObject = Instantiate(fire, newPosition, transform.rotation);
            fireGameObject.GetComponent<AttackSource>().attackSource = this.transform;
    }


    public void dofire2()
    {
        // �p���H����m
        Vector3 newPosition = transform.position;
        
        newPosition.x = player.transform.position.x + y * direction;
        newPosition.y = player.transform.position.y + 5;
        y = y + 10;
        GameObject fire2GameObject = Instantiate(fire2, newPosition, transform.rotation);
        fire2GameObject.GetComponent<AttackSource>().attackSource = this.transform;
        if (y > 20)
        {
            y = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        move();
    }


    public void CliffTurn()
    {
        // �p��g�u����V
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // �g�u���_�I
        Vector3 rayStart = transform.position + new Vector3(0, -1.5f, 0);
        up = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        back = Physics2D.Raycast(rayStart, rayDirection, 3, WallLayer);
        // ø�s�g�u�]�Ω�ոաA�i���Ʈg�u�^
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red); // ����g�u�A���׬� 2
    }


    public void move()
    {
        if (back.collider != null)
            direction = direction * -1;
        if (actionMode != actionKind.move)
            return;
        if (actionMode == actionKind.move)
        {

            if (up.collider != null)
            {

                MoveSpeedY = 300;
            }
            if (up.collider == null)
            {
                if (upDuring < 0)
                {
                    upDuring = Random.Range(0.5f, 1);
                    if (flyDistance > distanceToPlayerX)
                    {
                        MoveSpeedY = Random.Range(-150, 150);
                    }
                    else if (flyDistance < distanceToPlayerX)
                    {
                        MoveSpeedY = Random.Range(-100, -300);
                    }
                }
                upDuring -= Time.deltaTime;
            }

            rb.velocity = new Vector2(direction * MoveSpeedX * Time.deltaTime, MoveSpeedY * Time.deltaTime);  
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


    public void firemancerAction()
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

            case actionKind.fire:
                if (distanceToPlayerX < spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                firemancerAnimation.Fire();
                break;

            case actionKind.fire2:
                if (distanceToPlayerX < spoutDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                firemancerAnimation.Fire2();
                break;

            case actionKind.spout:
                if (distanceToPlayerX > spoutDistance && distanceToPlayerY > 1)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                firemancerAnimation.Spout();
                break;
        }

    }
    public void fireDead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
    }
}
