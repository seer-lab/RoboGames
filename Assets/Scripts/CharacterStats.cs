using System.Diagnostics;
/// <summary>
/// Stores the Stats of the player as they progress through the game.
/// Only one instance of this is kept in Global State.
/// </summary>
public class CharacterStats{
    public bool FreeFall{get;set;}
    public float Speed {get;set;}
    public int Points {get;set;}
    public int Energy{get;set;}
    public float DamageLevel{get;set;}
    public CharacterStats(bool reset = false){
        if (reset){
            FreeFall = true; 
            Speed = StatLib.speeds[0]; 
            Energy = StatLib.energyLevels[0];
            DamageLevel = StatLib.damageLevels[0]; 
            Points = 0; 
        }
    }
    //Give the player more powers when playing. 
    public void GrantPower(){
        GlobalState.Stats.Speed = StatLib.speeds[3]; 
        GlobalState.Stats.Energy = StatLib.energyLevels[4]; 
    }

}