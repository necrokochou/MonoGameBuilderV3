using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Core;


public class Tile {
    // FIELDS
    private TextureFile textureFile;
    private Rectangle display;
    private Rectangle source;
    private Color color;

    private bool isSolid;
    

    // CONSTRUCTORS
    private Tile(TextureFile textureFile, Rectangle display, Rectangle source, Color color, bool isSolid) {
        this.textureFile = textureFile;
        this.display = display;
        this.source = source;
        this.color = color;
        this.isSolid = isSolid;
    }


    // PROPERTIES
    // --- INSTANCE PROPERTIES ---
    internal TextureFile TextureFile {
        get => textureFile;
    }
    internal Texture2D Texture {
        get => textureFile.Texture;
    }
    internal Rectangle Display {
        get => display;
    }
    internal Rectangle Source {
        get => source;
    }
    internal Color Color {
        get => color;
    }
    internal bool IsSolid {
        get => isSolid;
    }
    
    // --- STATIC PROPERTIES ---
    private static int TileSize {
        get => Stage.TileSize;
    }


    // METHODS
    // --- MAIN METHODS ---
    internal void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, Display, Source, Color);
    }
    
    // --- INSTANCE METHODS ---
    internal void SetLocation(int row, int column) {
        display.Location = new Point(TileSize * column, TileSize * row);
    }

    internal void SetProperties(bool solid) {
        isSolid = solid;
    }
    
    // --- STATIC METHODS ---
    public static Tile Create(TextureFile txFile, int selectedRow, int selectedColumn, bool solid = true) {
        if (selectedRow < 0 || selectedRow >= txFile.Rows || selectedColumn < 0 || selectedColumn >= txFile.Columns)
            throw new ArgumentException($"Selected source is out of bounds: ({selectedRow},{selectedColumn})");
        
        return new(txFile, txFile.SetDisplay(), txFile.SetSource(selectedRow, selectedColumn), Color.White, solid);
    }

    public static Tile Copy(Tile tile) {
        return new(tile.TextureFile, tile.Display, tile.Source, tile.Color, tile.IsSolid);
    }
}
