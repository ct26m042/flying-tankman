using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    ShellPool pool;
    Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void SetPool(ShellPool shellPool)
    {
        pool = shellPool;
    }

    public void Launch(Vector3 position, Quaternion rotation, Vector3 velocity, float lifetime)
    {
        transform.SetPositionAndRotation(position, rotation);
        body.position = position;
        body.rotation = rotation;
        body.linearVelocity = velocity;
        body.angularVelocity = Vector3.zero;

        StopAllCoroutines();
        StartCoroutine(DespawnAfter(lifetime));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
            return;

        MessageLogger.ReportEnemy(collision.collider.gameObject);
        Despawn();
    }

    IEnumerator DespawnAfter(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Despawn();
    }

    void Despawn()
    {
        StopAllCoroutines();
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        pool.ReturnObject(gameObject);
    }
}
