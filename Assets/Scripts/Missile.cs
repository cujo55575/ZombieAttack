using System;
using System.Collections;
using UnityEngine;
namespace Slint
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        private float damage = 10;
        private float speed = 50;
        private float rotateSpeed = 700;
        [SerializeField] private MissileSO missileSo;
        [HideInInspector] public Vector3 mTarget;
        [HideInInspector] public float maxDistancePredict = 100;
        [HideInInspector] public float minDistancePredict = 10;
        [HideInInspector] public float maxTimePrediction = 50;
        [HideInInspector] public float deviationAmount = 0;
        [HideInInspector] public float deviationSpeed = 0;
        [HideInInspector] public Rigidbody rb;
        private void Start()
        {
            explosionPrefab.SetActive(false);
            rb = gameObject.GetComponent<Rigidbody>();
            damage = missileSo.damage;
            speed = missileSo.speed;
            Debug.Log($"DAMAGE:: {damage}::: speed::: {speed}");
        }
        private void FixedUpdate()
        {
            StartCoroutine(selfDestruct());
            rb.velocity = transform.forward * speed;
            var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, mTarget));
            launchMissile(leadTimePercentage);
        }
        public void launchMissile(float leadTimePercentage)
        {
            float predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);
            Vector3 standardPrediction = mTarget + Vector3.one * predictionTime;

            Vector3 deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

            Vector3 predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

            Vector3 deviatedPrediction = standardPrediction + predictionOffset;
            Vector3 heading = deviatedPrediction - transform.position;

            Quaternion rotation = Quaternion.LookRotation(heading);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Target" || collision.gameObject.tag == "Missile") return;
            if (collision.transform.TryGetComponent<IHit>(out var ex))
            {
                ex.Hit(damage);
                explosionPrefab.SetActive(true);
            }
            destoryMissile();
        }
        private IEnumerator selfDestruct()
        {
            yield return new WaitForSeconds(7);
            destoryMissile();
        }
        private void destoryMissile()
        {
            GameManager.Instance.missliePool.Release(this);
        }

    }
}