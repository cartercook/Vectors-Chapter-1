using UnityEngine;
using System.Collections;
/**
 * Because Quads are fuck, transform.lookat makes them point AWAY from the camera.
 * So I have written this handy class, namely for my Goal and NumLine objects.
 */
public class PointAtCamera : MonoBehaviour {
	public bool constrainY;
	Transform mainCam;

	void Start () {
		mainCam = Camera.main.transform;
	}
	
	void Update () {
		Vector3 inversePos = transform.position*2 - mainCam.position; //gets cam position mirrored behind this obj
		Vector3 up = Vector3.up; //will be used as a param for LookAt()
		if (constrainY) { 
			Transform parent = transform.parent; //just using 'transform' causes it to slide away from the center
			inversePos = parent.InverseTransformPoint(inversePos); //bring point into local xyz coordinates
			inversePos.y = 0;//move into same xz plane as us
			up = parent.up; //we're in local, not world space
			inversePos = parent.TransformPoint(inversePos); //bring back to worldspace
		}
		transform.LookAt(inversePos, up); //"up" prevents us from sitting upright in worldspace
	}
}
