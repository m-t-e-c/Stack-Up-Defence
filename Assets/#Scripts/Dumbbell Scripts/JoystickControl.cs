using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    //Script References
    GameObject settingObj;
    GameControl game;
    SettingsControl settings;
    Joystick joystick;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Rigidbody rb;
    public float moveSpeed = 20;
    public float rotationSpeed = 5;

    public void InitializeReferences()
    {
        settingObj = GameObject.FindGameObjectWithTag("Settings");
        game = settingObj.GetComponent<GameControl>();
        settings = settingObj.GetComponent<SettingsControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        joystick = game.joystick;
    }

    public void JoystickInput()
    {
        if (Input.GetMouseButton(0))
        {
            float x = joystick.Horizontal;
            float y = joystick.Vertical;
            float heading = Mathf.Atan2(x * 100f, y * 100f);
            if(y >= 0.1f || y <= -0.1f || x >= 0.1f || x <= -0.1f)
            {
                transform.rotation = Quaternion.Euler(0f, (heading * Mathf.Rad2Deg), 0f);
            }
            rb.velocity = new Vector3(x*moveSpeed*Time.deltaTime,0,y*moveSpeed*Time.deltaTime);
        }
        else rb.velocity = Vector3.zero;
    }
}
