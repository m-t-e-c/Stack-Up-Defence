using UnityEngine;
using CameraShake;
public class Base : MonoBehaviour
{

    private GameControl gameControl;
    private UIControl uiControl;
    public int Health;

    [Header("Damage Shake")]
    [SerializeField] private PerlinShake.Params shakeParams;

    private void Start() {
        gameControl = GameObject.FindWithTag("Settings").GetComponent<GameControl>();
        uiControl = GameObject.FindWithTag("Settings").GetComponent<UIControl>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out Enemy enemy)){
            Destroy(other.gameObject);
            if(gameControl.isGameFinished) return;
            if(enemy.enemyName == "Human"){
                Health -= 1;
            }else if(enemy.enemyName == "Chopper"){
                Health -= 2;
            }else if(enemy.enemyName == "Plane"){
                Health -= 3;
            }else if(enemy.enemyName == "Boss"){
                Health -= 999;
            }
            Enemy.OnEnemyKilled?.Invoke();
            CameraShaker.Shake(new PerlinShake(shakeParams));
            if(Health<= 0){
                gameControl.isGameFinished = true;
                uiControl.Lose();
            }
        }
    }
}
