using UnityEngine;
using System.Collections;

public class JammerGenerator : MonoBehaviour
{
    public float generationFreq = 1;
    public GameObject jammerPrefab;

    private float timeSinceLastGeneration = 0;

    void Update()
    {
        timeSinceLastGeneration += Time.deltaTime;

        if (timeSinceLastGeneration > 1 / generationFreq)
        {
            timeSinceLastGeneration = 0;

            if (GameManager.instance.HasRoom)
            {
                GameObject jammer = Instantiate(jammerPrefab);
                jammer.transform.position = this.transform.position;
            }
        }
    }
}
