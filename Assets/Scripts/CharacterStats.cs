using System.Diagnostics;
/// <summary>
/// Stores the Stats of the player as they progress through the game.
/// Only one instance of this is kept in Global State.
/// </summary>
public class CharacterStats{
    public int XPBoost{get;set;}
    public float Speed {get;set;}
    public int Points {get;set;}
    public int Energy{get;set;}
    public float DamageLevel{get;set;}
    public CharacterStats(bool reset = false){
        if (reset){
            XPBoost = StatLib.xpboost[0]; 
            Speed = StatLib.speeds[0]; 
            Energy = StatLib.energyLevels[0];
            if (GlobalState.GameMode == stringLib.GAME_MODE_BUG){
                DamageLevel = StatLib.bug_damageLevels[0];
            }
            else DamageLevel = StatLib.on_damageLevels[0]; 
            Points = 0; 
        }
    }
    //Give the player more powers when playing. 
    public void GrantPower(){
        GlobalState.Stats.Speed = StatLib.speeds[3]; 
        GlobalState.Stats.Energy = StatLib.energyLevels[4]; 
    }

}