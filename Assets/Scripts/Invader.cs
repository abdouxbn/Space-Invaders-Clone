using System;
using UnityEngine;

public class Invader : MonoBehaviour {

    private Animator invaderAnimator;
    private BoxCollider2D invaderCollider;
    
    public event EventHandler OnInvaderDestroyed;

    private void Awake() {
        invaderAnimator = GetComponentInChildren<Animator>();
        invaderCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out Projectile laser)) {
            OnInvaderDestroyed?.Invoke(this, EventArgs.Empty);
            gameObject.SetActive(false);
        }
    }
}
