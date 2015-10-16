using UnityEngine;
using System.Collections;

public class QuarterCircleGrid : GridOverlay {

	// Update is called once per frame
	void OnPostRender () {
		beginDrawing();
		drawQuarterCircle(centerX, centerY, centerZ, DotMaster.instance.radius);
		endDrawing();
	}
}
