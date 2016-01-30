using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckInZone : MonoBehaviour
{
    public Transform staffPosition;
    public Transform waitingQueuePosition;
    public Transform spawnPosition;

    public int WaitingQueueSize = 5;

    [HideInInspector]
    public List<Jammer> waitingQueue = new List<Jammer>();
    [HideInInspector]
    public Staff assignedStaff;

    public void AddJammer()
    {
        // Generate Jammer
        // Spawn jammer
        // Notify jammer to start waiting
    }

    public void AssignStaff(Staff staff)
    {
        // Start processing
        // when finish
            // if no more jammers
                // stop working
            // else
                // continue
    }

    public void Process()
    {
        // TODO Notify first jammer
        // TODO Remove first jammer from queue
        // if there are more jammers
            // TODO Move jammers
    }

    public void OnClick()
    {

    }
}
