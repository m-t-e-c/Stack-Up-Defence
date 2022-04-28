using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float xRotSpeed;
    [SerializeField] private float yRotSpeed;
    [SerializeField] private float zRotSpeed;


    private void Update() {
        var rot = new Vector3(xRotSpeed,yRotSpeed,zRotSpeed);
        transform.Rotate(rot * Time.deltaTime);
    }
}
