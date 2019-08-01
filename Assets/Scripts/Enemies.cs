using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public abstract class Enemies : MonoBehaviour{

    public Vector3 Position {get;set;}
    protected int index; 
    public int Index{
        get{
            return index; 
        }
        set{
            index = value; 
        }
    }
    protected LevelGenerator lg; 
    protected CodeProperties properties; 
    public CodeProperties Properties{
        set{
            properties = value; 
        }
    }
    protected Transform position; 
    hero2Controller hero; 
    void Start(){
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();
        properties = lg.Properties; 
        position = GetComponent<Transform>();
        hero = GameObject.Find("Hero").GetComponent<hero2Controller>();
        InitializeEnemyMovement(); 
    }
    protected abstract IEnumerator MoveEnemy(); 
    public abstract void InitializeEnemyMovement(); 
    void UpdatePosition(){
        position.position = Position; 
    }
    protected abstract float GetDamage(); 
    protected abstract int GetCode();
    void OnTriggerEnter2D(Collider2D collidingObj){
        if (collidingObj.name == "Hero"){
            hero.onTakeDamage(GetDamage(), GetCode()); 
        }
    }
    void Update(){
        UpdatePosition(); 
    }
}