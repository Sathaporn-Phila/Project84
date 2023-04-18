using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolPath))]
public class PatrolPathEditor : Editor
{
    private void OnSceneGUI()
    {
        PatrolPath fov = (PatrolPath)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.sightRange);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.attackRange);
        //Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.attackRange);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.fovAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.sightRange);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.sightRange);

        Handles.color = Color.blue;
        Handles.DrawLine(fov.transform.position, fov.player.transform.position);

        if (fov.playerInSight)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
