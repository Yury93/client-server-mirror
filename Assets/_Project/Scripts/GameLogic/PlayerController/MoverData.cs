using System;
using UnityEngine;

namespace Assets._Project.Scripts.GameLogic.PlayerController
{
    [Serializable]
    public struct MoverData
    {
        [field: SerializeField] public float MoveSpeed { get; set; }
        [field: SerializeField] public float SprintSpeed { get; set; }
        [field: SerializeField] public float JumpHeight { get; set; }
        [field: SerializeField] public float Gravity { get; set; }
        [field: SerializeField] public float JumpTimeout { get; set; }
        [field: SerializeField] public float GroundedOffset { get; set; }
        [field: SerializeField] public float GroundedRadius { get; set; }
        [field: SerializeField] public LayerMask GroundLayers { get; set; }
        [field: SerializeField] public float RotationLerpSpeed { get; set; }
    }
}