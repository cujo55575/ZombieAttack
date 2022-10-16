using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemySatusSO", order = 1)]
public class EnemySO : ScriptableObject
{
    public float health;
    public float movementSpeed;
    public float size;
    public float damage;
}
