using Godot;
using System;
using System.Collections.Generic;

public class Global : Node
{
    public Vector2 previousPlayerPos;

    public const int coinsInGame = 69;

    public int cootsCoins;
    public int playerHealth = 100;

    public List<RatState> mainRoomRats;
    public List<BoxState> mainRoomBoxes;

    public List<RatState> room1Rats;
    public List<BoxState> room1Boxes;

    public List<RatState> puzzleRoomRats;
    public List<BoxState> puzzleRoomBoxes;

    public bool puzzleBlockage = true;

    public Vector2 mainPosition = Vector2.Zero;
    public Vector2 room1Position = Vector2.Zero;
    public Vector2 puzzlePosition = Vector2.Zero;

    public SceneName sceneName;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mainRoomRats = new List<RatState>();
        mainRoomBoxes = new List<BoxState>();

        room1Rats = new List<RatState>();
        room1Boxes = new List<BoxState>();

        puzzleRoomRats = new List<RatState>();
        puzzleRoomBoxes = new List<BoxState>();

        puzzleBlockage = true;
    }

    public void reloadSceneItems(List<RatState> ratList, List<BoxState> boxList)
    {
        if (ratList.Count > 0)
        {
            foreach (var globalRat in ratList)
            {
                var ratChildren = GetNode("/root/Node2D/rats").GetChildren();
                foreach (Node rat in ratChildren)
                {
                    if (rat.GetType() == typeof(RatStateMachine) && globalRat.name == rat.Name)
                    {
                        RatStateMachine r = (RatStateMachine)rat;
                        r.GlobalPosition = globalRat.position;
                        r.health = globalRat.health;
                        r.reload();
                    }
                }
            }
        }
        if (boxList.Count > 0)
        {
            foreach (var globalBox in boxList)
            {
                var ratChildren = GetNode("/root/Node2D/boxes").GetChildren();
                foreach (Node box in ratChildren)
                {
                    if (box.GetType() == typeof(Box) && globalBox.name == box.Name)
                    {
                        Box b = (Box)box;
                        b.GlobalPosition = globalBox.position;
                        b.broken = globalBox.broken;
                        b.reload();
                    }
                }
            }
        }
    }

    public class BoxState
    {
        public string name;
        public Vector2 position;
        public bool broken;

        public BoxState(Vector2 pos, bool broken, string name)
        {
            this.name = name;
            this.position = pos;
            this.broken = broken;
        }
    }

    public class RatState
    {
        public string name;
        public Vector2 position;
        public int health;
        public bool dead;

        public RatState(Vector2 pos, int health, string name)
        {
            this.name = name;
            this.position = pos;
            this.health = health;
            this.dead = health <= 0;
        }
    }

}

public enum SceneName
{
    Main,
    Room1,
    Puzzle,
    Boss
}
