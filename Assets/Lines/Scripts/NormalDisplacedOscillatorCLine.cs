using CLines.Oscillation;
using System.Collections.Generic;
using UnityEngine;

namespace CLines.Lines
{

    [RequireComponent(typeof(LineRenderer))]
    public class NormalDisplacedOscillatorCLine : CLine, IBendToTargetsLine
    {
        protected override int DefaultResolution => 20;

        [SerializeField] private List<DisplacementOscillatorNormal> normals;

        protected override void OnValidation()
        {
            foreach (DisplacementOscillatorNormal dN in normals)
            {
                dN.Validate();
            }
        }

        protected override void ModifyPoints(Vector3[] _inPoints, out Vector3[] _outPoints)
        {
            _outPoints = new Vector3[Resolution];

            float lineLength = Vector3.Distance(_inPoints[0], _inPoints[1]);

            float segmentProg = 1f / (Resolution - 1);

            for (int i = 0; i < Resolution; i++)
            {
                float t = segmentProg * i;

                Vector3 initialPoint = Vector3.Lerp(_inPoints[0], _inPoints[1], t);
                Vector3 point = Vector3.Lerp(_inPoints[0], _inPoints[1], t);

                float distanceToOrigin = Vector3.Distance(_inPoints[0], initialPoint);
                float distanceToTarget = Vector3.Distance(_inPoints[1], initialPoint);

                if (normals != null && normals.Count > 0)
                {
                    foreach (DisplacementOscillatorNormal dN in normals)
                    {
                        dN.Validate();

                        float localCompression = Mathf.Max(float.Epsilon, dN.useWorldSpaceCompression ? (lineLength * dN.compression) : dN.compression);
                        float localOffset = dN.useWorldSpaceCompression ? dN.offset / lineLength : dN.offset;

                        Vector3 addition = dN.Evaluate(t, localOffset, localCompression, transform.rotation, distanceToOrigin, distanceToTarget);

                        point += addition;
                    }
                }

                _outPoints[i] = point;
            }
        }

    }

}