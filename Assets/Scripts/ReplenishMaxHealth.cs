using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ReplenishMaxHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerController player;
    private UnityAction UpdateText;
    TextMeshProUGUI costText;

    public int cost;
    void Start()
    {
        UpdateText += Purchased;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(UpdateText);

        updateCost();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health < player.maxHealth)
        {
            updateCost();
        }
    }

    public void Purchased()
    {
        if (player.healthDynamicSuccesful == true)
        {
            costText.text = "Already at Max Health";
            player.healthDynamicSuccesful = false;
        }
    }

    public void updateCost()
    {
        TextMeshProUGUI[] textlist = GetComponentsInChildren<TextMeshProUGUI>();

        costText = textlist[2];

        cost = (player.maxHealth - player.health) * 25;

        costText.text = "Cost: " + cost.ToString();
    }
}
