public class CharacterStats{
    public bool FreeFall{get;set;}
    public float Speed {get;set;}
    public float ProjectileTime{get;set;}
    public int Energy{get;set;}
    public CharacterStats(bool reset = false){
        if (reset){
<<<<<<< HEAD
            FreeFall = false; 
            Speed = StatLib.speeds[0]; 
            ProjectileTime = StatLib.projectileDistance[0]; 
            Energy = StatLib.energyLevels[2]; 
=======
            FreeFall = true; 
            Speed = StatLib.speeds[StatLib.speeds.Length-2]; 
            ProjectileTime = StatLib.projectileDistance[StatLib.projectileDistance.Length-1]; 
            Energy = StatLib.energyLevels[0]; 
>>>>>>> b6192106da89be4d33a67b7a8499cf8197faf37e
        }
    }

}