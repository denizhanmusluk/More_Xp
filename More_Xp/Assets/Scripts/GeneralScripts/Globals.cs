using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals 
{
    public static bool isGameActive = false, finish = false, followActive = true;
    public static int currentLevel = 1;
    public static int maxScore = 0;
    public static int currentLevelIndex = 0, LevelCount;
    public static int moneyAmount = 100000;


    public static int killedEnemy = 0;
    public static int maxEnemyCount = 150;

    // bash
    public static int bashLevel = 0;
    public static int bashCooldown = 10;
    public static int bashDamage = 10;
    public static int bashDistance = 10;
    public static int bashForce = 1;

    // stomp
    public static int stompLevel = 0;
    public static int stompCooldown = 15;
    public static int stompDamage = 1;
    public static int lightningAmount = 1;

    //spin
    public static int spinLevel = 0;
    public static int spinCooldown = 5;
    public static int spinDamage = 1;
    public static float spinTime = 3;

    //meteor
    public static int meteorLevel = 0;
    public static int meteorCooldown = 25;
    public static int meteorDamage = 1;
    public static float meteorTime = 3;

    // tornado
    public static int tornadoLevel = 0;
    public static int tornadoCooldown = 10;
    public static int tornadoDamage = 1;
    public static float tornadoDistance = 30;
    public static int tornadoForce = 1;

    // sowrd 
    public static int swordLevel = 0;
    public static int swordDamage = 1;
    public static float swordAttackSpeed = 1;

    // assasin
    public static int assassinLevel = 0;
    public static int assassinCooldown = 20;
    public static int assassinDamage = 1;
    public static int assassinAmount = 4;
}
