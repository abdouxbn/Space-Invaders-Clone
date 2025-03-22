using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Invaders : MonoBehaviour {
    
    [Header("Main Camera: ")]
    private Camera mainCamera;
    
    [Header("Invaders Prefabs: ")]
    [SerializeField] private Invader[] invaderPrefabs;
    
    [Header("Missile Prefabs: ")]
    [SerializeField] private Projectile missilePrefab;
    
    [Header("Invaders Attack Settings: ")]
    [SerializeField] private float missileAttackRepeatRate = 1f;
    [SerializeField] private float missileAttackDelay = 1f;
    
    [Header("Invaders Grid Settings: ")]
    private int columns = 10;
    private int rows = 5;
    
    [Header("Invaders Movement Settings: ")]
    [SerializeField] private AnimationCurve movementSpeed;
    private Vector3 movementDirection = Vector3.right;
    

    [Header("Invaders Difficulty Settings: ")]
    private int totalInvaders => columns * rows;
    public int InvadersDestroyed { get; private set; }
    private int invaderAlive => totalInvaders - InvadersDestroyed;
    private float percentageInvaderDestroyed => (float)InvadersDestroyed / (float)totalInvaders;

    private void Awake() {
        mainCamera = Camera.main;
        
        for (int i = 0; i < rows; i++) {
            float offset = 1.5f;
            float gridWidth = (columns - 1) * offset;
            float gridHeight = (rows - 1) * offset;
            Vector3 centering = new Vector2(-gridWidth / 2, -gridHeight / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (i * offset), 0);
            
            for (int j = 0; j < columns; j++) {
                Invader invader = Instantiate(invaderPrefabs[i], transform);
                invader.OnInvaderDestroyed += Invader_OnInvaderDestroyed;
                Vector3 invaderPosition = rowPosition;
                invaderPosition.x += j * offset;
                invader.transform.localPosition = invaderPosition;
            }
        }
    }

    private void Start() {
        InvokeRepeating(nameof(MissileAttack), missileAttackDelay, missileAttackRepeatRate);
    }

    private void Update() {
        Vector3 rightScreenEdge = mainCamera.ViewportToWorldPoint(Vector3.right);
        Vector3 leftScreenEdge = mainCamera.ViewportToWorldPoint(Vector3.zero);
        
        transform.position += movementDirection * (movementSpeed.Evaluate(percentageInvaderDestroyed) * Time.deltaTime);
        
        foreach (Transform invader in transform) {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }
            
            if (movementDirection == Vector3.right && invader.position.x >= rightScreenEdge.x - 1.0f) {
                FlipDirection();
                transform.position = DescendInvaders(transform.position);
            }
            if (movementDirection == Vector3.left && invader.position.x <= leftScreenEdge.x + 1.0f) {
                FlipDirection();
                transform.position = DescendInvaders(transform.position);
            }
        }
    }

    private void FlipDirection() {
        movementDirection.x *= -1;
    }

    /* Takes the Invaders position and subtracts a value and then returns it
        So Invaders go down by a certain value each time */
    private Vector3 DescendInvaders(Vector3 invadersPosition) {
        Vector3 position = invadersPosition;
        position.y -= 0.55f;
        return position;
    }
    
    private void Invader_OnInvaderDestroyed(object sender, EventArgs e) {
        InvadersDestroyed++;
    }

    private void MissileAttack() {
        foreach (Transform invader in transform) {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (Random.value < (1.0f / (float)invaderAlive)) {
                Instantiate(missilePrefab, invader.position + new Vector3(-0.4f, 0f, 0f), Quaternion.identity);
                break;
            }
        }
    }
}
