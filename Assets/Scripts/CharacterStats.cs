using System.Diagnostics;
public class CharacterStats{
    public bool FreeFall{get;set;}
    public float Speed {get;set;}
    public int Points {get;set;}
    public float ProjectileTime{get;set;}
    public int Energy{get;set;}
    public CharacterStats(bool reset = false){
        if (reset){
            FreeFall = true; 
            Speed = StatLib.speeds[0]; 
            ProjectileTime = StatLib.projectileDistance[0]; 
            Energy = StatLib.energyLevels[0]; 
            Points = 0; 
        }
    }
    public void GrantPower(){
        GlobalState.Stats.Speed = StatLib.speeds[3]; 
        GlobalState.Stats.ProjectileTime = StatLib.projectileDistance[4]; 
        GlobalState.Stats.Energy = StatLib.energyLevels[4]; 
    }

}