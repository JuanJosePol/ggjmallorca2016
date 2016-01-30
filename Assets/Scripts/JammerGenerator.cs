using UnityEngine;
using System.Collections;

public class JammerGenerator : MonoBehaviour
{
    public float generationFreq = 1;
    public Jammer jammerPrefab;

    private float timeSinceLastGeneration = 0;
    private CheckInZone checkInZone;

    void Awake()
    {
        checkInZone = FindObjectOfType<CheckInZone>();
    }

    void Update()
    {
        timeSinceLastGeneration += Time.deltaTime;

        if ((checkInZone.CanEnterJammer()) && (timeSinceLastGeneration > 1 / generationFreq))
        {
            timeSinceLastGeneration = 0;

            if (GameManager.instance.HasRoom)
            {
                Jammer jammer = Instantiate(jammerPrefab);
                jammer.transform.position = this.transform.position;
                checkInZone.AddJammer(jammer);
            }
        }
    }
}
