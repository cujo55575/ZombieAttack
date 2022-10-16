using System;
using System.Collections;
using UnityEngine;
namespace Slint
{
    public class Rocket : MonoBehaviour
    {
        public GameObject target;
        private float rocketSpeed = 0.2f;
        private float damage = 10;
        private void Update()
        {
            transform.LookAt(target.transform);
            StartCoroutine(ShootRocket());
        }
        public IEnumerator ShootRocket()
        {
            while (Vector3.Distance(target.transform.position, transform.position) > 0.3f)
            {
                transform.position += (target.transform.position - transform.position).normalized * rocketSpeed * Time.deltaTime;
                transform.LookAt(target.transform);
                yield return null;
            }
            Destroy(gameObject);
        }
        private void OnCollisionEnter(Collision collision)
        {
            //if (explosionPrefab) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (collision.transform.TryGetComponent<IHit>(out var ex)) ex.Hit(damage);

            Destroy(gameObject);
        }
    }
}