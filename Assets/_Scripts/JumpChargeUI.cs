using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpChargeUI : MonoBehaviour
{
    public Image chargeBarFill;
    public Player jumpScript;

    // Update is called once per frame
    void Update()
    {
        chargeBarFill.fillAmount = jumpScript.jumpValue / jumpScript.maxJumpValue;
    }
}
