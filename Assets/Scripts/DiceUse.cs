using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUse : MonoBehaviour
{
    public static bool canDrag = false;
    Board board;
    Die dice;

    void Start()
    {
        board = FindObjectOfType<Board>();
        dice = GetComponent<Die>();
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
    }
}
