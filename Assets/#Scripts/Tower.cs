using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Referances")] 
    [SerializeField] private GameObject turretBody;
    [HideInInspector] public Tile parentTile;
    [SerializeField] private ShootingInfo _shootingInfo;
    public TowerType towerType;
    public Enemy _enemy;

    public GameObject rankID;
    [Header("States")]
    public bool isBuilded;
    public bool stopFire;
    
    [Header("Configs")]
    private float shootingTime;

    private void Start() {
        InvokeRepeating("CheckEnemies",0.0f,0.1f);
    }

    private void Update()
    {
        if(_enemy){
            if(stopFire) return;
           LookEnemy();
        }
        Shooting();
        if(isBuilded && rankID!=null) rankID.SetActive(false);
    }

    private void CheckEnemies(){
        var enemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy nearestEnemy = null;
        var shortDistance = Mathf.Infinity;
        foreach(var enemy in enemies){
            var distance = Vector3.Distance(transform.position,enemy.transform.position);
            if(distance < shortDistance){
                shortDistance = distance;
                nearestEnemy = enemy.GetComponent<Enemy>();
            }
        }

        if(nearestEnemy != null && shortDistance <= _shootingInfo.Range){
            _enemy = nearestEnemy;
        }else{
            _enemy = null;
        }
    }

    private void LookEnemy()
    {
        Vector3 dir = _enemy.transform.position - turretBody.transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        Vector3 rot = lookRot.eulerAngles;
        turretBody.transform.rotation = Quaternion.Slerp(turretBody.transform.rotation,Quaternion.Euler(0,rot.y,0),0.15f);
    }

    private void Shooting()
    {
        shootingTime += Time.deltaTime;
        if(shootingTime >= _shootingInfo.FireRate){
            shootingTime = 0;
            if(!_enemy) return;
            foreach(var projectilePoint in _shootingInfo.ProjectileExitPoints){
                var dirToEnemy = projectilePoint.position - _enemy.transform.position;
                var rot = Quaternion.LookRotation(projectilePoint.transform.forward);
                var projectilePrefab = _shootingInfo.ProjectilePrefab;
                var projectile = Instantiate(projectilePrefab,projectilePoint.position,rot);
                projectile.GetComponent<Rigidbody>()
                        .AddForce(-dirToEnemy * 500f * Time.deltaTime,ForceMode.Impulse);
                projectile.GetComponent<Projectile>().Damage = _shootingInfo.Damage;
                Destroy(Instantiate(_shootingInfo.ShootParticle,projectilePoint.position,Quaternion.identity),2f);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,_shootingInfo.Range);
    }
}

public enum TowerType
{
    LEVEL0,
    LEVEL1,
    LEVEL2,
    LEVEL3,
    LEVEL4,
    LEVEL5,
    LEVEL6
}