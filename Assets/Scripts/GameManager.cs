using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Slint
{
    public class GameManager : MonoBehaviour, IHit
    {
        [SerializeField] private Transform silo;
        [SerializeField] private Button btnSmallMissile;
        [SerializeField] private Button btnNormalMissile;
        [SerializeField] private Button btnBigMissile;
        [SerializeField] private Image healthBar;
        [SerializeField] private Missile[] missileList = new Missile[3];
        [SerializeField] private GameObject winLostPanel;
        [SerializeField] private TMP_Text txtLose;
        [HideInInspector] public ObjectPool<Missile> missliePool;
        public List<Missile> missileObjectList = new List<Missile>();
        private Missile missle;
        public float health = 100000f;
        private float maxHealth = 100000f;
        private new Camera camera;
        public static GameManager Instance;
        private void Start()
        {
            Instance = this;
            winLostPanel.SetActive(false);
            camera = Camera.main;
            onClickChooseMissile(missileList[0]);
            instantiatePool();

            btnSmallMissile.onClick.AddListener(() => onClickChooseMissile(missileList[0]));
            btnNormalMissile.onClick.AddListener(() => onClickChooseMissile(missileList[1]));
            btnBigMissile.onClick.AddListener(() => onClickChooseMissile(missileList[2]));
        }
        private void Update()
        {
            if (health <= 0)
            {
                winLostPanel.SetActive(true);
                txtLose.text = "You Lost!";
                txtLose.color = Color.red;
                StartCoroutine(reloadGame());
            }
            else if (EnemySpawnManager.instance.totalEnemies <= 0)
            {
                winLostPanel.SetActive(true);
                txtLose.text = "You Win!";
                txtLose.color = Color.green;
                StartCoroutine(reloadGame());
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    Missile missile = missliePool.Get();
                    missile.GetComponent<Missile>().mTarget = hitInfo.point;
                }
            }
        }
        private void instantiatePool()
        {
            missliePool = new ObjectPool<Missile>(() =>
            {
                return Instantiate(missle, silo.position, silo.rotation);
            }, missile =>
            {
                missile.transform.position = silo.position;
                missile.transform.rotation = silo.rotation;
                missile.gameObject.SetActive(true);
            }, missile =>
            {
                missile.gameObject.SetActive(false);
            }, missile =>
            {
                Destroy(missile.gameObject);
            }, false, 20, 50);
        }
        public void Hit(float damage)
        {
            health -= damage;
            updateHealthBar(maxHealth, health);
        }
        private void updateHealthBar(float maxHealth, float currentHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        public void onClickChooseMissile(Missile currentMissile)
        {
            missle = currentMissile;
            if (missliePool != null)
            {
                missliePool.Clear();
                instantiatePool();
            }
        }
        private IEnumerator reloadGame()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Main");
        }

    }

}