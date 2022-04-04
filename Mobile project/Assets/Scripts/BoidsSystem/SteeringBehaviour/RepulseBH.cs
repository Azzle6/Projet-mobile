using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RepulseBH", menuName = "Scriptables/SteeringBehaviour_SO/Repluse", order = 1)]
public class RepulseBH : SteeringBehaviour
{
    public LayerMask repulseLayer;
    public float repulseRadius;

}
