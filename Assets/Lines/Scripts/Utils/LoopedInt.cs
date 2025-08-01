
using UnityEngine;

[System.Serializable]
public class LoopedInt
{
    public int lowLimit; // inclusive
    public int highLimit; // exclusive

    private int val;
    public int Value
    {
        get => val;
        set
        {
            while (value < lowLimit)
            {
                int under = lowLimit - value;

                value = highLimit - under;
            }

            while (value >= highLimit)
            {
                int over = value - highLimit;

                value = lowLimit + over;;
            }

            val = value;
        }
    }

    public static implicit operator int(LoopedInt i) =>  i.Value;

    public LoopedInt(int _value, int _low, int _high)
    {
        if (_high <= _low)
        {
            Debug.LogWarning("Attempted to create LoopedInt with high limit below low limit! High limit has been automatically set to low+1.");
            _high = _low + 1;
        }

        lowLimit = _low;
        highLimit = _high;

        Value = _value;
    }
}