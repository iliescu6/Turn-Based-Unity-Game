using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public static ToolTip instance;
    private Text info;
    public int mana = 0, health = 0, strength = 0, agility = 0,intelligence=0,damage=0;
    public string itemname = "";

    void Awake()
    {
        MakeSingleton();
        info = transform.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(Input.mousePosition.x + 70, Input.mousePosition.y - 70, Input.mousePosition.z);
        //If the tooltip appears it will have its information updated
        if (Inventory.instance.showToolTip)
        {

            info.text = itemname + "\n";
            if (health != 0)
            {
                info.text = info.text + "Health: " + health.ToString() + "\n";
            }
            if (mana != 0)
            {
                info.text = info.text + "Mana: " + mana.ToString() + "\n";
            }
            if (strength != 0)
            {
                info.text = info.text + "Strength: " + strength.ToString() + "\n";
            }
            if (agility != 0)
            {
                info.text = info.text + "Agility: " + agility.ToString() + "\n";
            }
            if (intelligence != 0)
            {
                info.text = info.text + "Intelligence: " + intelligence.ToString() + "\n";
            }
            if (damage != 0)
            {
                info.text = info.text + "Damage: " + damage.ToString() + "\n";
            }
            
        }
    }
    void MakeSingleton() {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}