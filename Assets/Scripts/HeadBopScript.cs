using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBopScript : MonoBehaviour
{
    [Range(0.001f, 0.01f)]
    public float Amount = 1f;
    [Range(1f, 30f)]
    public float Frequancy = 10.0f;
    [Range(10f, 100f)]
    public float Smooth = 40.0f;

    public int rotationSpeed = 180; 
    public int speed = 0;

    public bool playerIsWalking = false;

    void Start()
    {
        //StartPos = transform.localPosition;
    }

    void Update()
    {
        //Debug.Log("Update");
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerIsWalking = true;
            //Debug.Log("Hello KeyDown");

            Amount = 1f;
            Frequancy = 10.0f;
            Smooth = 0.40f;

        } 
        
        else

        {
            playerIsWalking = false;
            //Debug.Log("CUUHHHH");
            
            Amount = 0.3f;
            Frequancy = 3.0f;
            Smooth = 0.5f;
            StartHeadBop();



        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Frequancy = 20.0f;
        } 
        
        else
        
        {
            Frequancy = 10.0f;
        }

        if(playerIsWalking)
        {
            StartHeadBop();
        }
        
        //CheckForHeadbopTrigger();
    }

    //private void CheckForHeadbopTrigger()
    //{
        //if gameManager segir að player ma  byrja að labba 
        //StartHeadBop();
        
    //}

    private Vector3 StartHeadBop()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequancy) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequancy / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadBop()
    {
        //ef player snertir PlayerTrigger á husinu. þa stop
    }
}


    

