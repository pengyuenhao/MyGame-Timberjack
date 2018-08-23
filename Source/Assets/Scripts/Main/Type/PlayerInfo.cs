using UnityEngine;
using System.Collections;
namespace MySpace.Info
{
    public class PlayerInfo
    {
        public bool isAlive = true;
        public bool isReset = false;
        public static int counter = 0;
        public string name;
        public int index;
        public int score;
        public float health;
        public float healthMax;
        public float healthLast;
        public GameObject unit;
        public GameObject tree;
        public GameObject scene;
        public PlayerInfo(string name=null)
        {
            if (name == null)
            {
                this.name = "Player" + counter;
            }
            index = counter;
            counter++;
        }
    }
}