using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerChackable 
{
    bool IsAggroed { get; set; }
    bool IsWithinStrikingDistance { get; set; }
    bool IsEndGround { get; set; }
    bool IsNeedMoveRight { get; set; }
    void SetAggroStatus(bool isAggroed);
    void SetStrikingDistanceBool(bool isWithinStrikingDistance);
}
