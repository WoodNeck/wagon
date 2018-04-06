using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidDelegate();
public delegate void GameObjectDelegate(GameObject gameObject);
public delegate void IntDelegate(int value);
public delegate void FloatDelegate(float value);

public enum Direction4{EAST = 0, WEST, NORTH, SOUTH};
public enum Direction8{EAST = 0, WEST, NORTH, SOUTH, NORTHWEST, NORTHEAST, SOUTHWEST, SOUTHEAST};

public class Direction{
    public Direction(){
        dir8 = Direction8.EAST;
    }
    public Direction(Direction4 dir){
        dir8 = Dir4ToDir8(dir);
    }
    public Direction(Direction8 dir){
        dir8 = dir;
    }
    public Direction4 dir4 {get{
        return Dir8ToDir4(dir8);
    }}
    public Direction8 dir8 {get; private set;}
    public void SetDirection(Direction8 dir){
        dir8 = dir;
    }
    public float ToAnimDirection(){
        Direction4 dir = dir4;
        float result;
        if (dir == Direction4.EAST || dir == Direction4.WEST){
            result = 0f;
        } else if (dir == Direction4.NORTH){
            result = 0.5f;
        } else {
            result = 1f;
        }
        return result;
    }

    public static Direction operator - (Direction direction){
        Direction result = new Direction();
        if      (direction.dir8 == Direction8.EAST)      result.dir8 = Direction8.WEST;
        else if (direction.dir8 == Direction8.NORTH)     result.dir8 = Direction8.SOUTH;
        else if (direction.dir8 == Direction8.SOUTH)     result.dir8 = Direction8.NORTH;
        else if (direction.dir8 == Direction8.WEST)      result.dir8 = Direction8.EAST;
        else if (direction.dir8 == Direction8.NORTHEAST) result.dir8 = Direction8.SOUTHWEST;
        else if (direction.dir8 == Direction8.NORTHWEST) result.dir8 = Direction8.SOUTHEAST;
        else if (direction.dir8 == Direction8.SOUTHEAST) result.dir8 = Direction8.NORTHWEST;
        else if (direction.dir8 == Direction8.SOUTHWEST) result.dir8 = Direction8.NORTHEAST;
        return direction;
    }

    public static Vector2 east = Vector2.right;
    public static Vector2 west = Vector2.left;
    public static Vector2 north = Vector2.up;
    public static Vector2 south = Vector2.down;
    public static Vector2 northwest = new Vector2(-1, 1);
    public static Vector2 northeast = Vector2.one;
    public static Vector2 southwest = new Vector2(-1, -1);
    public static Vector2 southeast = new Vector2(1, -1);

    public static Vector2 ToVector(Direction4 dir){
        Vector2 direction = east;
        if      (dir == Direction4.EAST)      direction = east;
        else if (dir == Direction4.NORTH)     direction = north;
        else if (dir == Direction4.SOUTH)     direction = south;
        else if (dir == Direction4.WEST)      direction = west;
        return direction;
    }

    public static Vector2 ToVector(Direction8 dir){
        Vector2 direction = east;
        if      (dir == Direction8.EAST)      direction = east;
        else if (dir == Direction8.NORTH)     direction = north;
        else if (dir == Direction8.SOUTH)     direction = south;
        else if (dir == Direction8.WEST)      direction = west;
        else if (dir == Direction8.NORTHEAST) direction = northeast;
        else if (dir == Direction8.NORTHWEST) direction = northwest;
        else if (dir == Direction8.SOUTHEAST) direction = southeast;
        else if (dir == Direction8.SOUTHWEST) direction = southwest;
        return direction;
    }

    public static Quaternion ToDegree(Direction8 dir){
        Quaternion degree = Quaternion.identity;
        if      (dir == Direction8.EAST)      degree = Quaternion.Euler(0, 0, 0);
        else if (dir == Direction8.NORTH)     degree = Quaternion.Euler(0, 0, 90);
        else if (dir == Direction8.SOUTH)     degree = Quaternion.Euler(0, 0, 270);
        else if (dir == Direction8.WEST)      degree = Quaternion.Euler(0, 0, 180);
        else if (dir == Direction8.NORTHEAST) degree = Quaternion.Euler(0, 0, 45);
        else if (dir == Direction8.NORTHWEST) degree = Quaternion.Euler(0, 0, 135);
        else if (dir == Direction8.SOUTHEAST) degree = Quaternion.Euler(0, 0, 315);
        else if (dir == Direction8.SOUTHWEST) degree = Quaternion.Euler(0, 0, 225);
        return degree;
    }

    public static Direction4 ToDirection4(Vector2 velocity){
        Direction4 dir = Direction4.EAST;
        if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y)){
            if (velocity.x > 0) dir = Direction4.EAST;
            else                dir = Direction4.WEST;
        } else {
            if (velocity.y > 0) dir = Direction4.NORTH;
            else                dir = Direction4.SOUTH;
        }
        return dir;
    }

    public static Direction8 ToDirection8(Vector2 velocity){
        Direction8 dir = Direction8.EAST;
        float dirSize = Vector2.Angle (Vector2.right, velocity);
		float dirSign = Mathf.Sign (Vector2.Dot (Vector2.up, velocity));
		float dir360 = dirSign * dirSize;
		if (dir360 < 0) dir360 += 360;

        if      (dir360 <= 22.5 || dir360 > 337.5) dir = Direction8.EAST;
        else if (dir360 <= 67.5)                   dir = Direction8.NORTHEAST;
        else if (dir360 <= 112.5)                  dir = Direction8.NORTH;
        else if (dir360 <= 157.5)                  dir = Direction8.NORTHWEST;
        else if (dir360 <= 202.5)                  dir = Direction8.WEST;
        else if (dir360 <= 247.5)                  dir = Direction8.SOUTHWEST;
        else if (dir360 <= 292.5)                  dir = Direction8.SOUTH;
        else if (dir360 <= 337.5)                  dir = Direction8.SOUTHEAST;

        return dir;
    }
    public static Direction4 Dir8ToDir4(Direction8 dir){
        Direction4 result;
        if (dir == Direction8.EAST)              result = Direction4.EAST;
        else if (dir == Direction8.NORTH)        result = Direction4.NORTH;
        else if (dir == Direction8.SOUTH)        result = Direction4.SOUTH;
        else if (dir == Direction8.WEST)         result = Direction4.WEST;
        else if (dir == Direction8.NORTHEAST)    result = Direction4.EAST;
        else if (dir == Direction8.NORTHWEST)    result = Direction4.WEST;
        else if (dir == Direction8.SOUTHEAST)    result = Direction4.EAST;
        else                                     result = Direction4.WEST;
        return result;
    }

    public static Direction8 Dir4ToDir8(Direction4 dir){
        Direction8 result;
        if (dir == Direction4.EAST)              result = Direction8.EAST;
        else if (dir == Direction4.NORTH)        result = Direction8.NORTH;
        else if (dir == Direction4.SOUTH)        result = Direction8.SOUTH;
        else                                     result = Direction8.WEST;
        return result;
    }
}

public static class AnimatorExtension {
     public static void SetTriggerOneFrame(this Animator anim, string trigger) {
        StaticCoroutine.Start(TriggerOneFrame(anim, trigger));
    }
 
    private static IEnumerator TriggerOneFrame(Animator anim, string trigger) {
        anim.SetTrigger(trigger);
        yield return null;
        if (anim != null) {
            anim.ResetTrigger(trigger);
        }
    }
}

public class StaticCoroutine : MonoBehaviour {
    private static StaticCoroutine mInstance = null;
 
    private static StaticCoroutine instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(StaticCoroutine)) as StaticCoroutine;
 
                if (mInstance == null)
                {
                    mInstance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
                }
            }
            return mInstance;
        }
    }
 
    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as StaticCoroutine;
        }
    }
 
    IEnumerator Perform(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
        Die();
    }
 
    public static void Start(IEnumerator coroutine)
    {
        instance.StartCoroutine(instance.Perform(coroutine));    
    }
    
    void Die()
    {
        mInstance = null;
        Destroy(gameObject);
    }
 
    void OnApplicationQuit()
    {
        mInstance = null;
    }
}