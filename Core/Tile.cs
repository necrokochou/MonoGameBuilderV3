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
    private bool isCollectable;
    private bool isDamaging;
    

    // CONSTRUCTORS
    private Tile(TextureFile textureFile, Rectangle display, Rectangle source, Color color, bool isSolid, bool isCollectable, bool isDamaging) {
        this.textureFile = textureFile;
        this.display = display;
        this.source = source;
        this.color = color;
        this.isSolid = isSolid;
        this.isCollectable = isCollectable;
        this.isDamaging = isDamaging;
    }


    // PROPERTIES
    // --- STATIC PROPERTIES ---
    private static int TileSize {
        get => Stage.TileSize;
    }
    
    // --- INSTANCE PROPERTIES ---
    internal Texture2D Texture {
        get => textureFile.Texture;
    }
    internal Rectangle Display {
        get => display;
    }
    internal bool IsSolid {
        get => isSolid;
    }
    internal bool IsCollectable {
        get => isCollectable;
    }
    internal bool IsDamaging {
        get => isDamaging;
    }
    

    // METHODS
    // --- MAIN METHODS ---
    internal void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, display, source, color);
    }
    
    // --- INSTANCE METHODS ---
    internal void SetLocation(int row, int column) {
        display.Location = new Point(TileSize * column, TileSize * row);
    }

    internal void SetProperties(bool solid, bool collectable) {
        isSolid = solid;
        isCollectable = collectable;
    }
    
    // --- STATIC METHODS ---
    public static Tile Create(TextureFile txFile, int selectedRow, int selectedColumn, bool solid = true, bool collectable = false, bool damaging = false) {
        if (selectedRow < 0 || selectedRow >= txFile.Rows || selectedColumn < 0 || selectedColumn >= txFile.Columns)
            throw new ArgumentException($"Selected source is out of bounds: ({selectedRow},{selectedColumn})");
        
        return new(txFile, txFile.SetDisplay(), txFile.SetSource(selectedRow, selectedColumn), Color.White, solid, collectable, damaging);
    }

    public static Tile Copy(Tile tile) {
        return new(tile.textureFile, tile.display, tile.source, tile.color, tile.isSolid, tile.isCollectable, tile.isDamaging);
    }
}
