using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item {
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            GameAttribute.instance.AddCoin(1);
            
        }

    }
}
