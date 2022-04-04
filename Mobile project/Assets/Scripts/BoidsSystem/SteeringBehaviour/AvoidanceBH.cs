using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidanceBH", menuName = "Scriptables/SteeringBehaviour_SO/Avoidance", order = 1)]
public class AvoidanceBH : SteeringBehaviour
{
    public int rayStep = 10;
    public float rayWidth = 1;
    public LayerMask obstacleMask;

}
