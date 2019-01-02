using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour {
    //linerenderer component
    LineRenderer lineRenderer;
    //Create Raycast Variables
    RaycastHit2D rc2d;
    Transform t_parent;
    //RaycastHit2D[] results;
    int x;
    public ContactFilter2D cf_empty;
    Vector3[] line_startEnd;
    Vector2 line_end;
    public float max_dist;
    //Create Trigger or collision detection
	void Start ()
    {
        x = 1;
        line_startEnd = new Vector3[3];
        t_parent = transform;
        //results = new RaycastHit2D[x];
        cf_empty = new ContactFilter2D();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = .1f;
	}
	
	void Update ()
    {
        RaycastHit2D[] results = new RaycastHit2D[2];
        Physics2D.Raycast(transform.position, transform.right, cf_empty.NoFilter(), results, max_dist);
        line_startEnd[0] = transform.position; //TODO why are we doing this here in update
        Vector3 first_hit_surfacetemp = results[0].point;
        if(results[0].transform == null)//when we dont hit something
        {
            line_end = transform.position + (transform.right * max_dist);
            line_startEnd[1] = line_end; //this is the end of the line
        }
        else
        {
            line_startEnd[1] = results[0].point;
        }
        //lineRenderer.SetPositions(line_startEnd);
        Debug.Log(Mathf.Acos(results[0].normal.x));
        float gObj_rotationForReflection_z = ((180.0f + (2.0f * (Mathf.Acos(results[0].normal.x)) * Mathf.Rad2Deg)) - transform.eulerAngles.z);
        if(gObj_rotationForReflection_z >= 360.0f)
        {
            gObj_rotationForReflection_z -= 360.0f;
        }

        Vector3 newtransformrighttemp;
        newtransformrighttemp.x = Mathf.Cos(gObj_rotationForReflection_z);
        newtransformrighttemp.y = Mathf.Sin(gObj_rotationForReflection_z);
        newtransformrighttemp.z = 0;
        Vector2 sourcev2temp = results[0].point;
        Physics2D.Raycast(sourcev2temp, newtransformrighttemp, cf_empty.NoFilter(), results, max_dist);
        if (results[0].transform == null)//when we dont hit something
        {
            line_end = first_hit_surfacetemp + (newtransformrighttemp * max_dist);
            line_startEnd[2] = line_end; //this is the end of the line
        }
        else
        {
            line_startEnd[2] = results[0].point;
            //Debug.Log("line_startend[2]: " + line_startEnd[2].ToString() + "\n" +     
            //          "firsthitsurfacetemp: " + first_hit_surfacetemp.ToString() + "\n" +
            //          "newtransfromrighttemp: " + newtransformrighttemp.ToString());
        }
        lineRenderer.SetPositions(line_startEnd);
        //Debug.Log("Angles: WorldAngle: " + transform.eulerAngles.z + ", Rot4ref: " + gObj_rotationForReflection_z + " \n" + 
                // " surfacenorm: " + Mathf.Acos(results[0].normal.x) *Mathf.Rad2Deg);

        //Debug.Log(transform.right.ToString());
        //Debug.Log("Length: " + results.Length + ", hitname: " + results[0].transform.name);
    }
}
