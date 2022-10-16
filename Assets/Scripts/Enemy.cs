using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
namespace Slint
{
    public class Enemy : MonoBehaviour, IHit
    {
        [HideInInspector] public float speed;
        public float health;
        public float damage;
        public EnemySO enemySo;
        [HideInInspector] public Vector3 mTarget;
        [SerializeField] private Animator mAnimator;
        [SerializeField] private Rigidbody mRigidbody;
        [SerializeField] private Image healthBar;
        [SerializeField] private GameObject healthBarPanel;
        private Vector3 target;
        private void Start()
        {
            transform.localScale = new Vector3(enemySo.size, enemySo.size, enemySo.size);
            //health = enemySo.health;
            //speed += enemySo.movementSpeed;
            //damage += enemySo.damage;
            target = mTarget;

            healthBarPanel.SetActive(true);
        }
        private void Update()
        {
            if (Vector3.Distance(transform.position, target) > 35)
            {
                //transform.position = Vector3.Lerp(transform.position, target, speed);
                transform.Translate(0, 0, 2 * speed * Time.deltaTime);
                mAnimator.SetBool("isWalking", true);
            }
            else
            {
                mAnimator.SetBool("isWalking", false);
                mAnimator.SetBool("isAttacking", true);
            }
            if (health <= 0) Dead();
        }

        public void Dead()
        {
            StartCoroutine(dead());

        }
        public void Hit(float damage)
        {
            health -= damage;
            updateHealthBar(enemySo.health, health);
        }
        IEnumerator dead()
        {
            Destroy(GetComponent<BoxCollider>());
            healthBarPanel.SetActive(false);
            mAnimator.SetBool("isAttacking", false);
            mAnimator.SetBool("isDead", true);
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
            EnemySpawnManager.instance.totalEnemies--;
        }
        private void updateHealthBar(float maxHealth, float currentHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
