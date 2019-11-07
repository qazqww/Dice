using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterClone : MonoBehaviour
{
    public enum State
    {
        Idle,
        Run,
        Attack,
        Knockout
    }
    public State curState = State.Idle;

    Animator animator;
    public ParticleSystem hitEffect;

    Transform enemy;
    CharacterClone enemyChar;
    float attackDist = 1.5f;
    float moveSpeed = 2.25f;
    bool isOne = false;

    float waitTime = 1f;
    float elapsedTime;
    bool combatEnd = false;
    bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
    }

    int maxHp = 999;
    public int MaxHp
    {
        get { return maxHp; }
    }

    int curHp = 99;
    public int CurHp
    {
        get { return curHp; }
        set { curHp -= value; }
    }

    int atk = 99;
    public int Atk
    {
        get { return atk; }
    }

    int def = 99;
    public int Def
    {
        get { return def; }
    }

    public void SetStatus(int maxHp, int curHp, int atk, int def)
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        this.atk = atk;
        this.def = def;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (transform.name == "PlayerOne")
        {
            enemy = GameObject.Find("PlayerTwo").GetComponent<Transform>();
            isOne = true;
        }
        else if (transform.name == "PlayerTwo")
        {
            enemy = GameObject.Find("PlayerOne").GetComponent<Transform>();
        }

        enemyChar = enemy.GetComponent<CharacterClone>();
        hitEffect.Stop();
        elapsedTime = 0f;
    }

    void Update()
    {
        if (combatEnd)
        {
            Combat.isEnd = true;
            elapsedTime += Time.deltaTime;

            if (isOne) // PlayerOne 캐릭터를 기준으로 결과 도출
            {
                if (isDead)
                {
                    if (enemyChar.IsDead) // 무승부
                        Combat.result = 0;

                    else // 패배
                        Combat.result = 2; // p2 승리
                }
                else // 승리
                    Combat.result = 1; // p1 승리
            }
        }
        else if (elapsedTime < waitTime) // 전투 전 대기시간 (1초)
            elapsedTime += Time.deltaTime;

        UpdateState();
    }

    void UpdateState()
    {
        switch(curState)
        {
            case State.Idle:
                Idle();
                break;
                
            case State.Run:
                Run();
                break;
                
            case State.Attack:
                Attack();
                break;
                
            case State.Knockout:
                Knockout();
                break;
        }

        DeadCheck();
    }

    void ChangeState(State state, int aniNum)
    {
        if (curState == state)
            return;

        curState = state;
        animator.SetInteger("aniNum", aniNum);
        // Run: 1, Kick: 2, Down: 3
    }

    void Idle()
    {
        if (elapsedTime >= waitTime && combatEnd == false)
            ChangeState(State.Run, 1);
    }

    void Run()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemy.position, moveSpeed * Time.deltaTime);
        if (GetDistance() < attackDist)
            ChangeState(State.Attack, 2);
    }

    void Attack()
    {
        // AttackMotion
        elapsedTime = 0f;

        if (enemyChar.CurHp <= 0)
        {
            ChangeState(State.Idle, 0);
            combatEnd = true;
        }
    }

    void AttackMotion()
    {
        enemyChar.CurHp = atk - enemyChar.Def;
        ShowHitEffect();
    }

    void Knockout()
    {
        ChangeState(State.Knockout, 3);
        isDead = true;
    }

    void DeadCheck()
    {
        if (curHp <= 0)
        {
            curHp = 0;
            ChangeState(State.Knockout, 3);
            combatEnd = true;
        }
    }

    public void ShowHitEffect()
    {
        hitEffect.Play();
    }

    float GetDistance()
    {
        float dist = Vector3.Distance(transform.position, enemy.transform.position);
        return dist;
    }
}
