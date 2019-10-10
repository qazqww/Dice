using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform enemy;
    CharacterClone enemyChar;
    float attackDist = 1.5f;
    float moveSpeed = 1.8f;

    int maxHp = 100;
    int curHp = 50;
    int atk = 10;
    int def = 0;
    public int Def
    {
        get { return def; }
    }

    public int CurHp {
        get { return curHp; }
        set { curHp -= value; }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyChar = enemy.GetComponent<CharacterClone>();
    }

    void Update()
    {
        UpdateState();
    }

    public void SetStatus(int maxHp, int curHp, int atk, int def)
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        this.atk = atk;
        this.def = def;
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
        // 상대 hp 감소
        enemyChar.CurHp = atk - enemyChar.Def;
        if (enemyChar.CurHp < 0)
            ChangeState(State.Idle, 1);
    }

    void Knockout()
    {
        ChangeState(State.Knockout, 3);
    }

    float GetDistance()
    {
        float dist = Vector3.Distance(transform.position, enemy.transform.position);
        return dist;
    }
}
