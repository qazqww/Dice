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

    Animator camAni;

    Animator animator;    
    public ParticleSystem hitEffect;
    public Guirao.UltimateTextDamage.UltimateTextDamageManager ultimateText;
    public Image hpBar;

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
        set {
            curHp -= value;
            if(curHp > 0)
                hpBar.rectTransform.localScale = new Vector3(curHp / (float)maxHp, 1f, 1f);
            else
                hpBar.rectTransform.localScale = new Vector3(0f, 1f, 1f);
        }
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
        camAni = Camera.main.GetComponent<Animator>();
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

        hpBar.rectTransform.localScale = new Vector3(curHp / (float)maxHp, 1f, 1f);
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
        int num = Random.Range(1, 4);
        string str = "rock_impact_small_hit_0" + num;
        AudioManager.Instance.PlayUISound(str);

        int myDmg = atk - enemyChar.Def;
        myDmg = myDmg > 0 ? myDmg : 0;
        enemyChar.CurHp = myDmg;

        int enemyDmg = enemyChar.atk - def;
        enemyDmg = enemyDmg > 0 ? enemyDmg : 0;
        ultimateText.Add("-" + enemyDmg, new Vector3(transform.position.x, transform.position.y + 1.75f, transform.position.z), "key0");

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
        camAni.SetTrigger("isHit");
    }

    float GetDistance()
    {
        float dist = Vector3.Distance(transform.position, enemy.transform.position);
        return dist;
    }
}
