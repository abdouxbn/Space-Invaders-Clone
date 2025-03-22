using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    [SerializeField] private Projectile laserPrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private float movementSpeed = 8f;
    private bool isLaserActive;


    private void Start() {
        GameInput.Instance.OnLeftMouse += GameInput_OnLeftMouse;
    }

    private void GameInput_OnLeftMouse(object sender, EventArgs e) {
        Shoot();
    }

    private void Update() {
        transform.position += Vector3.right * (GameInput.Instance.GetMovementInput() * movementSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out Projectile missile)) {
            // TODO: Player loses one life.
        }
    }

    private void OnDisable() {
        GameInput.Instance.OnLeftMouse -= GameInput_OnLeftMouse;
    }
    
    private void Shoot() {
        if (!isLaserActive) {
            Projectile projectile = Instantiate(laserPrefab, projectileSpawnPoint.position, quaternion.identity);
            projectile.OnProjectileDestroyed += Projectile_OnProjectileDestroyed;
            isLaserActive = true;
        }
    }

    private void Projectile_OnProjectileDestroyed(object sender ,EventArgs ctx) {
        /* Sets the isLaserActive bool back to false 
        ** indicating that there are no active lasers in the scene */
        isLaserActive = false;
    }
    
}
