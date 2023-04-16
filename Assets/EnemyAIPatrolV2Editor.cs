using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAIPatrolV2))]
public class EnemyAIPatrolV2Editor : Editor
{
    private void OnSceneGUI()
    {
        EnemyAIPatrolV2 fov = (EnemyAIPatrolV2)target;
        
        //patrol range
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.patrolRange);
        //sight range
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.sightRange);
        //attack range
        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.attackRange);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.fovAngle / 2);
        
        //fov
        Handles.color = Color.blue;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.sightRange);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.sightRange);

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
