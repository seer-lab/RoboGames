
using UnityEngine;
public class LeaderboardData{
    public string PlayerName {get;set;}
    public int PlayerRank {get;set;}
    public int PlayerScore {get;set;}
    public bool IsPlayer{get;set;}
    public LeaderboardData(string name, int rank, int score, bool isPlayer = false){
        PlayerName = name; 
        PlayerRank = rank; 
        PlayerScore = score; 
        IsPlayer = isPlayer; 
    }
}

[System.Serializable]
public class LeaderboardDataDB{
    public string name;
    public string points;
    public string levelName;
    public string rank;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}