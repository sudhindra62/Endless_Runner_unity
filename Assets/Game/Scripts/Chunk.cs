using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Transform begin;
    public Transform end;

    private void OnDrawGizmos()
    {
        if(begin == null || end == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(begin.position, end.position);
    }
}
