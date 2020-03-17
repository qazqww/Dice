using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollPlace : MonoBehaviour
{
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Dice")
            AudioManager.Instance.PlayUISound(SoundType.diceroll);
    }
}
