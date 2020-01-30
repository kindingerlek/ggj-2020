using Utils.String;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelReplacer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var parameters = new
        {
            start_game = "Jogar",
            options = "Opções",
            credits = "Creditos",
            quit = "Sair",
            version = "1.0.1",
            year = "2020"
        };

        foreach (var text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
        {
            text.text = text.text.FormatWith(parameters);
        }

    }
}
