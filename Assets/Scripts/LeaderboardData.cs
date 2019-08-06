public class LeaderboardData{
    public string PlayerName {get;set;}
    public int PlayerRank {get;set;}
    public int PlayerScore {get;set;}
    public LeaderboardData(string name, int rank, int score){
        PlayerName = name; 
        PlayerRank = rank; 
        PlayerScore = score; 
    }
}