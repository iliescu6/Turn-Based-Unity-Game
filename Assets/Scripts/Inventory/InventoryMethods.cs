using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMethods : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void EquipChange()
    {
        //Get the current gameObject to verify it's name
        GameObject button;
        button = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);


        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 20; i++)
            {

                GameObject temp = Inventory.instance.slot[i];
                if (button.name == temp.name)
                {
                    if (button.transform.GetChild(0).GetComponent<Image>().sprite.name != "blank")
                    {
                        //Get the type of item to determine where to equip it
                        string temp2 = Inventory.instance.slots[i].type;

                        //Find the equip type button
                        GameObject temp3 = GameObject.Find(temp2);


                        if (temp2 == Inventory.instance.equip[j].name && temp3.transform.GetChild(0).GetComponent<Image>().sprite.name == "blank")
                        {
                            Inventory.instance.equips[j] = Inventory.instance.slots[i];
                            ChangeImage(i, temp3, button);
                            AddStats(i);
                            Inventory.instance.slots[i] = new Item();
                            Inventory.instance.showToolTip = false;
                            break;
                        }
                        else if (temp2 == Inventory.instance.equip[j].name && temp3.transform.GetChild(0).GetComponent<Image>().sprite.name != "blank")
                        {
                            Item t;
                            string tempN;
                            TakeStats(j);
                            AddStats(i);
                            //Switch image between equip and slot if there's already an equiped item
                            tempN = temp3.transform.GetChild(0).GetComponent<Image>().sprite.name;
                            temp3.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + Inventory.instance.slots[i].itemName); //change equip image 
                            button.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + tempN);
                            t = Inventory.instance.equips[j];
                            Inventory.instance.equips[j] = Inventory.instance.slots[i];
                            Inventory.instance.slots[i] = t;

                            ToolTip.instance.itemname = Inventory.instance.slots[i].itemName;
                            ToolTip.instance.health = Inventory.instance.slots[i].health;
                            ToolTip.instance.mana = Inventory.instance.slots[i].mana;
                            ToolTip.instance.strength = Inventory.instance.slots[i].strength;
                            ToolTip.instance.agility = Inventory.instance.slots[i].agility;
                            ToolTip.instance.intelligence = Inventory.instance.slots[i].intelligence;
                            ToolTip.instance.damage = Inventory.instance.slots[i].damage;


                            break;

                        }
                    }
                }
            }
        }
    }

    public void Shop() {
        GameObject button = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);
        for (int i = 0; i <= 14; i++) {
            if (button.name == Inventory.instance.shopGO[i].name && Player.instance.gold >= Inventory.instance.shopItem[i].gold) {
                UI.instance.confirmPanel.SetActive(true);
                Text itemText = GameObject.Find("Confirm Text").GetComponent<Text>();
                itemText.text = "Are you sure you want to buy " + Inventory.instance.shopItem[i].itemName + " for " + Inventory.instance.shopItem[i].gold + " gold?";
                GameObject.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + Inventory.instance.shopItem[i].itemName);
                Inventory.instance.tempItem = Inventory.instance.shopItem[i];
            } else if (button.name == Inventory.instance.shopGO[i].name && Player.instance.gold < Inventory.instance.shopItem[i].gold) {
                UI.instance.insufficientPanel.SetActive(true);
                GameObject.Find("Insufficient Text").GetComponent<Text>().text="Insufficient gold!";
            }
        }
    }

    public void YesBuy() {
        bool space = false;
        for (int i = 0; i < 20; i++) {
            if (Inventory.instance.slots[i].itemName == null) {
                space = true;
            }
            if (space == true)
            {
                Inventory.instance.slots[i] = Inventory.instance.tempItem;
                Player.instance.gold -= Inventory.instance.tempItem.gold;
                UI.instance.confirmPanel.SetActive(false);
                break;
            }
        }
        if (space == false) {
            UI.instance.insufficientPanel.SetActive(true);
            GameObject.Find("Insufficient Text").GetComponent<Text>().text = "Inventory is full!";
        }
        
    }

    public void NoBuy() {
        UI.instance.confirmPanel.SetActive(false);
    }

    public void ClosePanel() {
        if (EventSystem.current.currentSelectedGameObject.name.Contains("Close Insufficient Button")) {
            UI.instance.insufficientPanel.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject.name.Contains("Back Button"))
        {
            UI.instance.shopPanel.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject.name.Contains("Close Inventory Button"))
        {
            Inventory.instance.inven = false;
        }
    }

    public void Unequip()
    {

        GameObject button = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);
        if (button.transform.GetChild(0).GetComponent<Image>().sprite.name != "blank")
        {
            for (int i = 0; i < 3; i++)
            {
                if (button.name == Inventory.instance.equip[i].name)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (Inventory.instance.slot[j].transform.GetChild(0).GetComponent<Image>().sprite.name == "blank")
                        {
                            Inventory.instance.slots[j] = Inventory.instance.equips[i];
                            TakeStats(i);
                            Inventory.instance.slot[j].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + Inventory.instance.equips[i].itemName);
                            Inventory.instance.equip[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/blank");
                            Inventory.instance.equips[i] = new Item();
                            Inventory.instance.showToolTip = false;
                            break;
                        }
                    }
                }
            }

        }
    }
    public void OnPointerEnter(PointerEventData evenData)
    {
        PoPUp();
    }

    public void OnPointerExit(PointerEventData evenData)
    {
        Inventory.instance.showToolTip = false;
    }



    public void ChangeImage(int i, GameObject t, GameObject b)
    {
        t.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + Inventory.instance.slots[i].itemName); //change equip image 
        b.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/Blank");


    }

    private void AddStats(int i)
    {
        Player.instance.player.maxHP += Inventory.instance.slots[i].health;
        Player.instance.player.maxMana += Inventory.instance.slots[i].mana;
        Player.instance.player.strength += Inventory.instance.slots[i].strength;
        Player.instance.player.agility += Inventory.instance.slots[i].agility;
        Player.instance.player.intelligence += Inventory.instance.slots[i].intelligence;
        Player.instance.player.dmg += Inventory.instance.slots[i].damage;
    }

    private void TakeStats(int i)
    {
        Player.instance.player.maxHP -= Inventory.instance.equips[i].health;
        Player.instance.player.maxMana -= Inventory.instance.equips[i].mana;
        Player.instance.player.strength -= Inventory.instance.equips[i].strength;
        Player.instance.player.agility -= Inventory.instance.equips[i].agility;
        Player.instance.player.intelligence -= Inventory.instance.equips[i].intelligence;
        Player.instance.player.dmg -= Inventory.instance.equips[i].damage;
    }

    public void PoPUp()
    {
        for (int i = 0; i < 4; i++)
        {
            if (UI.instance.skillArray[i] != null)
            {
                GameObject temp = UI.instance.skillArray[i];
                if (transform.name == temp.name)
                {

                    Inventory.instance.showToolTip = true;
                    ToolTip.instance.itemname = UI.instance.skill[i].description + "\n" + UI.instance.skill[i].manaCost.ToString()+" mana";


                }
            }
        }

        for (int i = 0; i < 15; i++)
        {
            if (Inventory.instance.shopGO[i] != null)
            {
                GameObject temp = Inventory.instance.shopGO[i];
                if (transform.name == temp.name)
                {
                    if (transform.GetChild(0).GetComponent<Image>().sprite.name != "blank")
                    {
                        Inventory.instance.showToolTip = true;
                        ToolTip.instance.itemname = Inventory.instance.shopItem[i].itemName;
                        ToolTip.instance.health = Inventory.instance.shopItem[i].health;
                        ToolTip.instance.mana = Inventory.instance.shopItem[i].mana;
                        ToolTip.instance.strength = Inventory.instance.shopItem[i].strength;
                        ToolTip.instance.agility = Inventory.instance.shopItem[i].agility;
                        ToolTip.instance.intelligence = Inventory.instance.shopItem[i].intelligence;
                        ToolTip.instance.damage = Inventory.instance.shopItem[i].damage;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < 20; i++)
        {
            if (Inventory.instance.slot[i] != null)
            {
                GameObject temp = Inventory.instance.slot[i];
                if (transform.name == temp.name)
                {
                    if (transform.GetChild(0).GetComponent<Image>().sprite.name != "blank")
                    {
                        Inventory.instance.showToolTip = true;
                        ToolTip.instance.itemname = Inventory.instance.slots[i].itemName;
                        ToolTip.instance.health = Inventory.instance.slots[i].health;
                        ToolTip.instance.mana = Inventory.instance.slots[i].mana;
                        ToolTip.instance.strength = Inventory.instance.slots[i].strength;
                        ToolTip.instance.agility = Inventory.instance.slots[i].agility;
                        ToolTip.instance.intelligence = Inventory.instance.slots[i].intelligence;
                        ToolTip.instance.damage = Inventory.instance.slots[i].damage;
                        break;
                    }
                }
            }
        }
        for (int j = 0; j < 3; j++)
        {
            Item temp = Inventory.instance.equips[j];
            if (transform.name == temp.type)
            {

                if (transform.GetChild(0).GetComponent<Image>().sprite.name != "blank")
                {
                    Inventory.instance.showToolTip = true;
                    ToolTip.instance.itemname = Inventory.instance.equips[j].itemName;
                    ToolTip.instance.health = Inventory.instance.equips[j].health;
                    ToolTip.instance.mana = Inventory.instance.equips[j].mana;
                    ToolTip.instance.strength = Inventory.instance.equips[j].strength;
                    ToolTip.instance.agility = Inventory.instance.equips[j].agility;
                    ToolTip.instance.intelligence = Inventory.instance.equips[j].intelligence;
                    ToolTip.instance.damage = Inventory.instance.equips[j].damage;
                    break;
                }
            }
        }
    }
}