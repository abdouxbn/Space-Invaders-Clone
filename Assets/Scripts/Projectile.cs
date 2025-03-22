using UnityEngine;
using System;

public class Projectile : MonoBehaviour {

    [SerializeField] private Vector3 projectileDirection;
    [SerializeField] private float projectileSpeed;
    public event EventHandler OnProjectileDestroyed;

    private void Update() {
        transform.position += projectileDirection * (projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        OnProjectileDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
