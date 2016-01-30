using UnityEngine;
using System.Collections;

public class Staff : MonoBehaviour
{
    [HideInInspector]
    public Walker walker;
	
	static string[] names   ={"Sergi Lorenzo", "Juanjo Pol", "Javi Cepa", "Elena Blanes", "Curro Campos", "Alberto Rico", "Alejandro Rico", "David Rico", "Jesús Fernández", "Espe Olea", "Aina Ferriol"};
	
	void GenerateJammerName() {
		name=names.GetRandom();
	}
	
    void Awake()
    {
        GameManager.instance.staff.Add(this);
        walker = gameObject.AddComponent<Walker>();
    }
}
