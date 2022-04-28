using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public static Action OnEnemySpawned;
    public static Action OnEnemyKilled;

    [Header("Referances")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject dieParticle;

    [Header("States")]
    public bool isDead;

    [Header("Configs")]    
    public string enemyName;
    public float Health;

    private void Awake() {
        OnEnemySpawned?.Invoke();
    }
    public void GetDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(isDead) return;
        isDead = true;
        if(animator){
            animator.SetBool("Die",true);
        }
        OnEnemyKilled?.Invoke();
        Destroy(Instantiate(dieParticle,transform.position,Quaternion.identity),1f);
        Destroy(this.gameObject);
    }
}
