using Slint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;
public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();
    private ObjectPool<Enemy> enemiesPool;
    private float health = 0;
    private float damage = 0;
    private float speed = 0;
    public bool isInstantiatingDone = false;
    public int totalEnemies = 25;
    private void Start()
    {
        instance = this;
        instantiateEnemiesPool();
        StartCoroutine(stopGame());
        InvokeRepeating(nameof(spawnEnemies), 0, 1f);
    }
    public void spawnEnemies()
    {
        if (!isInstantiatingDone)
        {
            health += 15;
            damage += 10;
            speed += 5f;
            for (int i = 0; i < 5; i++)
            {
                Enemy enemy = enemiesPool.Get();
                int ran = Random.Range(0, enemySpawnPoints.Length);
                enemy.health = enemy.enemySo.health + (health * 0.5f); ;
                enemy.damage = enemy.enemySo.damage + (damage * 0.25f);
                enemy.speed = enemy.enemySo.movementSpeed + (speed * 0.15f);
                enemy.transform.position = new Vector3(enemySpawnPoints[ran].position.x, 5.8f, enemySpawnPoints[ran].position.z);
                enemy.transform.LookAt(target.transform, Vector3.up);
                enemy.mTarget = new Vector3(target.transform.position.x, 5.8f, target.transform.position.z);
            }
        }
    }
    private void instantiateEnemiesPool()
    {
        enemiesPool = new ObjectPool<Enemy>(() =>
        {
            return Instantiate(enemyList[Random.Range(0, enemyList.Count)]);
        }, enemy =>
        {
            enemy.gameObject.SetActive(true);
        }, enemy =>
        {
            enemy.gameObject.SetActive(false);
        }, enemy =>
        {
            Destroy(enemy.gameObject);
        }, false, 40, 150);
    }
    private int enemyRotation(int index)
    {
        if (index >= 0 && index <= 9) return 90;
        if (index >= 10 && index <= 19) return 180;
        if (index >= 20 && index <= 29) return -90;
        return 0;
    }
    private IEnumerator stopGame()
    {
        yield return new WaitForSeconds(5);
        isInstantiatingDone = true;
    }
}
//public enum e_DIRECTION
//{
//    North = 90,
//    East = 180,
//    South = -90,
//    West = 0,
//}
//0->9
//10->19
//20->29
//29->39