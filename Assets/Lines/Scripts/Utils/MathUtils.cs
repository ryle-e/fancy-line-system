using UnityEngine;

public static class MathUtils
{
    public static float Remap(float _n, float _low1, float _high1, float _low2, float _high2)
    {
        return _low2 + (_n - _low1) * (_high2 - _low2) / (_high1 - _low1);
    }

    public static float RemapClamped(float _n, float _low1, float _high1, float _low2, float _high2)
    {
        _n = Mathf.Clamp(_n, _low1, _high1);

        return Remap(_n, _low1, _high1, _low2, _high2);
    }
}