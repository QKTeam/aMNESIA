using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour {

    [SerializeField] private SwitchConsciousController SwitchController;

    private void SetActiveFalse()
    {
        SwitchController.SetActive();
    }
}
