using Slint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public float damage = 10;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<GameManager>().Hit(damage);
        }
    }
}
