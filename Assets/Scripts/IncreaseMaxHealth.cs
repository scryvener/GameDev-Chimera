using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class IncreaseMaxHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerController player;
    private UnityAction UpdateText;
    TextMeshProUGUI costText;


    void Awake()
    {
        UpdateText += Purchased;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(UpdateText);

        updateCost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Purchased()
    {
        if (player.healthDynamicSuccesful == true)
        {
            updateCost();
            player.healthDynamicSuccesful = false;
        }
    }

    void updateCost()
    {
        TextMeshProUGUI[] textlist = GetComponentsInChildren<TextMeshProUGUI>();
        costText = textlist[2];

        int cost = player.maxHealthUpgradeCost;

        costText.text = "Cost: " + cost.ToString();
    }
}
