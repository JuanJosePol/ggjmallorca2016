using UnityEngine;
using System.Collections;

public class NavMeshNavigator : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.selectedStaff == null)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Clickable"))
                {
                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(hitInfo.point, out navHit, 0.1f, NavMesh.AllAreas))
                    {
                        Staff staff = GameManager.instance.selectedStaff;
                        staff.Unassign();
                        staff.walker.MoveTo(navHit.position, false);
                    }

                }
            }
        }
    }
}
