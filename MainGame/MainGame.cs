using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MainGame;


public class MainGame : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Stage stage1;

    public MainGame() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        _graphics.PreferredBackBufferWidth = 1000;
        _graphics.PreferredBackBufferHeight = 650;
    }

    protected override void Initialize() {
        // --- INITIALIZATION ---
        Stage.SetWindow(Window.ClientBounds);
        Stage.SetTileSize(25);
        Character.SetGraphicsDevice(GraphicsDevice);
        // 1000 / 25 = 40 columns
        // 650 / 25 = 26 rows
        
        stage1 = new Stage();
        

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // --- LOAD TEXTURE FILES ---
        TextureFile background7Data = new TextureFile(Content.Load<Texture2D>("GameBackground7"), 1, 1);
        TextureFile frostRocksData = new TextureFile(Content.Load<Texture2D>("FrostRocks"), 1, 6);
        TextureFile rockBlocksData = new TextureFile(Content.Load<Texture2D>("RockBlocks"), 1, 4);
        TextureFile plantsData = new TextureFile(Content.Load<Texture2D>("Plants"), 1, 4);
        TextureFile frostLeavesData = new TextureFile(Content.Load<Texture2D>("FrostLeaves"), 1, 4);
        TextureFile signsData = new TextureFile(Content.Load<Texture2D>("Signs"), 1, 12);
        TextureFile stones1Data = new TextureFile(Content.Load<Texture2D>("Stones1"), 1, 4);
        TextureFile stones2Data = new TextureFile(Content.Load<Texture2D>("Stones2"), 1, 4);
        
        TextureFile heroKnightData = new TextureFile(Content.Load<Texture2D>("HeroKnight"), 9, 10);
        
        // --- LOAD ASSETS ---
        Background background = Background.Create(background7Data);
        Tile frostTile1 = Tile.Create(frostRocksData, 0, 0);
        Tile frostTile2 = Tile.Create(frostRocksData, 0, 1);
        Tile frostTile3 = Tile.Create(frostRocksData, 0, 2);
        Tile frostTile4 = Tile.Create(frostRocksData, 0, 3);
        Tile frostTile5 = Tile.Create(frostRocksData, 0, 4);
        Tile frostTile6 = Tile.Create(frostRocksData, 0, 5);
        Tile rockTile1 = Tile.Create(rockBlocksData, 0, 0);
        Tile rockTile2 = Tile.Create(rockBlocksData, 0, 1);
        Tile rockTile3 = Tile.Create(rockBlocksData, 0, 2);
        Tile rockTile4 = Tile.Create(rockBlocksData, 0, 3);
        Tile plantsTile1 = Tile.Create(plantsData, 0, 0, false);
        Tile plantsTile2 = Tile.Create(plantsData, 0, 1, false);
        Tile plantsTile3 = Tile.Create(plantsData, 0, 2, false);
        Tile plantsTile4 = Tile.Create(plantsData, 0, 3, false);
        Tile frostLeavesTile1 = Tile.Create(frostLeavesData, 0, 0);
        Tile frostLeavesTile2 = Tile.Create(frostLeavesData, 0, 1);
        Tile frostLeavesTile3 = Tile.Create(frostLeavesData, 0, 2);
        Tile frostLeavesTile4 = Tile.Create(frostLeavesData, 0, 3);
        Tile correctWaySign = Tile.Create(signsData, 0, 0, false);
        Tile wrongWaySign = Tile.Create(signsData, 0, 1, false);
        Tile rightSign = Tile.Create(signsData, 0, 2, false);
        Tile bottomRightSign = Tile.Create(signsData, 0, 3, false);
        Tile topRightSign = Tile.Create(signsData, 0, 4, false);
        Tile bottomLeftSign = Tile.Create(signsData, 0, 5, false);
        Tile topLeftSign = Tile.Create(signsData, 0, 6, false);
        Tile leftSign = Tile.Create(signsData, 0, 7, false);
        Tile upSign = Tile.Create(signsData, 0, 8, false);
        Tile downSign = Tile.Create(signsData, 0, 9, false);
        Tile deathSign = Tile.Create(signsData, 0, 10, false);
        Tile questionMarkSign = Tile.Create(signsData, 0, 11, false);
        Tile stonesTile4 = Tile.Create(stones1Data, 0, 3, false);
        Tile stonesTile7 = Tile.Create(stones2Data, 0, 2, false);
        
        Character heroKnight = Character.Create(heroKnightData, 0, 0);
        
        
        // --- STAGE SETUP ---
        stage1.Builder.SetBackground(background);
        
        stage1.Builder.AddTiles(frostTile1, 20, 20, 0, 7);
        stage1.Builder.AddTiles(frostTile1, 20, 20, 11, 13);
        stage1.Builder.AddTiles(frostTile1, 20, 20, 16, 18);
        stage1.Builder.AddTiles(frostTile1, 20, 20, 21, 23);
        stage1.Builder.AddTiles(frostTile1, 20, 20, 26, 28);
        stage1.Builder.AddTiles(frostTile1, 20, 20, 32, 39);
        
        stage1.Builder.AddTiles(frostTile4, 14, 14, 13, 14);
        stage1.Builder.AddTiles(frostTile4, 12, 12, 15, 16);
        stage1.Builder.AddTile(frostTile4, 11, 17);
        stage1.Builder.AddTile(frostTile4, 11, 22);
        stage1.Builder.AddTiles(frostTile4, 12, 12, 23, 24);
        stage1.Builder.AddTiles(frostTile4, 14, 14, 25, 26);

        stage1.Builder.AddTiles(frostTile5, 13, 13, 0, 3);
        stage1.Builder.AddTiles(frostTile5, 17, 17, 18, 21);
        stage1.Builder.AddTiles(frostTile5, 12, 12, 30, 31);
        stage1.Builder.AddTile(frostTile5, 16, 32);
        stage1.Builder.AddTiles(frostTile5, 13, 13, 35, 37);

        stage1.Builder.AddTiles(frostTile6, 15, 15, 7, 8);
        stage1.Builder.AddTiles(frostTile6, 17, 17, 11, 12);
        stage1.Builder.AddTiles(frostTile6, 13, 13, 19, 20);
        stage1.Builder.AddTiles(frostTile6, 17, 17, 27, 28);

        stage1.Builder.AddTiles(rockTile1, 21, 21, 0, 7);
        stage1.Builder.AddTiles(rockTile1, 21, 21, 11, 18);
        stage1.Builder.AddTiles(rockTile1, 20, 20, 14, 15);
        stage1.Builder.AddTiles(rockTile1, 21, 21, 21, 28);
        stage1.Builder.AddTiles(rockTile1, 20, 20, 24, 25);
        stage1.Builder.AddTiles(rockTile1, 21, 21, 32, 39);

        stage1.Builder.AddTiles(rockTile2, 22, 24, 0, 7);
        stage1.Builder.AddTiles(rockTile2, 22, 24, 17, 18);
        stage1.Builder.AddTiles(rockTile2, 22, 24, 21, 22);
        stage1.Builder.AddTiles(rockTile2, 22, 24, 32, 39);

        stage1.Builder.AddTiles(rockTile3, 25, 25, 0, 7);
        stage1.Builder.AddTiles(rockTile3, 25, 25, 11, 28);
        stage1.Builder.AddTiles(rockTile3, 25, 25, 32, 39);

        stage1.Builder.AddTiles(rockTile4, 15, 19, 14, 14);
        stage1.Builder.AddTiles(rockTile4, 13, 19, 15, 15);
        stage1.Builder.AddTiles(rockTile4, 13, 14, 16, 16);
        stage1.Builder.AddTiles(rockTile4, 13, 14, 23, 23);
        stage1.Builder.AddTiles(rockTile4, 13, 19, 24, 24);
        stage1.Builder.AddTiles(rockTile4, 15, 19, 25, 25);
        stage1.Builder.AddTiles(rockTile4, 8, 8, 36, 39);
        stage1.Builder.AddTiles(rockTile4, 5, 7, 39, 39);

        stage1.Builder.AddTiles(frostLeavesTile1, 15, 19, 16, 16);
        stage1.Builder.AddTiles(frostLeavesTile1, 12, 13, 17, 17);
        stage1.Builder.AddTiles(frostLeavesTile1, 12, 13, 22, 22);
        stage1.Builder.AddTiles(frostLeavesTile1, 15, 19, 23, 23);
        stage1.Builder.AddTiles(frostLeavesTile3, 9, 9, 34, 35);
        stage1.Builder.AddTiles(frostLeavesTile3, 4, 4, 35, 39);

        stage1.Builder.AddTile(questionMarkSign, 12, 1);
        stage1.Builder.AddTile(rightSign, 19, 2);
        stage1.Builder.AddTile(deathSign, 19, 7);
        stage1.Builder.AddTile(wrongWaySign, 24, 13);
        stage1.Builder.AddTile(bottomRightSign, 10, 17);
        stage1.Builder.AddTile(downSign, 16, 19);
        stage1.Builder.AddTile(correctWaySign, 24, 20);
        stage1.Builder.AddTile(wrongWaySign, 24, 26);
        stage1.Builder.AddTile(topRightSign, 11, 30);
        stage1.Builder.AddTile(deathSign, 19, 32);
        stage1.Builder.AddTile(correctWaySign, 7, 37);
        stage1.Builder.AddTile(correctWaySign, 18, 37);
        stage1.Builder.AddTile(rightSign, 19, 37);

        stage1.Builder.AddTile(plantsTile4, 19, 3);
        stage1.Builder.AddTile(plantsTile3, 19, 11);
        stage1.Builder.AddTile(plantsTile4, 19, 26);
        stage1.Builder.AddTile(plantsTile3, 19, 34);
        stage1.Builder.AddTile(plantsTile3, 19, 39);

        stage1.Builder.AddTile(stonesTile4, 19, 5);
        stage1.Builder.AddTile(stonesTile7, 19, 21);
        
        stage1.Builder.SetCharacter(heroKnight, 19, 1);
        
        stage1.Builder.Character.SetIdleAnimation(new Point(0, 0), new Point(0, 7), 5);
        stage1.Builder.Character.SetWalkAnimation(new Point(0, 8), new Point(1, 7), 5);
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        stage1.Update(gameTime);
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        stage1.Draw(_spriteBatch);
        
        _spriteBatch.End();
        

        base.Draw(gameTime);
    }
}
