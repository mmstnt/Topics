using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Skeleton2Animation skeleton2Animation;
    private GameObject player;

    [Header("����Ѽ�")]
    public GameObject skeleton2;
    public float MoveSpeed; // ���Ⲿ�ʳt��
     

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayer;
    public bool action;
    public float doing=0;
    public enum actionKind { move,call }
    public actionKind actionMode;
    private float moveDuring;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        skeleton2Animation = transform.Find("Ani").GetComponent<Skeleton2Animation>();
        direction = Random.Range(0, 2) * 2 - 1; // �H����l�Ƥ�V�]-1 �� 1�^
    }


    public void findPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");  // ��� Player ����
    }


    private void Update()
    {
        updateCharacterFacing();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        if (distanceToPlayer > 2 & doing==0)
        {
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            rb.velocity = new Vector2(direction * MoveSpeed * Time.deltaTime, rb.velocity.y); // �Ȧb X �b�W����

        }
        if (distanceToPlayer < 2)
        {
            doing = 1;
            direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            skeleton2Animation.Skeleton2attack();

        }     
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

     


     
}
