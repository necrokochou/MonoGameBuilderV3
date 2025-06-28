using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Core;


public class TextureFile {
    // FIELDS
    private Texture2D texture;
    private int rows;
    private int columns;
    private int width;
    private int height;
    private int frameWidth;
    private int frameHeight;


    // CONSTRUCTORS
    public TextureFile(Texture2D texture, int rows, int columns) {
        this.texture = texture;
        this.rows = rows;
        this.columns = columns;
    }
    

    // PROPERTIES
    // --- STATIC PROPERTIES ---
    private static int TileSize {
        get => Stage.TileSize;
    }
    
    // --- INSTANCE PROPERTIES ---
    internal Texture2D Texture {
        get => texture;
    }
    public int Rows {
        get => rows;
    }
    public int Columns {
        get => columns;
    }
    public int Width {
        get => texture.Width;
    }
    public int Height {
        get => texture.Height;
    }


    // METHODS
    // --- INSTANCE METHODS ---
    internal Rectangle SetDisplay() {
        return new(0, 0, TileSize, TileSize);
    }

    internal Rectangle SetDisplay(Rectangle source, float sizeMultiplier = 1.0f) {
        float scale = (float) source.Width / source.Height;
        int height = (int) (TileSize * 2 * sizeMultiplier);
        int width = (int) (height * scale);
        
        return new(0, 0, width, height);
    }
    
    internal Rectangle SetSource(int row, int column) {
        frameWidth = Width / Columns;
        frameHeight = Height / Rows;
        
        return new(frameWidth * column, frameHeight * row, frameWidth, frameHeight);
        
        // return new(Width / Columns * column, Height / Rows * row, Width / Columns, Height / Rows);
    }
}
