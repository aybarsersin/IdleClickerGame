using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCam : MonoBehaviour
{
    public bool mouseLock = true;

    public float mainSpeed = 50f; //regular speed
    public float shiftAdd = 100f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000f; //Maximum speed when holdin gshift
    public float camSens = 0.15f; //How sensitive it with mouse

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1f;

    // Update is called once per frame
    void Update()
    {
        MouseLockMethod();

        if (mouseLock == false)
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;

        }
        //Mouse  camera angle done.  

        //Keyboard commands
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1, 1000);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        transform.Translate(p);
    }

    private void MouseLockMethod()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && mouseLock == true)
        {
            mouseLock = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && mouseLock == false)
        {
            mouseLock = true;
        }
    }

    //returns the basic values, if it's 0 than it's not active.
    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W) && mouseLock == false)
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S) && mouseLock == false)
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A) && mouseLock == false)
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) && mouseLock == false)
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space) && mouseLock == false)
        {
            Vector3 pos = transform.position;
            pos.y += mainSpeed/100f;
            transform.position = pos;
        }
        return p_Velocity;
    }
}
