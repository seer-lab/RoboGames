public class CharacterStats{
    public bool FreeFall{get;set;}
    public float Speed {get;set;}
    public float ProjectileTime{get;set;}
    public int Energy{get;set;}
    public CharacterStats(bool reset = false){
        if (reset){
            FreeFall = true; 
            Speed = StatLib.speeds[StatLib.speeds.Length-2]; 
            ProjectileTime = StatLib.projectileDistance[StatLib.projectileDistance.Length-1]; 
            Energy = StatLib.energyLevels[0]; 
        }
    }

}