using UnityEngine;
using CameraShake;
public class Tile : MonoBehaviour
{
      [Header("Referances")]
      private MeshRenderer meshRenderer;
      private TowerList towerList;
      private TowerSpawner towerSpawner;
      private Tower currentTower;
      private Tower holdingTower;

      private Material defaultMat;
      public Material greenMat;
      public Material purpleMat;

      [Header("Particles")]
      [SerializeField] private GameObject buildParticle;
      [SerializeField] private GameObject combineParticle;

      [Header("Camera Shake")]
      [SerializeField] private PerlinShake.Params shakeParams;

      private void Start() {
            meshRenderer = GetComponent<MeshRenderer>();
            defaultMat = meshRenderer.material;
            towerSpawner = GameObject.FindWithTag("TowerSpawner").GetComponent<TowerSpawner>();
            towerList = GameObject.FindWithTag("TowerList").GetComponent<TowerList>();
      }

      public bool SetTower(Tower tower){
            if(tower == currentTower) return false;
            if(currentTower){
                  GameObject requestTower = towerList.GetTower(tower.towerType,currentTower.towerType) as GameObject;
                  if(requestTower){
                        RemoveTower();
                        var newTower = Instantiate(requestTower,transform.position,Quaternion.identity);
                        currentTower = newTower.GetComponent<Tower>();
                        currentTower.transform.position = transform.position;
                        currentTower.parentTile = this;
                        currentTower.isBuilded = true;
                        if(tower.parentTile){
                               tower.parentTile.RemoveTower();
                        }else{
                              Destroy(tower.gameObject);
                        }
                        Destroy(Instantiate(combineParticle,transform.position,Quaternion.identity),4f);
                        print("Tower Combined");
                        CameraShaker.Shake(new PerlinShake(shakeParams));
                        return true;
                  }

                  print("Tower Can't Combined");
                  return false;
            }

            if(tower.isBuilded) return false;

            currentTower = tower;
            currentTower.transform.position = transform.position;
            currentTower.parentTile = this;
            currentTower.isBuilded = true;
            Destroy(Instantiate(buildParticle,transform.position,Quaternion.identity),4f);
            print("Tower Setted");
            towerSpawner.waitTime = 0f;
            return true;
      }

      public void RemoveTower(){
            var tempTower = currentTower;
            currentTower = null;
            Destroy(tempTower.gameObject);
      }

      private void OnHoldTower(Tower tower){
            if(currentTower){
                  if(currentTower.towerType == tower.towerType){
                        holdingTower = tower;
                        meshRenderer.material = purpleMat;
                        return;
                  }else return;
            }
            holdingTower = tower;
            meshRenderer.material = greenMat;
      }

      private void OnDropTower(Tower tower){
            holdingTower = null;
            meshRenderer.material = defaultMat;
      }

      private void OnEnable() {
            TowerControl.OnHoldTower += OnHoldTower;
            TowerControl.OnDropTower += OnDropTower;
      }

      private void OnDisable() {
            TowerControl.OnHoldTower -= OnHoldTower;
            TowerControl.OnDropTower -= OnDropTower;
      }
}