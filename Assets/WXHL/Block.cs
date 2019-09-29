using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LVector3Int
{
    public int x;
    public int y;
    public int z;
   

    public LVector3Int(int v1, int v2, int v3) : this()
    {
        this.x = v1;
        this.y = v2;
        this.z = v3;
    }
    public static implicit operator Vector3(LVector3Int v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
}

[System.Serializable]
public class WayPoint
{
    [Header("位置")]
    public LVector3Int pos;
    public Vector3 hitPoint;
    public Vector3[] airPoints  ;
    [System.NonSerialized]
    public List<WayPoint> connecteds = new List<WayPoint>()  ;
}
public class Block : MonoBehaviour {

    public enum TYPE
    {
        WAY,
        STAIRS,
        PITFALL,
        TRAMPOLINE,
 
    }
    public TYPE type = TYPE.WAY;
    public List<WayPoint> points = new List<WayPoint>();

    [System.NonSerialized]
    public List<MeshFilter> meshFilters = new List<MeshFilter>();

    [System.NonSerialized]
    public List<MeshRenderer> meshRenders = new List<MeshRenderer>();

    public Color connectColor = Color.red; 
    public static int SafeInt(float f)
    {
        if (f > 0)
        {
            return (int)(f + 0.00001f);
        }
        else
        {
            return (int)(f - 0.00001f);
        }
    }
    public static int SafeHeight(float f)
    {
        if (f > 0)
        {
            return Mathf.FloorToInt(f + 0.00001f);
        }
        else
        {
            return Mathf.FloorToInt(f - 0.00001f);
        }
    }
    private void Start()
    {
        UpdateConnecctPoint();
    }
    public void UpdateConnecctPoint()
    {
        for (int i = 0, c = points.Count; i < c; i++)
        {
            var wp = points[i];
            wp.connecteds.Clear();
            for (int j = 0; j < c; j++)
            {
                var wp1 = points[j];
                if (i == j)
                    continue;
                if ( Mathf.Abs(wp.pos.x - wp1.pos.x) <= 1
                    && Mathf.Abs(wp.pos.y - wp1.pos.y) <= 1
                    && Mathf.Abs(wp.pos.z - wp1.pos.z) <= 1
                     )
                {
                    wp.connecteds.Add(wp1);
                }
            }
        }
    }
    public void UpdateWayPoint(int minRange = -500, int  maxRange = 500)
    {
 
        points.Clear();
        for (int x = minRange; x < maxRange; x++)
        {
            for (int z = minRange; z < maxRange; z++)
            {
        
                WayPoint p = new WayPoint();
                p.hitPoint = new Vector3(x, -1001, z);
                bool isHit = false;
                for (int j = 0,c0 = meshFilters.Count; j < c0 ; j++)
                {
                    var mr = meshRenders[j];
                    var mf = meshFilters[j];
                    var max = mr.bounds.max;
                    var min = mr.bounds.min;
                    if (min.x < x && x < max.x && min.z < z && z < max.z)
                    {
                        RaycastHit hitInfor;
                        if (RaycastHelper.Raycast(mf, new Ray(new Vector3(x, 500, z), Vector3.down), out hitInfor, 1000f))
                        {
                            if (isHit)
                            {
                                if (hitInfor.point.y > p.hitPoint.y)
                                {
                                    p.hitPoint = hitInfor.point;
                                }
                            }
                            else
                            {
                                p.hitPoint = hitInfor.point;
                                
                            }
                            isHit = true;
                        }
                    }
                }
                if (isHit)
                {
                    
                    p.pos = new LVector3Int(SafeInt(p.hitPoint.x), SafeHeight(p.hitPoint.y), SafeInt(p.hitPoint.z));
                    if (type == TYPE.WAY)
                    {
                        p.airPoints = new Vector3[4];
                        p.airPoints[0] = p.pos + new Vector3(-0.5f,0.5f,-0.5f);
                        p.airPoints[1] = p.pos + new Vector3(0.5f, 0.5f,-0.5f);
                        p.airPoints[2] = p.pos + new Vector3(-0.5f, 0.5f,0.5f);
                        p.airPoints[3] = p.pos + new Vector3(0.5f, 0.5f, 0.5f);
                    }
                    points.Add(p);
                }
     
            }
        }
        
    }

    static Color wayPointColor = new Color(0.5f, 0.5f, 1.0f);
    
    static Vector3 waySize = new Vector3(1f, 0.001f, 1f);
    void OnDrawGizmos()
    {
        for (int i = 0, c = points.Count; i < c; i++)
        {
            var wp = points[i];

            if (type == TYPE.WAY)
            {
                Gizmos.color = connectColor;
                Gizmos.DrawCube(wp.hitPoint, waySize);
                Gizmos.color = wayPointColor;
                Gizmos.DrawWireCube(wp.pos, Vector3.one);
                Gizmos.DrawSphere(wp.hitPoint, 0.2f);
                
            }
            else if (type == TYPE.STAIRS)
            {
                Gizmos.color = wayPointColor;
                Gizmos.DrawWireCube(wp.pos, Vector3.one);
                Gizmos.DrawSphere(wp.pos, 0.2f);
            }
            Vector3 p0 = wp.hitPoint;
            for (int j = 0 ,c2 = wp.connecteds.Count; j < c2; j++)
            {
                Gizmos.DrawLine(wp.hitPoint,wp.connecteds[j].hitPoint);
                Gizmos.color = wayPointColor;
                
                Vector3 p1 = wp.connecteds[j].hitPoint;

                Gizmos.DrawLine(wp.hitPoint, wp.connecteds[j].hitPoint);
                //Gizmos.DrawSphere((wp.hitPoint + wp.connecteds[j].hitPoint) *0.5f, 0.1f);
           
            }
        }
        
        
        //Debug.DrawLine(transform.position, transform.position+Vector3.up*2,Color.red);
    }
}
