using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public Transform spawnPoint;
    //public int characterIndex;
    public CommunicatorSO communicatorSO;
    // Start is called before the first frame update
    public void Awake()
    {   
        Debug.Log("Subhash is gay");
        int selectedIndex = communicatorSO.characterIndex; 
        GameObject Player = characters[selectedIndex];
        Instantiate(Player, spawnPoint.position , Quaternion.identity);
    }
}
