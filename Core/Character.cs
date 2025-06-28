using System;
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

    // private InputManager input;
    
    private Vector2 actualPosition;
    private Vector2 gamePosition;
    private Vector2 velocity;
    private Vector2 move;

    private int jumpCounter;
    private int maxJumpCount;
    
    private float movementSpeed;
    private float jumpPower;
    private float gravity;

    private bool isFacingRight;
    private bool isGrounded;
    private bool isWalking;
    private bool isJumping;
    private bool isFalling;
    private bool isGettingHit;
    private bool isAttacking;
    private bool isHoldingJump;
    
    private Animation currentAnimation;
    private Animation idleAnimation;
    private Animation walkAnimation;
    private Animation jumpAnimation;
    private Animation fallAnimation;
    private Animation getHitAnimation;
    private Animation attackAnimation;
    
    private Rectangle collisionBox;
    private Point collisionBoxOffset;

    private KeyboardState keyState;
    private MouseState mouseState;

    private static GraphicsDevice graphicsDevice;
    private Texture2D debugPixel;
    

    // CONSTRUCTORS
    private Character(TextureFile textureFile, Rectangle display, Rectangle source, Color color) {
        this.textureFile = textureFile;
        this.display = display;
        this.source = source;
        this.color = color;
        
        // input = new();
        
        isFacingRight = true;

        movementSpeed = 150f;
        jumpPower = 320f;
        gravity = 700f;
        maxJumpCount = 2;
        
        debugPixel = new(graphicsDevice, 1, 1);
        debugPixel.SetData(new[] {Color.White});
    }


    // PROPERTIES
    // --- STATIC PROPERTIES ---
    internal static int TileSize {
        get => Stage.TileSize;
    }
    
    // --- INSTANCE PROPERTIES ---
    internal int Columns {
        get => textureFile.Columns;
    }
    internal int Rows {
        get => textureFile.Rows;
    }
    internal Texture2D Texture {
        get => textureFile.Texture;
    }
    internal Rectangle Source {
        get => source;
    }
    internal Stage Stage {
        set => stage = value;
    }
    

    // METHODS
    // --- MAIN METHODS ---
    internal void Update(GameTime gameTime) {
        float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        // input.Update(Keyboard.GetState());
        keyState = Keyboard.GetState();
        mouseState = Mouse.GetState();
        move = Vector2.Zero;

        if (keyState.IsKeyDown(Keys.A)) {
            move.X -= 1;
            isFacingRight = false;
        } else if (keyState.IsKeyDown(Keys.D)) {
            move.X += 1;
            isFacingRight = true;
        } else {
            move.X = 0;
        }

        if (keyState.IsKeyDown(Keys.Space) && jumpCounter < maxJumpCount && !isHoldingJump) {
            velocity.Y = -jumpPower;
            isGrounded = false;
            jumpCounter++;
            isHoldingJump = true;
        }
        if (keyState.IsKeyUp(Keys.Space))
            isHoldingJump = false;

        if (mouseState.LeftButton == ButtonState.Pressed) {
            isAttacking = true;
        }
        
        isWalking = velocity.X != 0 && isGrounded;
        isJumping = velocity.Y < 0 && !isGrounded;
        isFalling = velocity.Y > 0 && !isGrounded;

        if (isAttacking) {
            currentAnimation = attackAnimation;
            isAttacking = false;
        }
        else if (isGettingHit) {
            currentAnimation = getHitAnimation;
            isGettingHit = false;
        }
        else if (isFalling) currentAnimation = fallAnimation;
        else if (isJumping) currentAnimation = jumpAnimation;
        else if (isWalking) currentAnimation = walkAnimation;
        else currentAnimation = idleAnimation;
        
        currentAnimation.Update();
        
        velocity.X = move.X * movementSpeed;
        velocity.Y += gravity * deltaTime;
        GetNextPosition(deltaTime);
        
        display.Location = actualPosition.ToPoint();
        gamePosition = GetGamePos();
        UpdateCollisionBox();
        // DebugShowKeyPresses();
    }
    
    internal void Draw(SpriteBatch spriteBatch) {
        SpriteEffects flip = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        
        spriteBatch.Draw(
            Texture, display,
            currentAnimation.GetFrame(),
            color, 0f, Vector2.Zero,
            flip, 0f
        );
        
        // DebugShowDisplay(spriteBatch);
        // DebugCollisionBox(spriteBatch);
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
    private void GetNextPosition(float deltaTime) {
        Vector2 nextPosition = actualPosition;
        
        nextPosition.X += velocity.X * deltaTime;
        // display.Location = new Point((int) nextPosition.X, (int) position.Y);
        Rectangle nextHorizontalBox = new Rectangle(
            (int) nextPosition.X + collisionBoxOffset.X,
            (int) actualPosition.Y + collisionBoxOffset.Y,
            collisionBox.Width,
            collisionBox.Height
        );
        CheckDamageTiles(nextHorizontalBox);
        if (!CheckCollision(nextHorizontalBox)) {
            actualPosition.X = nextPosition.X;
        } else {
            velocity.X = 0;
        }
        
        nextPosition.Y += velocity.Y * deltaTime;
        // display.Location = new Point((int) position.X, (int) nextPosition.Y);
        Rectangle nextVerticalBox = new Rectangle(
            (int) actualPosition.X + collisionBoxOffset.X,
            (int) nextPosition.Y + collisionBoxOffset.Y,
            collisionBox.Width,
            collisionBox.Height
        );
        if (!CheckCollision(nextVerticalBox)) {
            actualPosition.Y = nextPosition.Y;
            isGrounded = false;
        } else {
            if (velocity.Y > 0) {
                isGrounded = true;
                isJumping = false;
                jumpCounter = 0;
            }
            
            velocity.Y = 0;
        }
    }

    private void CheckDamageTiles(Rectangle nextBox) {
        foreach (Tile tile in stage.Tiles) {
            if (tile == null) continue;

            if (nextBox.Intersects(tile.Display) && tile.IsDamaging) {
                isGettingHit = true;
            }
        }
    }
    
    private void CheckCollectableTiles(Rectangle nextBox) {
        for (int row = 0; row < stage.Rows; row++) {
            for (int column = 0; column < stage.Columns; column++) {
                if (stage.Tiles[row, column] == null) continue;
                
                if (nextBox.Intersects(stage.Tiles[row, column].Display) && stage.Tiles[row, column].IsCollectable) {
                    stage.Tiles[row, column] = null;
                }
            }
        }
    }
    
    private bool CheckCollision(Rectangle nextBox) {
        CheckCollectableTiles(nextBox);
        
        foreach (Tile tile in stage.Tiles) {
            if (tile == null) continue;

            if (nextBox.Intersects(tile.Display) && tile.IsSolid) {
                // Console.WriteLine($"Collision with tile at: ({tile.Display.Location})");
                return true;
            }
        }
            
        //Console.WriteLine($"No collision at: ({position})");
        return false;
    }
    
    public void UpdateCollisionBox() {
        SetCollisionBox();
    }
    
    public void SetIdleAnimation(Point startFrame, Point endFrame, int interval) {
        idleAnimation = Animation.Create(this, startFrame, endFrame, interval);
    }

    public void SetWalkAnimation(Point startFrame, Point endFrame, int interval) {
        walkAnimation = Animation.Create(this, startFrame, endFrame, interval);
    }

    public void SetJumpAnimation(Point startFrame, Point endFrame, int interval) {
        jumpAnimation = Animation.Create(this, startFrame, endFrame, interval);
    }

    public void SetFallAnimation(Point startFrame, Point endFrame, int interval) {
        fallAnimation = Animation.Create(this, startFrame, endFrame, interval);
    }

    public void SetGetHitAnimation(Point startFrame, Point endFrame, int interval) {
        getHitAnimation = Animation.Create(this, startFrame, endFrame, interval);
    }
    
    public void SetAttackAnimation(Point startFrame, Point endFrame, int interval) {
        attackAnimation = Animation.Create(this, startFrame, endFrame, interval);
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

            // attackRangeBox = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, collisionBox.Height);;
        }
        else {
            collisionBox = Rectangle.Empty;
            collisionBoxOffset = Point.Zero;
            
            // attackRangeBox = Rectangle.Empty;
        }
    }
    
    public void SetLocation(int row, int column) {
        int posX = column * TileSize;
        int posY = row * TileSize;
        
        posX = posX + TileSize / 2 - display.Width / 2;
        posY = posY + TileSize - display.Height;
        
        display.Location = new Point(posX, posY);
        actualPosition = new Vector2(posX, posY);
        
        SetCollisionBox();
    }
    
    private Vector2 GetGamePos() {
        return new Vector2(collisionBox.X, collisionBox.Y) + new Vector2(collisionBox.Width / 2, collisionBox.Height);
    }
    
    // --- DEBUG METHODS ---
    private void DebugShowDisplay(SpriteBatch spriteBatch) {
        DrawHollowRect(spriteBatch, display, Color.Red, 2);
    }
    
    private void DebugCollisionBox(SpriteBatch spritebatch) {
        // spritebatch.Draw(debugPixel, collisionBox, Color.Yellow * 0.5f);
        DrawHollowRect(spritebatch, collisionBox, Color.Yellow * 0.5f, 2);
    }

    private void DrawHollowRect(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness = 1) {
        // TOP
        spriteBatch.Draw(debugPixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // BOTTOM
        spriteBatch.Draw(debugPixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
        // LEFT
        spriteBatch.Draw(debugPixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // RIGHT
        spriteBatch.Draw(debugPixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }

    private void DebugShowKeyPresses() {
        foreach (Keys key in keyState.GetPressedKeys()) {
            Console.WriteLine($"{key}");
        }
    }
}
