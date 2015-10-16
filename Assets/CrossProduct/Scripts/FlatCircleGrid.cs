using UnityEngine;
using System.Collections;

/**
 * 
 * Circle drawing algorithm: https://en.wikipedia.org/wiki/Midpoint_circle_algorithm#Example
 */
public class FlatCircleGrid : GridOverlay {

	void OnPostRender() {
		beginDrawing();
		drawCircle(centerX, centerY, centerZ, CrossMaster.instance.radius);
		endDrawing();
	}

}
