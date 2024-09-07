using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    public float Hunger { get; set; }
    public float Health { get; set; }
    public float Thirst { get; set; }
    public float Stamina { get; set; }

    public PlayerStatus(float hunger, float health, float thirst, float stamina)
    {
        Hunger = hunger;
        Health = health;
        Thirst = thirst;
        Stamina = stamina;
    }
}
