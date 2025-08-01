using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace CLines.Lines
{

    [RequireComponent(typeof(LineRenderer))]
    public class NormalDisplacedCLine : CLine
    {
        [System.Serializable]
        private class DisplacementNormal : IBendToTargetsLine.BendableNormal
        {
            [Space(6)]
            public Vector3 normal;
            public bool worldSpaceNormal;
            public float distance;

            [CurveRange(0, -1, 1, 1)]
            public AnimationCurve displacement;
        }

        [SerializeField] private List<DisplacementNormal> normals;

        protected override int DefaultResolution => 10;

        protected override void OnValidation()
        {
            foreach (DisplacementNormal dN in normals)
            {
                dN.normal = dN.normal.normalized;
            }
        }

        protected override void ModifyPoints(Vector3[] _inPoints, out Vector3[] _outPoints)
        {
            _outPoints = new Vector3[Resolution];

            float segmentProg = 1f / (Resolution - 1);

            for (int i = 0; i < Resolution; i++)
            {
                float t = segmentProg * i;

                Vector3 initialPoint = Vector3.Lerp(_inPoints[0], _inPoints[1], t);
                Vector3 point = Vector3.Lerp(_inPoints[0], _inPoints[1], t);

                float distanceToOrigin = Vector3.Distance(_inPoints[0], initialPoint);
                float distanceToTarget = Vector3.Distance(_inPoints[1], initialPoint);

                foreach (DisplacementNormal dN in normals)
                {
                    Vector3 addition = (!dN.worldSpaceNormal ? transform.rotation : Quaternion.identity) * (dN.displacement.Evaluate(t) * dN.distance * dN.normal);

                    dN.ApplyBend(ref addition, t, distanceToOrigin, distanceToTarget);

                    point += addition;
                }

                _outPoints[i] = point;
            }
        }
    }

}