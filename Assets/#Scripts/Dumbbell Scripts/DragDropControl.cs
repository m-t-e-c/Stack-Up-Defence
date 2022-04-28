using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropControl : MonoBehaviour
{
    [HideInInspector] public float yOffset;
    [HideInInspector] public LayerMask draggable, groundLayer;

    // Internal Variables
    float startHeight;
    GameObject selectedObject;
    bool isHolding;
    Vector3 pos1;

    GameObject settingObj;
    GameControl game;
    SettingsControl settings;

    public void InitializeReferences()
    {
        settingObj = GameObject.FindGameObjectWithTag("Settings");
        game = settingObj.GetComponent<GameControl>();
        settings = settingObj.GetComponent<SettingsControl>();

        yOffset = settings.dragDropYOffset;
        draggable = settings.draggableLayer;
        groundLayer = settings.groundLayer;
    }

    public void DragAndDrop()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggable))
            {
                selectedObject = hit.transform.gameObject;
                startHeight = selectedObject.transform.position.y;
                isHolding = true;
            }
        }
        // Set objects position to mouse position and give y offset
        if (Input.GetMouseButton(0) && isHolding) 
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                selectedObject.transform.position = new Vector3(hit.point.x, yOffset, hit.point.y);
            }
        }
        // Drop selected object & set selected object null
        if (Input.GetMouseButtonUp(0) && isHolding) 
        {
            Vector3 dropPosition = selectedObject.transform.position;
            dropPosition.y = startHeight;
            selectedObject.transform.position = dropPosition;
            selectedObject = null;
            isHolding = false;
        }
    }
}
