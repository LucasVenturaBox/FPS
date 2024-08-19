using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerator DelaySeconds(float waitDuration, Action action)
    {
        yield return new WaitForSeconds(waitDuration);
        action?.Invoke();
    }
}
