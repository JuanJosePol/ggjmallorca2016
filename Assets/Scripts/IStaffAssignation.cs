using UnityEngine;
using System.Collections;

public interface IStaffAssignation
{
    void AssignStaff(Staff staff);
    void UnassignStaff();
    void Process();
    void OnStaffReady();
}
