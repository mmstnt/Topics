using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Skeleton2Animation skeleton2Animation;
    private GameObject player;
    public LayerMask GroundLayer;
    private PhysicsCheck physicsCheck;

    [Header("����Ѽ�")]
    public float MoveSpeed; // ���Ⲿ�ʳt��
    public RaycastHit2D hit;

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayer;
    public bool action;
    public bool isDead;
    public enum actionKind { move,call }
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        physicsCheck = GetComponent<PhysicsCheck>();
        rb = transform.GetComponent<Rigidbody2D>();
        skeleton2Animation = transform.Find("Ani").GetComponent<Skeleton2Animation>();
        direction = Random.Range(0, 2) * 2 - 1; // �H����l�Ƥ�V�]-1 �� 1�^
        player = GameObject.FindGameObjectWithTag("Player");  // ��� Player ����
        isDead = false;
    }

    private void Update()
    {
        CliffTurn();
        updateCharacterFacing();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        if (isDead) return;
        if (distanceToPlayer > 2)
        {
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // �Ȧb X �b�W����
            if (   physicsCheck.isGround& hit.collider != null)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 3, 20), ForceMode2D.Impulse);
            }
        }
        else
        {
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            skeleton2Animation.Skeleton2attack();
            isDead = true;
        }     
    }

    public void CliffTurn()
    {
        // �p��g�u����V
        Vector3 rayDirection = transform.localScale.x * new Vector3(1f, 0, 0);
        // �g�u���_�I
        Vector3 rayStart = transform.position;
        hit = Physics2D.Raycast(rayStart, rayDirection, 3, GroundLayer);
        // ø�s�g�u�]�Ω�ոաA�i���Ʈg�u�^
        Debug.DrawRay(rayStart, rayDirection * 3, Color.red); // ����g�u�A���׬� 2
    }

    public void updateCharacterFacing()
    {
        if (isDead) return;
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
}
