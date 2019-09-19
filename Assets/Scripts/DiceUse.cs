using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUse : MonoBehaviour
{
    public static bool canDrag = false;
    Character player;
    Board board;
    Die dice;

    void Start()
    {
        board = FindObjectOfType<Board>();
        dice = GetComponent<Die>();
        player = GameObject.Find("BlackPlayer").GetComponent<Character>();
    }

    void Update()
    {
        if (!board.CheckRolling())
            canDrag = true;
    }

    private void OnMouseDrag()
    {
        if (canDrag)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Score")
        {
            board.score += dice.value;
            board.dices.Remove(gameObject);
            Destroy(gameObject);
        }

        else if (coll.transform.tag == "CharMove")
        {
            int moveCount = dice.value;
            board.dices.Remove(gameObject);
            Destroy(gameObject);
            player.GetMove(moveCount);
        }
    }

    
}
