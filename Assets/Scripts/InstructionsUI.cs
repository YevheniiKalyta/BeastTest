using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI instructionsText;

    public void SetInstruction(InstructionsType instructionsType)
    {
        switch (instructionsType)
        {
            case InstructionsType.Interaction:
                instructionsText.text = "E - interact";
                break;
            case InstructionsType.Movement:
                instructionsText.text = "WASD to move";
                break;
        case InstructionsType.None:
        default:
            instructionsText.text = "";
                break;
        }
    }
}
