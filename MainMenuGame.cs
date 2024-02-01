#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using _2DPlayground.Games;
using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2DPlayground;

/// <summary>
/// if you are using this project to learn the basics of monogame or c# in general
/// you can mostly ignore this file and look at the minigame classes
/// this main menu might be hard for beginner to understand but its really not to complex
/// this file wont have much value if you just want to learn how to make one of the minigames
/// </summary>
public class MainMenuGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private ImGuiRenderer _imGuiRenderer;

    private MiniGameBase? _activeMinigame;

    // storing a list of our minigame types so that we can create that minigame when a button is pressed
    // This looks weird because we are avoiding using the `new` keyword for our minigame class because there might 
    // be many minigames that we dont "running" / created before its been selected
    private List<(Type game, string name)> _miniGameSelection = new()
    {
        (typeof(TicTacToeGame), "Tic Tac Toe")
    };

    public MainMenuGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // we are using im gui for buttons. you can mostly ignore this

        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();
        base.Initialize();
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_activeMinigame is not null)
        {
            // call the active minigame update

            _activeMinigame.Update(gameTime);
            return;
        }

        //main menu update
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_activeMinigame is not null)
        {
            // dont render the main menu and call the draw of the minigame

            _activeMinigame.Draw(gameTime);
            return;
        }

        //main menu draw

        GraphicsDevice.Clear(Color.CornflowerBlue);
        _imGuiRenderer.BeforeLayout(gameTime);
        ImGui.BeginChild("games");
        // here we are looping over all of our minigames and creating a new class if its respective button is pressed
        foreach (var game in _miniGameSelection)
        {
            if (ImGui.Button(game.name))
            {
                // because like explained above, are avoiding using the `new` keyword have to do things in a strange way.
                // this line is almost the same as `_activeMinigame = new TicTacToe();`
                _activeMinigame =
                    (MiniGameBase)Activator.CreateInstance(game.game);
                if (_activeMinigame is null)
                {
                    throw new Exception($"Failed to create minigame of type: {game.game}");
                }

                _activeMinigame.Setup(_graphics, _spriteBatch, this);
                _activeMinigame.Initialize();
                _activeMinigame.LoadContent();
            }

            ImGui.SameLine();
        }

        ImGui.EndChild();
        _imGuiRenderer.AfterLayout();
    }


    public void CloseMiniGame()
    {
        // IDisposable?
        _activeMinigame = null;
    }
}