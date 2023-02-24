using Godot;
using System;

public class EntityStateMachine : KinematicBody2D
{
    public EntityBaseState CurrentState;
    public EntityStateFactory _states;

    public PlayerController player;

}