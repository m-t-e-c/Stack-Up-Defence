using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveControl : MonoBehaviour
{
    // Variables
    [HideInInspector] public float speedSideways, speedForward, clampValue;
    [HideInInspector] public bool planeMovement = false;
    // References
    [HideInInspector] public GameObject player;
    [HideInInspector] public Rigidbody rb;
    bool holding;
    Vector3 startPos, currentPos;
    SettingsControl settings;
    GameControl game;
    GameObject settingObj;
    Vector2 GetMousePosition()
    {
        return new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
    }

    public void InitializeReferences()
    {
        settingObj = GameObject.FindGameObjectWithTag("Settings");
        game = settingObj.GetComponent<GameControl>();
        settings = settingObj.GetComponent<SettingsControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();

        speedSideways = settings.swerveSideways;
        speedForward = settings.swerveForward;
        clampValue = settings.clampValue;
        planeMovement = settings.planeSwerve;
    }

    public void Swerve()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = GetMousePosition();
            holding = true;
        }

        if (Input.GetMouseButton(0) && holding)
        {
            currentPos = GetMousePosition();
            Vector3 deltaPos = startPos - currentPos;
            startPos = currentPos;

            float Vx = Mathf.Lerp(rb.velocity.x, -deltaPos.x * speedSideways, 100f * Time.deltaTime);
            float Vy = rb.velocity.y;
            PlaneSwerve(Vx, Vy, deltaPos);
            SideSwerve(Vx, Vy);
        }

        if (Input.GetMouseButtonUp(0))
        {
            holding = false;
            rb.velocity = Vector3.zero;
        }
    }
#region Input Dimension
    void SideSwerve(float Vx, float Vy)
    {
        float Vz = rb.velocity.z;
        rb.velocity = new Vector3(Vx, Vy, Vz);

        float clampedX = Mathf.Clamp(rb.transform.position.x, -clampValue, clampValue);
        player.transform.position = new Vector3(clampedX, rb.transform.position.y, rb.transform.position.z);
    }
    void PlaneSwerve(float Vx, float Vy, Vector3 deltaPos)
    {
        float Vz = Mathf.Lerp(rb.velocity.y, -deltaPos.y * speedForward, 100f * Time.deltaTime);
        rb.velocity = new Vector3(Vx,Vy,Vz);
    }
#endregion Input Dimension
}
