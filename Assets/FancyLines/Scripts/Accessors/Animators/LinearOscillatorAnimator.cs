using FancyLines.Lines;
using FancyLines.Lines.InfoTypes;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LinearOscillatorAnimator : OscillatorAnimator
{
    [SerializeField] private float offsetSpeed;

    protected override void OnStart()
    {
        normal.keepChangedOffset = true;
    }

    private void LateUpdate()
    {
        normal.offset += offsetSpeed * Time.deltaTime;
    }
}
