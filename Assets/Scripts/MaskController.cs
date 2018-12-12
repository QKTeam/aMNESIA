using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour {

    [SerializeField] private SwitchConsciousController SwitchController;
    [SerializeField] private Transform player;

    private void Update()
    {
        transform.position = player.position;
    }

    private void FreezeMap()
    {
        SwitchController.HideMap();
        SwitchController.RestoreCollider();
    }
}
