using UnityEngine;
using System.Collections;

public class MoveableArrow: Arrow {

	private Master master;
	public bool controlable;
	public enum Axes {XY, XZ};
	public Axes planeAxes;
	private Plane plane;
	public int xBound;
	public int yBound;

	void Start() {
		master = Master.instance;
		controlable = true;
		switch(planeAxes)
		{
		case Axes.XY:
			plane = new Plane(Vector3.forward, 0);
			break;
		case Axes.XZ:
			plane = new Plane(Vector3.up, 0);
			break;
		}
	}
	
	void OnMouseDrag() {
		if (controlable) {
			//Find the distance from the mouse to the xz plane
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //shoots out of the camera starting at mouse pos
			float rayDistance; //used as the z for curScreenPos
			if (plane.Raycast(ray, out rayDistance)) { //if ray intersects plane
				Vector3 gridPoint = ray.GetPoint(rayDistance);
				int radius = CrossMaster.instance.radius; //save locally for easy access
				if (gridPoint.magnitude > radius) { //gridPoint is too far from origin
					gridPoint = gridPoint.normalized*radius; //scale it back down to the radius
				}
				if (gridPoint.x < xBound) { //gridPoint is out of bounds
					gridPoint.x = xBound; //bump it back to xBound
				}
				if (gridPoint.y < yBound) { //same as above but for y axis
					gridPoint.y = yBound;
				}
				//round to plane
				float yOrZ = float.NaN; //will hold either gridPoint.y or .z depending on planeAxes
				if (planeAxes == Axes.XY) {
					yOrZ = gridPoint.y;
				} else if (planeAxes == Axes.XZ) {
					yOrZ = gridPoint.z;
				}
				//if gridPoint == (0,0,0), make it a unit vector instead
				if (Mathf.Abs(gridPoint.x) <= 0.5 && Mathf.Abs(yOrZ) <= 0.5) { //if gridPoint == vector3.zero
					if (gridPoint.x >= yOrZ) {
						gridPoint.x = Mathf.Sign(gridPoint.x); //make gridPoint a unit vector along x axis
						yOrZ = 0f;
					} else {
						yOrZ = Mathf.Sign(yOrZ); //make gridPoint a unit vector along y or z axes
						gridPoint.x = 0f;
					}
				} else { //otherwise snap x and yOrZ to grid
					gridPoint.x = Mathf.Round(gridPoint.x);
					yOrZ = Mathf.Round(yOrZ);
				}
				//now reassign y or z back to yOrZ
				if (planeAxes == Axes.XY) {
					gridPoint.y = yOrZ;
				} else if (planeAxes == Axes.XZ) {
					gridPoint.z = yOrZ;
				}
				stretchTo(gridPoint); //stretch cylinder to where they intersected
				master.updateCrossProduct();
			}
		}
	}
}
