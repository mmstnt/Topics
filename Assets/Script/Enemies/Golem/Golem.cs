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

    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public GameObject laser;
    public GameObject rock;
    public float MoveSpeed; // ���Ⲿ�ʳt��
    public float moveTimeMin;
    public float moveTimeMax;

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayer;
    public bool action;
    public enum actionKind { move, transmit ,shoot,laser}
    public actionKind actionMode;

    private void Awake()
    {
        golemTransform = this.transform;
        rb = transform.GetComponent<Rigidbody2D>();
        golemAnimation = transform.Find("Ani").GetComponent<GolemAnimation>();
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
        updateCharacterFacing();
        mushroomAction();
        distanceToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    private void FixedUpdate()
    {
        move();
    }
    public void transmit()
    {
        golemTransform = this.transform;
        float randomX = Random.Range(-20f, 20f); // �H���ͦ� -20 �� 20 ���ƭ�
        Vector3 newPosition = new Vector3(randomX, golemTransform.position.y, golemTransform.position.z); // �O�� y �M z ����
        golemTransform.position = newPosition; // ��s��m
    }

    public void shootlaser()
    {
        GameObject laserObject = Instantiate(laser, transform.position, transform.rotation);
        laserObject.GetComponent<Laser>().Move(direction);
    }  

    public void shootrock()
    {
        GameObject rockObject = Instantiate(rock, transform.position, transform.rotation);
        rockObject.GetComponent<Rock>().Move(direction);
    }


    public void move()
    {
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


    public void mushroomAction()
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
             
            case actionKind.transmit:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemtransmit();
                break;

            case actionKind.shoot:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemshoot();
                break;

            case actionKind.laser:
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                golemAnimation.Golemlaser();
                break;







        }
    }
}
