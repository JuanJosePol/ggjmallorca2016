using UnityEngine;
using System.Collections;

public interface IStaffAssignation
{
    void AssignStaff(Staff newStaff);
    void UnassignStaff();
    void Process();
    void OnStaffReady();
    void OnClick();
}
