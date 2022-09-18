using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime
{
	public static class RaycastPlane
	{

		private static Plane? _rayPlane;

		public static Plane RayPlane => _rayPlane ??= new Plane(Vector3.up, 0);

		public static Vector3 QueryPlane()
		{
			Ray mouseRay = CameraHelper.GetMouseRay();
			RayPlane.Raycast(mouseRay, out float distance);

			return mouseRay.GetPoint(distance);
		}

		public static Vector2 QueryPlaneAsXY()
		{
			Vector3 navPlanePoint = QueryPlane();

			return new Vector2(navPlanePoint.x, navPlanePoint.z);
		}

		public enum SnappingMode
		{
			NONE,
			HALF,
			WHOLE,
			HEXAGON
		}
	}
}
