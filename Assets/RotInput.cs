using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LCharacterFellowCamera))]
public class RotInput : MonoBehaviour {

    LCharacterFellowCamera cam;
    // Use this for initialization
    void Start () {
        cam = GetComponent<LCharacterFellowCamera>();

    }


   /* void OnMouseDrag()     //鼠标拖拽时的操作// 
    {
        Debug.LogError("OnMouseDrag");
        cam.xRot += Input.GetAxis("moveX")*Time.deltaTime;
        cam.yRot +=  Input.GetAxis("moveY") * Time.deltaTime;
      
    }*/

    bool dragging = false;
    Vector3 last = Vector3.zero;
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        if (dragging)
        {
            Vector3 delta = Input.mousePosition - last;
            cam.xRot += delta.x * Time.deltaTime;
            cam.yRot -= delta.y * Time.deltaTime;
            last = Input.mousePosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            last = Input.mousePosition;
            dragging = true;
        }
        
		
	}
}
