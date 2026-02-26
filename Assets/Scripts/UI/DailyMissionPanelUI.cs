using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SimpleObjectPool))]
public class DailyMissionPanelUI : MonoBehaviour
{
    [Header("UI Configuration")]
    [SerializeField] private Transform missionContainer;

    private SimpleObjectPool missionUIPool;
    private readonly List<GameObject> activeMissionUIObjects = new List<GameObject>();

    void Awake()
    {
        missionUIPool = GetComponent<SimpleObjectPool>();
    }

    void OnEnable()
    {
        DailyMissionManager.OnMissionsRefreshed += RefreshMissionDisplay;
    }

    void OnDisable()
    {
        DailyMissionManager.OnMissionsRefreshed -= RefreshMissionDisplay;
    }

    void Start()
    {
        RefreshMissionDisplay();
    }

    private void RefreshMissionDisplay()
    {
        foreach (GameObject uiObject in activeMissionUIObjects)
            missionUIPool.ReturnObject(uiObject);

        activeMissionUIObjects.Clear();

        if (DailyMissionManager.Instance == null ||
            DailyMissionManager.Instance.activeMissions == null) return;

        foreach (var missionState in DailyMissionManager.Instance.activeMissions)
        {
            GameObject missionObj = missionUIPool.GetObject();
            missionObj.transform.SetParent(missionContainer, false);
            missionObj.GetComponent<DailyMissionUI>().Setup(missionState);
            activeMissionUIObjects.Add(missionObj);
        }
    }
}
