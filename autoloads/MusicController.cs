using Godot;
using System;

public class MusicController : Node
{
    private AudioStream menu_music;
    private AudioStream main_music;
    private AudioStream boss_music;

    public AudioStreamPlayer musicPlayer;
    private AudioStreamPlayer swipe1;
    private AudioStreamPlayer swipe2;
    private bool swipeTracker;

    private AudioStreamPlayer hit1;
    private AudioStreamPlayer hit2;
    private bool hitTracker;

    private AudioStreamPlayer deadRat;

    private AudioStreamPlayer playerHit1;
    private AudioStreamPlayer playerHit2;
    private AudioStreamPlayer playerHit3;
    private AudioStreamPlayer playerDed;
    private bool playerHitTracker;

    private AudioStreamPlayer boxbreak1;
    private AudioStreamPlayer boxbreak2;
    private AudioStreamPlayer boxbreak3;
    private int boxBreakTracker;

    private AudioStreamPlayer coin1;
    private AudioStreamPlayer coin2;
    private bool coinTracker;

    private AudioStreamPlayer heal;

    private AudioStreamPlayer sludge;
    private float sludgePos;

    public float mainVolume = -5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        musicPlayer = GetNode<AudioStreamPlayer>("Music");

        menu_music = (AudioStream)ResourceLoader.Load("res://Audio/travcatgamemenu.mp3");
        main_music = (AudioStream)ResourceLoader.Load("res://Audio/travcatgamev2.mp3");
        boss_music = (AudioStream)ResourceLoader.Load("res://Audio/travcatgameboss.mp3");

        swipe1 = GetNode<AudioStreamPlayer>("Swipe1");
        swipe2 = GetNode<AudioStreamPlayer>("Swipe2");

        hit1 = GetNode<AudioStreamPlayer>("Hit1");
        hit2 = GetNode<AudioStreamPlayer>("Hit2");

        playerHit1 = GetNode<AudioStreamPlayer>("PlayerHit1");
        playerHit2 = GetNode<AudioStreamPlayer>("PlayerHit2");
        playerHit3 = GetNode<AudioStreamPlayer>("PlayerHit3");
        playerDed = GetNode<AudioStreamPlayer>("PlayerDie");

        deadRat = GetNode<AudioStreamPlayer>("DeadRat");
        sludge = GetNode<AudioStreamPlayer>("SludgeSounds");
        sludgePos = 0;

        boxbreak1 = GetNode<AudioStreamPlayer>("boxbreak1");
        boxbreak2 = GetNode<AudioStreamPlayer>("boxbreak2");
        boxbreak3 = GetNode<AudioStreamPlayer>("boxbreak3");

        coin1 = GetNode<AudioStreamPlayer>("coin1");
        coin2 = GetNode<AudioStreamPlayer>("coin2");

        heal = GetNode<AudioStreamPlayer>("heal");

    }

    public void updateVolume(float value)
    {
        mainVolume = value;
        if (mainVolume <= -50)
            mainVolume = -80;
        musicPlayer.VolumeDb = mainVolume;
        sludge.VolumeDb = getSludgeLevel();
    }

    public void play_menu_music()
    {
        musicPlayer.Stop();
        musicPlayer.Stream = menu_music;
        musicPlayer.VolumeDb = mainVolume;
        musicPlayer.Play();
    }

    public void play_main_music()
    {
        musicPlayer.Stop();
        musicPlayer.Stream = main_music;
        musicPlayer.VolumeDb = mainVolume;
        musicPlayer.Play();
    }

    public void play_boss_music()
    {
        musicPlayer.Stop();
        musicPlayer.Stream = boss_music;
        musicPlayer.VolumeDb = mainVolume;
        musicPlayer.Play();
    }

    public void SetMainVolume(float volume)
    {
        musicPlayer.VolumeDb = mainVolume;
    }

    public void playSwipe()
    {
        if (swipeTracker)
        {
            swipe1.VolumeDb = mainVolume;
            swipe1.Play();
        }
        else
        {
            swipe2.VolumeDb = mainVolume;
            swipe2.Play();
        }
        swipeTracker = !swipeTracker;
    }

    public void playHit()
    {
        if (hitTracker)
        {
            hit1.VolumeDb = mainVolume;
            hit1.Play();
        }
        else
        {
            hit2.VolumeDb = mainVolume;
            hit2.Play();
        }
        hitTracker = !hitTracker;
    }

    public void playEnemyDeath()
    {
        deadRat.VolumeDb = mainVolume;
        deadRat.Play();
    }

    public void playPlayerHit()
    {
        if (playerHitTracker)
        {
            playerHit1.VolumeDb = mainVolume;
            playerHit1.Play();
        }
        else
        {
            playerHit2.VolumeDb = mainVolume;
            playerHit2.Play();
        }
        playerHitTracker = !playerHitTracker;
    }

    public void PlayerDed()
    {
        playerDed.VolumeDb = mainVolume;
        playerDed.Play();
    }

    public void playSludge()
    {
        sludge.VolumeDb = getSludgeLevel();
        sludge.Play();
        sludge.Seek(sludgePos);
    }

    public void stopSludge()
    {
        sludgePos = sludge.GetPlaybackPosition();
        sludge.Stop();
    }

    private float getSludgeLevel()
    {
        return mainVolume - 15;
    }

    public void boxBreak()
    {
        if (boxBreakTracker == 1)
        {
            boxbreak1.VolumeDb = mainVolume;
            boxbreak1.Play();
        }
        else if (boxBreakTracker == 2)
        {
            boxbreak2.VolumeDb = mainVolume;
            boxbreak2.Play();
        }
        else
        {
            boxbreak3.VolumeDb = mainVolume;
            boxbreak3.Play();
            boxBreakTracker = 0;
        }
        ++boxBreakTracker;
    }

    public void playCoin()
    {
        if (coinTracker)
        {
            coin1.VolumeDb = mainVolume;
            coin1.Play();
        }
        else
        {
            coin2.VolumeDb = mainVolume;
            coin2.Play();
        }
        coinTracker = !coinTracker;
    }

    public void playHeal()
    {
        heal.VolumeDb = mainVolume;
        heal.Play();
    }
}
