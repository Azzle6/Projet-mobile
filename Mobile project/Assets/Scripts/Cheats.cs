using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RessourceManager_LAC.instance.matter += 100;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            RessourceManager_LAC.instance.knowledge += 100;
        }
    }
}
