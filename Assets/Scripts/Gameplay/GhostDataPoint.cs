using UnityEngine;

[System.Serializable]
public class GhostDataPoint 
{
    public Vector3 position;
    public float time;
    
    public float timestamp => time;
    public bool isJumping;
    public bool isSliding;
}
