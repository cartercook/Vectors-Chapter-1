using UnityEngine;
using System.Collections;

/**
 * 
 * Circle drawing algorithm: https://en.wikipedia.org/wiki/Midpoint_circle_algorithm#Example
 */
public class GridOverlay : MonoBehaviour {
	
	public int centerX;
	public int centerY;
	public int centerZ;
	public int radius;
	
	private Material lineMaterial;
	private Color color = new Color(0.5f, 0.5f, 0.5f, 1f);

	protected void beginDrawing() {
		if( !lineMaterial ) { //create line material
			lineMaterial = new Material(
				@"Shader ""Lines/Colored Blended"" {
				SubShader {
					Pass {
						Blend SrcAlpha OneMinusSrcAlpha 
						ZWrite Off Cull Off Fog { Mode Off } 
						BindChannels {
							Bind ""vertex"", vertex Bind ""color"", color
						}
					}
				}
			}" );
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
		lineMaterial.SetPass (0); // set the current material
		GL.Begin(GL.LINES);
		GL.Color(color);
		GL.PushMatrix();
	}

	protected void endDrawing() {
		GL.PopMatrix();
		GL.End();
	}

	protected void drawCircle(int x0, int y0, int z0, int radius) {
		int x = radius;
		int z = 0;
		int radiusError = 1-x;
		
		while(x >= z) {
			/*    101
			 *   2   2
			 *  2     2
			 * 1       1
			 * 0   X   0
			 * 1       1
			 *  2     2
			 *   2   2
			 *    101
			 */
			//DRAW HORIZONTAL LINES
			//upper half
			GL.Vertex3(x0 - x, y0, z0 + z); //left-up
			GL.Vertex3(x0 + x, y0, z0 + z); //right-up
			GL.Vertex3(x0 - z, y0, z0 + x); //up-left
			GL.Vertex3(x0 + z, y0, z0 + x); //up-right
			//lower half
			GL.Vertex3(x0 - x, y0, z0 - z); //left-down
			GL.Vertex3(x0 + x, y0, z0 - z); //right-down
			GL.Vertex3(x0 - z, y0, z0 - x); //down-left
			GL.Vertex3(x0 + z, y0, z0 - x); //down-right
			//DRAW VERTICAL LINES
			//left half
			GL.Vertex3(x0 - x, y0, z0 + z); //left-up
			GL.Vertex3(x0 - x, y0, z0 - z); //left-down
			GL.Vertex3(x0 - z, y0, z0 + x); //up-left
			GL.Vertex3(x0 - z, y0, z0 - x); //down-left
			//right half
			GL.Vertex3(x0 + x, y0, z0 + z); //right-up
			GL.Vertex3(x0 + x, y0, z0 - z); //right-down
			GL.Vertex3(x0 + z, y0, z0 + x); //up-right
			GL.Vertex3(x0 + z, y0, z0 - x); //down-right
			z++;
			if (radiusError<0) {
				radiusError += 2*z + 1;
			} else {
				x--;
				radiusError += 2*(z - x) + 1;
			}
		}
	}

	protected void drawQuarterCircle(int x0, int y0, int z0, int radius) {
		int x = radius;
		int y = 0;
		int radiusError = 1-x;
		
		while(x >= y) {
			/*    101
			 *   2   2
			 *  2     2
			 * 1       1
			 * 0   X   0
			 * 1       1
			 *  2     2
			 *   2   2
			 *    101
			 */
			//DRAW HORIZONTAL LINES
			//upper half
			GL.Vertex3(x0, y0 + y, z0); //left-up
			GL.Vertex3(x0 + x, y0 + y, z0); //right-up
			GL.Vertex3(x0, y0 + x, z0); //up-left
			GL.Vertex3(x0 + y, y0 + x, z0); //up-right
			//DRAW VERTICAL LINES
			//right half
			GL.Vertex3(x0 + x, y0 + y, z0); //right-up
			GL.Vertex3(x0 + x, y0, z0); //right-down
			GL.Vertex3(x0 + y, y0 + x, z0); //up-right
			GL.Vertex3(x0 + y, y0, z0); //down-right
			y++;
			if (radiusError<0) {
				radiusError += 2*y + 1;
			} else {
				x--;
				radiusError += 2*(y - x) + 1;
			}
		}
	}

}
