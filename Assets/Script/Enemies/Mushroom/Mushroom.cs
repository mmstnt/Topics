using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D rb;
    private MushroomAnimation mushroomAnimation;
    private GameObject player;
    private float moveDuring;
    
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("����Ѽ�")]
    public GameObject spore;
    public GameObject spore2;
    public float MoveSpeed; // ���Ⲿ�ʳt��
    public float moveTimeMin;
    public float moveTimeMax;
    public float biteDistance;

    [Header("���⪬�A")]
    public int direction; // ���ʤ�V�]-1 ��ܥ��A1 ��ܥk�^
    public float distanceToPlayer;
    public bool action;
    public enum actionKind {move, controlMushroom,bite, throwSpore }
    public actionKind actionMode;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();  
        mushroomAnimation = transform.Find("Ani").GetComponent<MushroomAnimation>();
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

    public void ThrowSpore()
    {
        int times = 10;
        for (int i = 0; i < times; i++)
        {
            GameObject sporeObject = Instantiate(spore, transform.position, transform.rotation);
            sporeObject.GetComponent<Spore>().attack3(direction);
        }
    }

    public void ControlMushroom()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = 1;
        newPosition.y = 1;
        int times = 10;
        for (int i = 0; i < times; i++)
        {
            GameObject spore2Object = Instantiate(spore2, newPosition, transform.rotation);
            spore2Object.GetComponent<Spore2>().attack3(direction);
        }
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
             
            case actionKind.controlMushroom:
                if (distanceToPlayer < biteDistance)
                    break;
                action = true;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                mushroomAnimation.attack1();
                break;


            case actionKind.bite:
                if (distanceToPlayer > biteDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                mushroomAnimation.attack2();
 
                break;
            case actionKind.throwSpore:
                if (distanceToPlayer < biteDistance)
                    break;
                action = true;
                rb.velocity = Vector2.zero;
                direction = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                mushroomAnimation.attack3();
                break;



        }
    }
}
