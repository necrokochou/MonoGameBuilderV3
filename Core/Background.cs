using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Core;


public class Background {
    // FIELDS
    private TextureFile textureFile;
    private Rectangle rectangle;
    private Color color;


    // CONSTRUCTORS
    private Background(TextureFile textureFile, Rectangle rectangle, Color color) {
        this.textureFile = textureFile;
        this.rectangle = rectangle;
        this.color = color;
    }


    // PROPERTIES
    private Texture2D Texture {
        get => textureFile.Texture;
    }
    private Rectangle Rectangle {
        get => rectangle;
    }
    private Color Color {
        get => color;
    }


    // METHODS
    // --- MAIN METHODS ---
    internal void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, Rectangle, Color);
    }
    
    // --- STATIC METHODS ---
    public static Background Create(TextureFile txFile) {
        return new(txFile, txFile.SetSource(0, 0), Color.White);
    }
}
