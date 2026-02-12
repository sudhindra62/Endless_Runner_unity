using UnityEngine;

public class AutoRunStarter : MonoBehaviour
{
    private void Start()
    {
        if (GameFlowController.Instance != null)
            GameFlowController.Instance.StartRun();
    }
}
