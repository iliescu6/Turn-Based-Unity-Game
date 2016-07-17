using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public static Inventory instance;

    public  List<Item> slots = new List<Item>();
    public  List<Item> equips = new List<Item>();
    public List<Item> shopItem = new List<Item>();

    public Item tempItem = new Item();

    public  bool showToolTip = false;

    public bool inven ;
    public  GameObject toolTip;
    public  GameObject[] slot, equip,shopGO;
    public  GameObject invPanel;


    // Use this for initialization
    void Awake()
    {
        MakeInstance();

            slot = new GameObject[20];
            equip = new GameObject[3];
        shopGO = new GameObject[15];
            //Assign the slot objects to the slot array
            for (int i = 0; i < 20; i++)
            {
                slots.Add(new Item());
                slot[i] = GameObject.Find("Button (" + i.ToString() + ")");
            }

        for (int i = 0; i <= 4; i++) {
            shopItem.Add(new Item("Chest","Chest",2,0,0,0,0,1,10));// +2 HP, +1 dmg- costs 10 gold
            shopGO[i] = GameObject.Find("Item (" + i.ToString() + ")");
        }

        for (int i = 5; i <= 9; i++)
        {
            shopItem.Add(new Item("Feet", "Feet", 1, 1, 3, 2, 0, 1, 10));// +1 HP,+1 Mana,+3 strength,+2 intel +1 dmg- costs 10 gold
            shopGO[i] = GameObject.Find("Item (" + i.ToString() + ")");
        }

        for (int i = 10; i <= 14; i++)
        {
            shopItem.Add(new Item("Superior Chest", "Chest", 5, 2, 0, 0, 2, 3, 20));// +6 HP,+2 mana,+2 agility, +3 dmg- costs 20 gold
            shopGO[i] = GameObject.Find("Item (" + i.ToString() + ")");
        }



        for (int i = 0; i < 3; i++)
            {
                equips.Add(new Item());
                
            }
        equip[0] = GameObject.Find("Head");
        equip[1] = GameObject.Find("Chest");
        equip[2] = GameObject.Find("Feet");

        toolTip = GameObject.Find("Tooltip");
        invPanel = GameObject.Find("Inventory Panel");
        slots[0] = new Item("Superior Chest", "Chest", 0, 0, 0, 0, 1, 1, 0);
        invPanel.SetActive(false);
        inven = false;

        UI.instance.confirmPanel.SetActive(false);
        UI.instance.shopPanel.SetActive(false);
    }

    void Update() {
        if (showToolTip)
        {
            toolTip.SetActive(true);
        }
        else if (showToolTip != true && toolTip.activeInHierarchy) {
            toolTip.SetActive(false);
        }
        if (inven == true && !invPanel.activeInHierarchy)
        {
            invPanel.SetActive(true);
        }
        else if (inven == false) {
            invPanel.SetActive(false);
        }
        if (inven)
        {
            for (int j = 0; j < 3; j++)
            {
                if (equips[j].itemName != null && equip[j] != null)
                {
                    string temp = equips[j].itemName;
                    equip[j].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + temp);
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (slots[i].itemName != null )
                {
                    //Get the name of the items in slot (list) and load the image for it on the slot (array/gameobject)
                    string temp = slots[i].itemName;
                    slot[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + temp);
                }
            }
        }
    }
    void MakeInstance() {
        if (instance != null) {
            Destroy(gameObject);
        }
        else{ instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
