using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPlayground.Games;

/// <summary>
/// this is simple version of monogames base "Game" class
/// so i can easily swap the games inside the main "Game1"
/// without needing to create a whole new monogame "game"
/// </summary>
///
// note this whole class might be dumb af and it might be smarter to just create actual monogame games instead of this custom "fake" game
// but this will allow comments to explain things 
public abstract class MiniGameBase
{
    protected GraphicsDeviceManager Graphics;
    protected ContentManager Content;
    protected GraphicsDevice GraphicsDevice;

    /// <summary>
    /// this is provided by our custom Setup() method but in your project
    /// it will be setup in your LoadContent() method Like MonoGame defaults to.
    /// Again this is because the weird way we are instantiating minigame classes
    /// </summary>
    protected SpriteBatch SpriteBatch;

    protected MainMenuGame MainMenuGame;

    //if you are copying this code for your own project
    // this Setup is not important and you should use your game class constructor instead
    // this is required because the "weird" way we are instantiating the minigames in the MainMenuGame 
    public void Setup(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, MainMenuGame mainMenuGame)
    {
        Graphics = graphics;
        SpriteBatch = spriteBatch;
        MainMenuGame = mainMenuGame;
        Content = MainMenuGame.Content;
        GraphicsDevice = MainMenuGame.GraphicsDevice;
    }

    public virtual void Initialize()
    {
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }
}