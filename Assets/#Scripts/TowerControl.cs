using System;
using UnityEngine;

public class TowerControl : MonoBehaviour
{

   public static Action<Tower> OnHoldTower;
   public static Action<Tower> OnDropTower;

   private RaycastHit towerHit;
   private RaycastHit tileHit;
   public Tower firstTower;
   public Tile firstTile;

   public Vector3 towerFirstPos;

   public bool isHolding;

   private void Update() {

      ControlTile();

      if(Input.GetMouseButton(0)){
         ControlTower();
         isHolding = true;
         if(!firstTower) return;
         firstTower.stopFire = true;
         OnHoldTower?.Invoke(firstTower);
      }

      if(Input.GetMouseButtonUp(0)){
          if(firstTower){
            firstTower.stopFire = false;
            OnDropTower?.Invoke(firstTower);
          }
         if(PlaceTower()){
            firstTower = null;
            towerFirstPos = Vector3.zero;            
         }else{
            if(firstTower){
               firstTower.transform.position = towerFirstPos;
            }
            towerFirstPos = Vector3.zero;    
            firstTower = null;
         }
         isHolding = false;
      }

      AttachTower();
   }

   private void ControlTile(){
      var tileRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      Physics.Raycast(tileRay.origin,tileRay.direction,out tileHit, Mathf.Infinity,LayerMask.GetMask("Tiles"));
      if(tileHit.collider){
         firstTile = tileHit.transform.GetComponent<Tile>();
      }else{
         firstTile = null;
      }
   }

   private void ControlTower(){
      if(firstTower) return;
      var towerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      Physics.Raycast(towerRay.origin,towerRay.direction,out towerHit, Mathf.Infinity,LayerMask.GetMask("Towers"));
      if(towerHit.collider){
         firstTower = towerHit.transform.GetComponent<Tower>();
         if(towerFirstPos == Vector3.zero){
            towerFirstPos = firstTower.transform.position;
         }
      }else{
         if(isHolding) return;
         firstTower = null;
      }
   }

   private bool PlaceTower(){
      if(!firstTower) return false;
      if(!firstTile) return false;

      if(firstTile.SetTower(firstTower)){
         return true;
      }
      return false;
   }

   private void AttachTower(){
      if(!firstTower) return;
      if(!isHolding) return;
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Physics.Raycast(ray.origin,ray.direction, out RaycastHit hit, Mathf.Infinity,LayerMask.GetMask("Ground"));
      Debug.DrawRay(ray.origin,ray.direction * 500f,Color.red);
      if(hit.collider){
         var absPos = new Vector3(hit.point.x,hit.point.y + 1.6f,hit.point.z - 1f);
         firstTower.transform.position = absPos;
      }
   }
}
