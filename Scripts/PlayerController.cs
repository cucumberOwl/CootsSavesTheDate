using Godot;
using System;
using System.Collections.Generic;

public class PlayerController : KinematicBody2D
{
    private int speed = 600;
    private float friction = .1f;
    private float acceleration = .25f;
    private float sensitivity = 0;
    private Vector2 direction;
    private Vector2 dashVelocity;

    public int health = 100;

    private Vector2 knockBack;
    private float knockbackSpeed;

    private ColorRect HealthTop;
    private ColorRect HealthBottom;

    private AnimatedSprite attackAnim;
    private AnimatedSprite cootsAnim;
    private MusicController mc;
    private Global global;

    public Position2D ZindexPos;

    private Position2D attack_N;
    private Position2D attack_S;
    private Position2D attack_W;
    private Position2D attack_E;
    private Position2D attack_NE;
    private Position2D attack_NW;
    private Position2D attack_SE;
    private Position2D attack_SW;

    private HUD hud;
    private bool blink;

    private float attackCooldown = 0;
    private float dashCooldown = 0;
    private float damageEfftect = 0;
    private float isAttacking = 0;
    private float dashAgain = 0;
    private float faintTimer = 2f;

    public Position2D currentAttack;

    public int attackDamage = 25;

    private bool canAttack;

    [Export]
    public float invicibleTimer = 0.5f;

    public float damagedControls = 0.2f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        knockBack = Vector2.Zero;
        //HealthTop = GetNode<Camera2D>("Camera2D").GetNode<Sprite>("HealthBar").GetNode<ColorRect>("HealthTop");
        //HealthBottom = GetNode<Camera2D>("Camera2D").GetNode<Sprite>("HealthBar").GetNode<ColorRect>("HealthBottom");
        hud = GetNode<HUD>("HUD");

        attackAnim = GetNode<AnimatedSprite>("AttackAnim");
        attack_N = GetNode<Position2D>("Attack_N");
        attack_S = GetNode<Position2D>("Attack_S");
        attack_W = GetNode<Position2D>("Attack_W");
        attack_E = GetNode<Position2D>("Attack_E");
        attack_NE = GetNode<Position2D>("Attack_NE");
        attack_NW = GetNode<Position2D>("Attack_NW");
        attack_SE = GetNode<Position2D>("Attack_SE");
        attack_SW = GetNode<Position2D>("Attack_SW");

        cootsAnim = GetNode<AnimatedSprite>("AnimatedSprite");

        mc = GetNode<MusicController>("/root/MusicController");
        ZindexPos = GetNode<Position2D>("ZIndex");
        global = GetNode<Global>("/root/Global");

        canAttack = true;
        hud.updateCoins(global.cootsCoins, Global.coinsInGame);

        GD.Print(global.playerHealth);
        GD.Print(global.playerHealth);

        if (global.playerHealth <= 0)
            global.playerHealth = 100;
        health = global.playerHealth;
        hud.setHealth(health);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ZIndex = (int)ZindexPos.GlobalPosition.y;

        if (damageEfftect > 0)
        {
            damageEfftect -= delta;
            if (blink)
                cootsAnim.Modulate = new Color(1, 0, 0, 0.8f);
            else
                cootsAnim.Modulate = new Color(1, 1, 1, 1);
            blink = !blink;
        }
        else if (damageEfftect <= 0)
        {
            cootsAnim.Modulate = new Color(1, 1, 1, 1);
        }

        if (health <= 0)
        {
            cootsAnim.Play("faint");
            if (faintTimer <= 0)
            {
                if (global.sceneName != SceneName.Boss)
                    saveUpdates();
                GetTree().ReloadCurrentScene();
            }
            faintTimer -= delta;
            return;
        }

        if (knockBack != Vector2.Zero)
        {
            MoveAndSlide(knockBack * knockbackSpeed, Vector2.Up);
            knockbackSpeed -= 5;

            if (knockbackSpeed <= 0)
            {
                knockBack = Vector2.Zero;
                knockbackSpeed = 0;
            }
            return;
        }
        // `velocity` will be a Vector2 between `Vector2(-1.0, -1.0)` and `Vector2(1.0, 1.0)`.
        // This handles deadzone in a correct way for most use cases.
        // The resulting deadzone will have a circular shape as it generally should.
        Vector2 velocity = InputGetVector("move_left", "move_right", "move_up", "move_down");
        if (dashCooldown <= 0)
        {
            Animate(velocity);
            if (velocity.y > sensitivity || velocity.y < (-1 * sensitivity))
            {
                velocity.y = Mathf.Lerp(velocity.y, velocity.y * speed, acceleration);
            }
            else
            {
                velocity.y = Mathf.Lerp(velocity.y, 0, friction);
            }

            if (velocity.x > sensitivity || velocity.x < (-1 * sensitivity))
            {
                velocity.x = Mathf.Lerp(velocity.x, velocity.x * speed, acceleration);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, friction);
            }
            MoveAndSlide(velocity, Vector2.Up);
        }
        else
        {
            MoveAndSlide(dashVelocity, Vector2.Up);
        }

        if (Input.IsActionPressed("attack") && attackCooldown <= 0 && canAttack)
        {
            attackCooldown = 0.4f;
            Attack();
            canAttack = false;
            playAttackSound();
        }

        if (Input.IsActionPressed("dash") && dashCooldown <= 0 && dashAgain <= 0 && canAttack)
        {
            dashCooldown = 0.4f;
            dashAgain = 0.7f;
            dashVelocity = direction.Normalized() * 300;
            Dash();
            canAttack = false;
            playAttackSound();
        }

        if (Input.IsActionJustReleased("attack"))
        {
            canAttack = true;
        }
        if (Input.IsActionJustReleased("dash"))
        {
            canAttack = true;
        }

        if (isAttacking <= 0)
        {
            currentAttack = null;
        }
        if (isAttacking > 0)
        {
            isAttacking -= delta;
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= delta;
        }
        if (dashCooldown > 0)
        {
            dashCooldown -= delta;
        }
        if (dashAgain > 0)
        {
            dashAgain -= delta;
        }
    }

    public Vector2 InputGetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = 0.5f)
    {
        var strength = new Vector2(
            Input.GetActionStrength(positiveX) - Input.GetActionStrength(negativeX),
            Input.GetActionStrength(positiveY) - Input.GetActionStrength(negativeY)
        ).Normalized();
        return strength.Length() > deadzone ? strength : Vector2.Zero;
    }

    private void Animate(Vector2 dir)
    {
        if (dir.x <= sensitivity && dir.x >= (-1 * sensitivity) && dir.y <= sensitivity && dir.y >= (-1 * sensitivity))
        {
            if (direction.x > 0.25 && direction.y > 0.25)
            {
                cootsAnim.Play("idle_right_down");
                return;
            }
            if (direction.x > 0.25 && direction.y < -0.25)
            {
                cootsAnim.Play("idle_right_up");
                return;
            }
            if (direction.x < -0.25 && direction.y > 0.25)
            {
                cootsAnim.Play("idle_left_down");
                return;
            }
            if (direction.x < -0.25 && direction.y < -0.25)
            {
                cootsAnim.Play("idle_left_up");
                return;
            }

            if (Math.Abs(direction.x) >= Math.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    cootsAnim.Play("idle_right");
                }
                else
                {
                    cootsAnim.Play("idle_left");
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    cootsAnim.Play("idle_down");
                }
                else
                {
                    cootsAnim.Play("idle_up");
                }
            }
            return;
        }
        direction = dir;
        if (Math.Abs(dir.x) >= 0.25 && Math.Abs(dir.y) >= .25)
        {
            if (dir.x > 0.25 && dir.y > 0.25)
                cootsAnim.Play("right_down");
            if (dir.x > 0.25 && dir.y < -0.25)
                cootsAnim.Play("right_up");
            if (dir.x < -0.25 && dir.y > 0.25)
                cootsAnim.Play("left_down");
            if (dir.x < -0.25 && dir.y < -0.25)
                cootsAnim.Play("left_up");
            return;
        }

        if (Math.Abs(dir.x) >= Math.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                cootsAnim.Play("right");
            }
            else
            {
                cootsAnim.Play("left");
            }
        }
        else
        {
            if (dir.y > 0)
            {
                cootsAnim.Play("down");
            }
            else
            {
                cootsAnim.Play("up");
            }
        }
    }

    private void Dash()
    {
        attackAnim.FlipH = false;
        attackAnim.FlipV = false;
        attackAnim.Frame = 0;
        isAttacking = 0.2f;
        if (Math.Abs(direction.x) >= 0.25 && Math.Abs(direction.y) >= 0.25)
        {
            if (direction.x > 0.25 && direction.y > 0.25) //SE
            {
                attackAnim.Position = attack_SE.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_right_down");
                cootsAnim.Play("dash");
                currentAttack = attack_SE;
            }
            if (direction.x > 0.25 && direction.y < -0.25) //NE
            {
                attackAnim.Position = attack_NE.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_right_up");
                cootsAnim.Play("dash");
                currentAttack = attack_NE;
            }
            if (direction.x < -0.25 && direction.y > 0.25) //SW
            {
                attackAnim.Position = attack_SW.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_left_down");
                cootsAnim.Play("dash");
                currentAttack = attack_SW;
            }
            if (direction.x < -0.25 && direction.y < -0.25) //NW
            {
                attackAnim.Position = attack_NW.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_left_up");
                cootsAnim.Play("dash");
                currentAttack = attack_NW;
            }
            return;
        }

        if (Math.Abs(direction.x) >= Math.Abs(direction.y))
        {
            if (direction.x > 0) // E
            {
                attackAnim.Position = attack_E.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_right");
                cootsAnim.Play("dash");
                currentAttack = attack_E;
            }
            else // W
            {
                attackAnim.Position = attack_W.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_left");
                cootsAnim.Play("dash");
                currentAttack = attack_W;
            }
        }
        else
        {
            if (direction.y > 0) // S
            {
                attackAnim.Position = attack_S.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_down");
                cootsAnim.Play("dash");
                currentAttack = attack_S;
            }
            else // N
            {
                attackAnim.Position = attack_N.Position;
                attackAnim.Play("attack");
                //cootsAnim.Play("dash_up");
                cootsAnim.Play("dash");
                currentAttack = attack_N;
            }
        }
    }

    private void Attack()
    {
        attackAnim.FlipH = false;
        attackAnim.FlipV = false;
        attackAnim.Frame = 0;
        isAttacking = 0.2f;
        if (Math.Abs(direction.x) >= 0.25 && Math.Abs(direction.y) >= 0.25)
        {
            if (direction.x > 0.25 && direction.y > 0.25) //SE
            {
                attackAnim.Position = attack_SE.Position;
                attackAnim.Play("attack");
                currentAttack = attack_SE;
            }
            if (direction.x > 0.25 && direction.y < -0.25) //NE
            {
                attackAnim.Position = attack_NE.Position;
                attackAnim.Play("attack");
                currentAttack = attack_NE;
            }
            if (direction.x < -0.25 && direction.y > 0.25) //SW
            {
                attackAnim.Position = attack_SW.Position;
                attackAnim.Play("attack");
                currentAttack = attack_SW;
            }
            if (direction.x < -0.25 && direction.y < -0.25) //NW
            {
                attackAnim.Position = attack_NW.Position;
                attackAnim.Play("attack");
                currentAttack = attack_NW;
            }
            return;
        }

        if (Math.Abs(direction.x) >= Math.Abs(direction.y))
        {
            if (direction.x > 0) // E
            {
                attackAnim.Position = attack_E.Position;
                attackAnim.Play("attack");
                currentAttack = attack_E;
            }
            else // W
            {
                attackAnim.Position = attack_W.Position;
                attackAnim.Play("attack");
                currentAttack = attack_W;
            }
        }
        else
        {
            if (direction.y > 0) // S
            {
                attackAnim.Position = attack_S.Position;
                attackAnim.Play("attack");
                currentAttack = attack_S;
            }
            else // N
            {
                attackAnim.Position = attack_N.Position;
                attackAnim.Play("attack");
                currentAttack = attack_N;
            }
        }
    }

    public bool TakeDamage(Vector2 point, float knockbackForce, int damage)
    {
        if (damageEfftect > 0 || dashCooldown > 0 || health <= 0)
            return false;
        health -= damage;
        knockBack = (GlobalPosition - point).Normalized();
        knockbackSpeed = knockbackForce;

        attackCooldown = 0;
        dashCooldown = 0;
        damageEfftect = invicibleTimer;

        global.playerHealth = health;

        hud.setHealth(health);

        if (health <= 0)
        {
            mc.PlayerDed();
        }
        else
        {
            mc.playPlayerHit();
        }

        return true;
    }

    private void playAttackSound()
    {
        mc.playSwipe();
    }

    public void collectCoin()
    {
        hud.updateCoins(++global.cootsCoins, Global.coinsInGame);
        mc.playCoin();
    }

    public void collectHeart()
    {
        health += 50;
        if (health > 100)
            health = 100;
        global.playerHealth = health;
        hud.setHealth(health);
        mc.playHeal();
    }

    public void breakBox()
    {
        mc.boxBreak();
    }

    public void _on_sludge_enter(Node body)
    {
        if (body.Name == "Player")
            mc.playSludge();
    }

    public void _on_sludge_exit(Node body)
    {
        if (body.Name == "Player")
            mc.stopSludge();
    }

    public void saveUpdates()
    {
        List<Global.RatState> ratList = new List<Global.RatState>();
        List<Global.BoxState> boxList = new List<Global.BoxState>();

        var ratChildren = GetNode("/root/Node2D/rats").GetChildren();
        foreach (var rat in ratChildren)
        {
            if (rat.GetType() == typeof(RatStateMachine))
            {
                RatStateMachine r = (RatStateMachine)rat;
                ratList.Add(new Global.RatState(r.GlobalPosition, r.health, r.Name));
            }
        }

        var boxesChildren = GetNode("/root/Node2D/boxes").GetChildren();
        foreach (var box in boxesChildren)
        {
            if (box.GetType() == typeof(Box))
            {
                Box b = (Box)box;
                boxList.Add(new Global.BoxState(b.GlobalPosition, b.broken, b.Name));
            }
        }

        switch (global.sceneName)
        {
            case SceneName.Main:
                global.mainRoomRats = ratList;
                global.mainRoomBoxes = boxList;
                break;
            case SceneName.Room1:
                global.room1Rats = ratList;
                global.room1Boxes = boxList;
                break;
            case SceneName.Puzzle:
                global.puzzleRoomRats = ratList;
                global.puzzleRoomBoxes = boxList;
                break;
        }
    }


}
