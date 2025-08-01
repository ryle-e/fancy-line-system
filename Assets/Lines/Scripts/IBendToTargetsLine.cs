using NaughtyAttributes;
using UnityEngine;

namespace CLines
{
    public interface IBendToTargetsLine
    {
        public class BendableNormal
        {
            public bool bendToTargets; // should this curve point towards the targets at either end?
            [AllowNesting, ShowIf("bendToTargets")]
            public bool useBendToTargetDistance = false; // use a real distance rather than a progression through the line?
            [AllowNesting, ShowIf("ShowBendToTargetT"), Range(0, 0.5f)]
            public float bendToTargetThreshold = 0.1f; // progression through the line when it starts bending towards the targets
            [AllowNesting, ShowIf("ShowBendToTargetDistance")]
            public float bendToTargetDistance = 0.1f; // distance left in the line for it to start bending towards the targets
            [AllowNesting, ShowIf("bendToTargets")]
            public AnimationCurve bendToTargetCurve = AnimationCurve.Linear(0, 0, 1, 1); // curve to bend towards targets with


            private bool ShowBendToTargetT => bendToTargets && !useBendToTargetDistance;
            private bool ShowBendToTargetDistance => bendToTargets && useBendToTargetDistance;

            public void ApplyBend(ref Vector3 _addition, float _lineT, float _distanceToOrigin, float _distanceToTarget)
            {
                if (bendToTargets)
                {
                    if (useBendToTargetDistance)
                    {
                        _addition *= BendToTargetDistance(_distanceToOrigin, _distanceToTarget, bendToTargetDistance, bendToTargetCurve);
                    }
                    else
                    {
                        _addition *= BendToTargetT(_lineT, bendToTargetThreshold, bendToTargetCurve);
                    }
                }
            }

            // returns value to multiply with the point to bend it closer to either target
            public float BendToTargetT(float _t, float _bendThreshold, AnimationCurve _bendCurve)
            {
                if (_t < _bendThreshold)
                {
                    float bendPower = Mathf.Clamp01(_t / _bendThreshold);

                    return _bendCurve.Evaluate(bendPower);
                }
                else if (1 - _t < _bendThreshold)
                {
                    float bendPower = Mathf.Clamp01((1 - _t) / _bendThreshold);

                    return _bendCurve.Evaluate(bendPower);
                }

                return 1;
            }

            public float BendToTargetDistance(float _distanceToOrigin, float _distanceToTarget, float _bendDistance, AnimationCurve _bendCurve)
            {
                if (_distanceToOrigin < _bendDistance)
                {
                    float bendPower = Mathf.Clamp01(_distanceToOrigin / _bendDistance);

                    return _bendCurve.Evaluate(bendPower);
                }
                else if (_distanceToTarget < _bendDistance)
                {
                    float bendPower = Mathf.Clamp01(_distanceToTarget / _bendDistance);

                    return _bendCurve.Evaluate(bendPower);
                }

                return 1;
            }
        }
    }
}