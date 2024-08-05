using System.Numerics;

namespace EmsMathRepo
{
    public class MagicFunctions
    {
        public Vector3 MoveAlongBezierCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
            return p;
        }
    }
}
