using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2DPlayground.Games;

enum CellState
{
    Empty,
    X,
    O
}

public class TicTacToeGame : MiniGameBase
{
    private Texture2D _gridTexture;
    private Texture2D _xTexture;
    private Texture2D _oTexture;

    private int _moveCount = 0;

    /// <summary>
    /// cell size will be calculated based off the size of the grid texture
    /// </summary>
    private int _gridCellSize = 0;

    private CellState _currentPlayer = CellState.X;

    private CellState[,] _gameBoard = new CellState[,]
    {
        { CellState.Empty, CellState.Empty, CellState.Empty },
        { CellState.Empty, CellState.Empty, CellState.Empty },
        { CellState.Empty, CellState.Empty, CellState.Empty }
    };

    public override void Initialize()
    {
        Graphics.PreferredBackBufferHeight = 100;
        Graphics.PreferredBackBufferWidth = 100;
        Graphics.ApplyChanges();
    }

    public override void LoadContent()
    {
        _gridTexture = Content.Load<Texture2D>("TicTacToeGrid");
        _gridCellSize = _gridTexture.Width / 3;


        _xTexture = Content.Load<Texture2D>("X");
        _oTexture = Content.Load<Texture2D>("O");
    }

    private ButtonState previousClickState;

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        // this assumes a square 1:1 ratio
        var screenWidth = Graphics.PreferredBackBufferWidth;
        var hoveredX = (int)MathF.Floor((float)mouseState.X / _gridCellSize);
        var hoveredY = (int)MathF.Floor((float)mouseState.Y / _gridCellSize);


        // guard clauses to prevent clicking cells "outside" of the 3x3 grid
        if (hoveredX is < 0 or > 2)
        {
            // click position is invalid do nothing
            return;
        }
        if (hoveredY is < 0 or > 2)
        {
            // click position is invalid do nothing
            return;
        }
        //since hoveredX and hoveredY is valid we continue 


        // if left click was just released. only executes for the single update after release / only runs once per click
        if (previousClickState == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
        {
            // get the state of the clicked cell
            var clickedCellState = _gameBoard[hoveredX, hoveredY];

            //if check if the cell is empty 
            if (clickedCellState == CellState.Empty)
            {
                _moveCount++;
                // set the empty clicked cell to the currentPlayer. or set the clicked cell to X or O depending on whos turn it is
                _gameBoard[hoveredX, hoveredY] = _currentPlayer;

                // now that we updated the board we need to check if this last move caused the player to win

                //https://stackoverflow.com/questions/1056316/algorithm-for-determining-tic-tac-toe-game-over

                if (CheckForWin(hoveredY, hoveredX))
                {
                    //CURRENT PLAYER HAS WON
                }
                else if
                    (_moveCount ==
                     9) // if a player did not win and the _moveCount == 9 then there is a tie since there are only 9 places
                {
                    // THERE IS A TIE
                    Console.WriteLine("TIE");
                }

                if (_currentPlayer == CellState.O)
                {
                    _currentPlayer = CellState.X;
                }
                else
                {
                    _currentPlayer = CellState.O;
                }
            }
        }

        previousClickState = mouseState.LeftButton;
    }

    private bool CheckForWin(int hoveredY, int hoveredX)
    {
        // check the horizontal direction of the row we clicked for a win
        // the 3 here is the grid size ex. 3x3 grid
        for (int i = 0; i < 3; i++)
        {
            // check if the current looped cell is not equal to the player
            if (_gameBoard[i, hoveredY] != _currentPlayer)
            {
                // if this is ever true then its impossible for that player to have won at this direction
                break;
            }

            // because the loop starts a 0. 
            if (i == 2)
            {
                Console.WriteLine($"{_currentPlayer} has won horizontal");
                return true;
            }
        }


        for (int i = 0; i < 3; i++)
        {
            // check if the current looped cell is not equal to the player
            if (_gameBoard[hoveredX, i] != _currentPlayer)
            {
                // if this is ever true then its impossible for that player to have won at this direction
                break;
            }

            // because the loop starts a 0. 
            if (i == 2)
            {
                Console.WriteLine($"{_currentPlayer} has won vertical");
                return true;
            }
        }

        //the 2 here is because the grid is a 3x3 and in the array we count 0,1,2. 
        // the only time the row + col cords is equal to 2 is when we have clicked on the horizontal "row" top right to bottom left
        /*
                 * +-----+-----+-----+
                   |     |     | 0,2 |  // 0+2 == 2
                   +-----+-----+-----+
                   |     | 1,1 |     | // 1+1 == 2
                   +-----+-----+-----+
                   | 2,0 |     |     | // 2+0 == 2
                   +-----+-----+-----+
                    As you can see if you add both the X and Y cords of the cells only the top left/bottom right diagonal is equal to 2


                    +-----+-----+-----+
                    | 0,0 | 0,1 | 0,2 |
                    +-----+-----+-----+
                    | 1,0 | 1,1 | 1,2 |
                    +-----+-----+-----+
                    | 2,0 | 2,1 | 2,2 |
                    +-----+-----+-----+
                                         */
        if (hoveredX + hoveredY == 2)
        {
            // if this is true then the player clicked a cell on the anti-diagonal "row" like described above

            for (int i = 0; i < 3; i++)
            {
                if (_gameBoard[i, 2 - i] != _currentPlayer)
                {
                    break;
                }

                if (i == 2)
                {
                    Console.WriteLine($"{_currentPlayer} has won anti-diagonal");
                    return true;
                }
            }
        }


        //now checking normal diagonal

        /* just like the other anti-diagonal we can check know what cell is in the diagonal "row" by checking of the clicked X == Y
        +-----+-----+-----+
        | 0,0 |     |     |
        +-----+-----+-----+
        |     | 1,1 |     |
        +-----+-----+-----+
        |     |     | 2,2 |
        +-----+-----+-----+
        as you can see the only time both the x and y are equal is when we have clicked on the diagonal "row" ex. 0,0 1,1 2,2
        and if we have clicked on the normal diagonal then we want to step over every element in that diagonal "row" and check if
        it its entirely made up of the currentPlayers marks
*/
        if (hoveredX == hoveredY)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_gameBoard[i, i] != _currentPlayer)
                {
                    break;
                }

                if (i == 2)
                {
                    Console.WriteLine($"{_currentPlayer} has won Diagonal");
                    return true;
                }
            }
        }

        return false;
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Red);

        SpriteBatch.Begin(SpriteSortMode.FrontToBack);

        //here we are looping over the _gameBoard and displaying the X's and O'x
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                // the current cellState for our loop
                var cellState = _gameBoard[x, y];

                // calculates the position to place the X's and O'x
                // this works because x and y starts at 0 because of the loop
                // and we are multiplying the _gridCellSize we calculated from the image.
                // The _gridCellSize is the size on a single grid. 
                // for example if the grid image size is 100x100
                // then the gridCellSize is 33x33 combined to make a 3x3 grid 
                var position = new Vector2(x * _gridCellSize, y * _gridCellSize);

                //if cellState is O then display an O with the correct layerDepth parameter "1" to make it render on top of the background
                // this is the same for the X
                if (cellState == CellState.O)
                {
                    SpriteBatch.Draw(_oTexture, position, null, Color.White, 0,
                        Vector2.Zero, Vector2.One,
                        SpriteEffects.None, 1);
                }

                if (cellState == CellState.X)
                {
                    SpriteBatch.Draw(_xTexture, position, null, Color.White, 0, Vector2.Zero, Vector2.One,
                        SpriteEffects.None, 1);
                }
            }
        }


        // draws the grid background with layerDepth 0 so it renders behind the X's and O's
        // this is because SpriteBatch.Begin(SpriteSortMode.FrontToBack); at the top of this method is using SpriteSortMode.FromToBack
        // meaning higher values are rendered ontop of lower values
        SpriteBatch.Draw(_gridTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One,
            SpriteEffects.None, 0);
        SpriteBatch.End();
    }
}