using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PurchasedButton : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerController player;
    public Weapons weaponsManager;

    private UnityAction UpdateText;

    void Start()
    {
        UpdateText += Purchased;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(UpdateText);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Purchased()
    {

        if (weaponsManager.purchaseSuccesful == true&& player.healthUpgrade==false)
        {
            TextMeshProUGUI[] textlist = GetComponentsInChildren<TextMeshProUGUI>();

            TextMeshProUGUI Cost = textlist[2];
            TextMeshProUGUI Requirements = textlist[3];

            Cost.text = "Cost: Already Purchased";
            Requirements.text = "Already Unlocked";
            weaponsManager.purchaseSuccesful = false;
        }

        else if(weaponsManager.purchaseSuccesful == true && player.healthUpgrade == true)
        {
            weaponsManager.purchaseSuccesful = false;
        }

        else
        {
            return;
        }

        
        
    }

    

}
