using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitForPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.state == GameManager.GameStates.Waiting)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Aguardando Jogadores";
        }
        else if (GameManager.Instance.state == GameManager.GameStates.Finished)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Fim de jogo!";
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "";

        }
    }
}
