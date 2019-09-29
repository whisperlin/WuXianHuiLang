using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakePointBuilder : MonoBehaviour {

	[MenuItem("无限回廊/生成路点")]
	static void BuildWayPoint () {
     
        Block [] blocks = GameObject.FindObjectsOfType<Block>();
        for (int i = 0; i < blocks.Length; i++)
        {
            Block block = blocks[i];

            block.meshFilters.Clear();
            block.meshRenders.Clear();
            var mf = block.gameObject.GetComponent<MeshFilter>();
            var mr = block.gameObject.GetComponent<MeshRenderer>();
            if (mf != null && mr != null)
            {
                block.meshFilters.Add(mf);
                block.meshRenders.Add(mr);
            }
           
            block.meshFilters.AddRange( block.gameObject.GetComponentsInChildren<MeshFilter>());
            block.meshRenders.AddRange( block.gameObject.GetComponentsInChildren<MeshRenderer>());

            
            block.UpdateWayPoint();

        }
       
        
	}
	 
}
