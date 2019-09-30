using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {

    Block[] blocks;

    void BuildWayPoint()
    {

        blocks = GameObject.FindObjectsOfType<Block>();
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

            block.meshFilters.AddRange(block.gameObject.GetComponentsInChildren<MeshFilter>());
            block.meshRenders.AddRange(block.gameObject.GetComponentsInChildren<MeshRenderer>());


            block.UpdateWayPoint();

        }


    }
    // Use this for initialization
    void Start () {
        BuildWayPoint();

    }
    List<Color> randomColor = new List<Color>();
    Color GetRandomColor(int i)
    {
        if (randomColor.Count > i)
        {
            return randomColor[i];
        }
        var c = new Color(Random.Range(0.4f, 1f), Random.Range(0.3f, 1f), Random.Range(0.2f, 1f));
        randomColor.Add(c);
        return c;
    }


    // Update is called once per frame
    void Update()
    {
        bool isConnect = false;
        Camera cam = Camera.main;
        for (int i = 0; i < blocks.Length; i++)
        {
            Block b = blocks[i];
            for (int j = 0; j < b.points.Count; j++)
            {
                var p = b.points[j];
                for (int k = 0; k < 4; k++)
                {
                    p.screenPoints[k] = cam.WorldToScreenPoint(p.airPoints[k]);
                }
            }
        }
        for (int i = 0; i < blocks.Length; i++)
        {
            Block b1 = blocks[i];

            for (int j = i; j < blocks.Length; j++)
            {

                
            }
        }
    }
}
