using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public float Hunger { get; set; }
    public float Health { get; set; }
    public float Thirst { get; set; }
    public float Stamina { get; set; }

    public PlayerState(float hunger, float health, float thirst, float stamina)
    {
        Hunger = hunger;
        Health = health;
        Thirst = thirst;
        Stamina = stamina;
    }
}
