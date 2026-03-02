using UnityEngine;
using System;

public class PerfectDodgeDetector : MonoBehaviour
{
    public static event Action OnPerfectDodge;

    // This method would be called by some other script when a perfect dodge occurs
    public void TriggerPerfectDodge()
    {
        OnPerfectDodge?.Invoke();
    }
}