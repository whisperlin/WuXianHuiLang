using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRot2 : MonoBehaviour {
    [Header("用来测试相机朝向一致性")]
    public LCharacterFellowCamera fc;
    public Transform target;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (null != fc)
        {
            transform.LookAt(target,Vector3.up);
            Vector3 v = transform.rotation.eulerAngles;
            fc.xRot = v.y;
            fc.yRot = v.x;


            Vector3 f0 =  fc.cam.transform.forward.normalized;

            Vector3 f1 = transform.forward.normalized;

            //Debug.LogError("d = "+Vector3.Dot(f0,f1)+" " +f0 + f1);
        }
		
	}
}
