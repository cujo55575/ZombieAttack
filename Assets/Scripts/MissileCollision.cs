using Slint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    public float damage = 100;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<IHit>(out var ex)) ex.Hit(damage);
    }
}
