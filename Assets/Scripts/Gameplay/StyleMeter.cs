using UnityEngine;

public class StyleMeter : MonoBehaviour
{
    private float _stylePoints;

    public void AddStylePoints(float points)
    {
        _stylePoints += points;
        _stylePoints = Mathf.Clamp01(_stylePoints);
    }

    public float GetStyleLevel()
    {
        return _stylePoints;
    }
}
