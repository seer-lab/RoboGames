public class CharacterStats{
    public bool FreeFall{get;set;}
    public float Speed {get;set;}
    public float ProjectileTime{get;set;}
    public int Energy{get;set;}
    public CharacterStats(bool reset = false){
        if (reset){
            FreeFall = false; 
            Speed = StatLib.speeds[0]; 
            ProjectileTime = StatLib.projectileDistance[0]; 
            Energy = StatLib.energyLevels[0]; 
        }
    }

}