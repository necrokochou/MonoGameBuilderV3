using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Core;


public class Stage {
    // FIELDS
    // --- STATIC FIELDS ---
    private static Rectangle window;
    private static int tileSize;
    
    // --- INSTANCE FIELDS ---
    private Builder builder;
    
    private Tile[,] tiles;
    private Background background;
    private Character character;


    // CONSTRUCTORS
    public Stage() {
        builder = new(this);
        tiles = new Tile[window.Height / tileSize, window.Width / tileSize];
    }


    // PROPERTIES
    // --- STATIC PROPERTIES ---
    public static int TileSize {
        get => tileSize;
    }
    
    // --- INSTANCE PROPERTIES ---
    public Builder Builder {
        get => builder;
    }
    internal Tile[,] Tiles {
        get => tiles;
    }
    internal int Rows {
        get => tiles.GetLength(0);
    }
    internal int Columns {
        get => tiles.GetLength(1);
    }
    internal Background Background {
        get => background;
        set => background = value;
    }
    internal Character Character {
        get => character;
        set => character = value;
    }


    // METHODS
    // --- MAIN METHODS ---
    public void Update(GameTime gameTime) {
        character.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch) {
        Background.Draw(spriteBatch);

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++) {
                if (tiles[row, col] == null) continue;
                
                tiles[row, col].Draw(spriteBatch);
            }
        }
        
        Character.Draw(spriteBatch);
    }
    
    // --- STATIC METHODS ---
    public static void SetWindow(Rectangle clientBounds) {
        window = new Rectangle(clientBounds.X, clientBounds.Y, clientBounds.Width, clientBounds.Height);
    }
    public static void SetTileSize(int size) {
        tileSize = size;
    }
}
