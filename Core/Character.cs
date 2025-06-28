using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Core;


public class Character {
    // FIELDS
    private TextureFile textureFile;
    private Rectangle display;
    private Rectangle source;
    private Color color;

    private Stage stage;

    private InputManager inputManager;
    private ActionManager actionManager;
    
    private Vector2 actualPosition;
    private Vector2 gamePosition;
    
    private float movementSpeed;
    private float jumpPower;

    private bool isFacingRight;
    private bool isGrounded;
    
    private Animation currentAnimation;
    private Animation idleAnimation;
    private Animation walkAnimation;
    
    private Rectangle collisionBox;
    private Point collisionBoxOffset;

    private static GraphicsDevice graphicsDevice;
    private Texture2D debugPixel;
    

    // CONSTRUCTORS
    private Character(TextureFile textureFile, Rectangle display, Rectangle source, Color color) {
        this.textureFile = textureFile;
        this.display = display;
        this.source = source;
        this.color = color;
        
        isFacingRight = true;

        actionManager = new(this);
        inputManager = new();
        
        debugPixel = new(graphicsDevice, 1, 1);
        
    }


    // PROPERTIES
    // --- STATIC PROPERTIES ---
    internal static int TileSize {
        get => Stage.TileSize;
    }
    
    // --- INSTANCE PROPERTIES ---
    internal TextureFile TextureFile {
        get => textureFile;
    }
    internal int Columns {
        get => textureFile.Columns;
    }
    internal int Rows {
        get => textureFile.Rows;
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
    internal Stage Stage {
        set => stage = value;
    }
    internal Vector2 ActualPosition {
        get => actualPosition;
        set => actualPosition = value;
    }
    internal Vector2 GamePosition {
        get => gamePosition;
    }
    internal float MovementSpeed {
        get => movementSpeed;
    }
    internal float JumpPower {
        get => jumpPower;
    }
    internal bool IsGrounded {
        get => isGrounded;
        set => isGrounded = value;
    }

    // METHODS
    // --- MAIN METHODS ---
    internal void Update(GameTime gameTime) {
        inputManager.Update(Keyboard.GetState());
        actionManager.Update(inputManager, gameTime);
        
        currentAnimation = idleAnimation;
        currentAnimation.Update();
    }
    
    internal void Draw(SpriteBatch spriteBatch) {
        SpriteEffects flip = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        
        spriteBatch.Draw(
            Texture, display,
            currentAnimation.GetFrame(),
            color, 0f, Vector2.Zero,
            flip, 0f
        );
        
        DebugCollisionBox(spriteBatch);
    }
    
    // --- STATIC METHODS ---
    public static Character Create(TextureFile txFile, int selectedRow, int selectedColumn) {
        Rectangle source = txFile.SetSource(selectedRow, selectedColumn);
        
        return new(
            txFile,
            txFile.SetDisplay(source, 1.2f),
            source,
            Color.White
        );
    }

    public static void SetGraphicsDevice(GraphicsDevice graphicsDevice) {
        Character.graphicsDevice = graphicsDevice;
    }
    
    // --- INSTANCE METHODS ---
    public void SetIdleAnimation(Point startFrame, Point endFrame, int interval) {
        idleAnimation = Animation.Create(this, interval, startFrame, endFrame);
    }

    public void SetWalkAnimation(Point startFrame, Point endFrame, int interval) {
        walkAnimation = Animation.Create(this, interval, startFrame, endFrame);
    }
    
    private void SetCollisionBox() {
        int srcWidth = source.Width;
        int srcHeight = source.Height;
        Color[] data = new Color[srcWidth * srcHeight];
        Texture.GetData(0, source, data, 0, data.Length);
    
        int minX = srcWidth;
        int maxX = 0;
        int minY = srcHeight;
        int maxY = 0;
    
        bool foundOpaque = false;
    
        for (int y = 0; y < srcHeight; y++) {
            for (int x = 0; x < srcWidth; x++) {
                Color pixel = data[y * srcWidth + x];
                
                if (pixel.A > 10) {
                    foundOpaque = true;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }
    
        if (foundOpaque) {
            float widthScale = 0.25f;
            float heightScale = 0.8f;
            
            int maxWidth = (int) (display.Width * widthScale);
            int maxHeight = (int) (display.Height * heightScale);
            
            int centerX = display.X + display.Width / 2;
            int bottomY = display.Y + display.Height;
            
            int newX = centerX - maxWidth / 2;
            int newY = bottomY - maxHeight;

            collisionBoxOffset.X = newX - display.X;
            collisionBoxOffset.Y = newY - display.Y;
            
            collisionBox = new Rectangle(newX, newY, maxWidth, maxHeight);
            // collisionBoxEdges = new Rectangle(
            //     collisionBox.X - 1,
            //     collisionBox.Y - 1,
            //     collisionBox.Width + 2,
            //     collisionBox.Height + 2
            // );

            // attackRangeBox = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, collisionBox.Height);;
        }
        else {
            collisionBox = Rectangle.Empty;
            collisionBoxOffset = Point.Zero;
            // collisionBoxEdges = Rectangle.Empty;
            //
            // attackRangeBox = Rectangle.Empty;
        }
    }
    
    public void SetLocation(int row, int column) {
        int posX = column * TileSize;
        int posY = row * TileSize;
        
        posX = posX + TileSize / 2 - display.Width / 2;
        posY = posY + TileSize - display.Height;
        
        actualPosition = new Vector2(posX, posY);
        display.Location = actualPosition.ToPoint();
        
        SetCollisionBox();
    }
    
    // --- DEBUG METHODS ---
    private void DebugCollisionBox(SpriteBatch spritebatch) {
        spritebatch.Draw(debugPixel, collisionBox, Color.Yellow * 0.5f);
    }
}
