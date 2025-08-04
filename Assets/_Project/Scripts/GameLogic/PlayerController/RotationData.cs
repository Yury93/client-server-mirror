using System;
using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    [Serializable]
    public struct RotationData
    {
        [field:SerializeField] public float SensivityHorizontal { get; set; }
        [field: SerializeField] public float SensivityVertical { get; set; }
        [field: SerializeField] public float BottomClamp { get;   set; }
        [field: SerializeField] public float TopClamp { get;   set; }
    }
}