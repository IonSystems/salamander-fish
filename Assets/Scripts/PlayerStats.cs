using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    

    public int maxHealth = 100;
    public int health;
	public int maxArmour = 100;
	public int armour;

    //store health and armour values
    public int saveArmour;
    public int saveHealth;

    public int healthRegenRate = 1;


    public void saveStats()
    {
        saveArmour = armour;
        saveHealth = health;
        PlayerPrefs.SetInt("Armour", saveArmour);
        PlayerPrefs.SetInt("Health", saveHealth);
    }

    public int getArmourOnLoad()
    {
        return PlayerPrefs.GetInt("Armour");
    }

    public int getHealthOnLoad()
    {
        return PlayerPrefs.GetInt("Health");
    }

	public void armourPickup(){
		if (armour < maxArmour - 10) {
			armour += 10;
        }
        else
        {
            armour = maxHealth;
        }

	}

	public void healthPickup(){
		if (health < maxHealth) {
			health++;
		}
	}

	public void injury(int damage){
		if (damage > armour) {
			damage -= armour;
			armour = 0;
		} else {
			armour -= damage;
			damage = 0;
		}

		if (damage > health) {
			health = 0;
		} else {
			health -= damage;
		}
			
	}


    void Start()
    {
		health = 100;
		armour = 100;
    }
}
