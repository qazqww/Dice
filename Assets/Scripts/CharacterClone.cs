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

    public Transform enemy;
    CharacterClone enemyChar;
    float attackDist = 1.5f;
    float moveSpeed = 2.25f;

    float waitTime = 1f;
    float elapsedTime;
    bool combatEnd = false;
    bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
    }
    int result = -1;

    int charCode;

    int maxHp = 100;
    public int MaxHp
    {
        get { return maxHp; }
    }

    int curHp = 50;
    public int CurHp
    {
        get { return curHp; }
        set { curHp -= value; }
    }

    int atk = 10;
    public int Atk
    {
        get { return atk; }
    }

    int def = 0;
    public int Def
    {
        get { return def; }
    }

    int gold; // 땜빵 변수

    void SetStatus(int maxHp, int curHp, int atk, int def)
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        this.atk = atk;
        this.def = def;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyChar = enemy.GetComponent<CharacterClone>();

        if (transform.name == "PlayerOne")
            charCode = 1;
        else if (transform.name == "PlayerTwo")
            charCode = 2;

        FuncHelper.GetPlayerData(ref maxHp, ref curHp, ref atk, ref def, ref gold);
        enemyChar.SetStatus(150, 80, 10, 5);
        hitEffect.Stop();

        elapsedTime = 0f;
    }

    void Update()
    {
        if (combatEnd)
        {
            Combat.isEnd = true;
            elapsedTime += Time.deltaTime;

            if (isDead)
            {
                if (enemyChar.IsDead) // 무승부
                {
                    result = 1;
                    Combat.result = 0;
                }

                else // 패배
                    result = 0;
            }
            else // 승리
            {
                result = 2;
                Combat.result = charCode;
            }

            if (elapsedTime >= 0.2f)
            {
                switch(result)
                {
                    case 0:
                        FuncHelper.SetPlayerHPHalf();
                        //resultText.text = "Lose.";
                        break;
                    case 1:
                        FuncHelper.SetPlayerHPHalf();
                        //resultText.text = "Draw.";
                        break;
                    case 2:
                        PlayerPrefs.SetInt("CurHp", curHp + 10);
                        //resultText.text = "Win.";
                        break;
                }

                combatEnd = false;
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
        //Debug.Log("Attack / " + enemyChar.CurHp);
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
