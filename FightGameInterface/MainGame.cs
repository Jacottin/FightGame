using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FightGame.Characters;

namespace FightGameInterface {
    public class MainGame : Game {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D[] _tankSprites;
        private Texture2D[] _damagerSprites;
        private Texture2D[] _healerSprites;

        private Texture2D _heartFull;
        private Texture2D _heartEmpty;

        private SpriteFont _font80;

        private RenderTarget2D _scaleUpTarget;
        const int RENDER_TARGET_WIDTH = 420;
        const int RENDER_TARGET_HEIGHT = 236;
        private float _upScaleAmount;

        private GameState _gameState;

        private Character _player;
        
        // ---------- PlayerChoice ----------
        private int _actualChoice;
        
        // ------------ In Game -------------
        private Character _npc;

        private void ResetData() {
            _gameState = GameState.PlayerChoice;
            _player = new Healer("Default Player");
            _actualChoice = 1;
            switch (new Random().Next(1, 4)) {
                case 1:
                    _npc = new Damager("NPC");
                    break;
                case 2:
                    _npc = new Healer("NPC");
                    break;
                case 3:
                    _npc = new Tank("NPC");
                    break;
                default:
                    _npc = new Healer("DEFAULT NPC");
                    break;
            }
        }

        public MainGame() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ResetData();
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // create 1x1 texture for line drawing
            Utils.Draw.t = new Texture2D(GraphicsDevice, 1, 1);
            Utils.Draw.t.SetData(
                new [] { Color.White }); // fill the texture with white

            _tankSprites = new[] {
                Content.Load<Texture2D>("assets/tank1"),
                Content.Load<Texture2D>("assets/tank2"),
                Content.Load<Texture2D>("assets/tank3")
            };
            _damagerSprites = new[] {
                Content.Load<Texture2D>("assets/damager1"),
                Content.Load<Texture2D>("assets/damager2"),
                Content.Load<Texture2D>("assets/damager3")
            };
            _healerSprites = new[] {
                Content.Load<Texture2D>("assets/healer1"),
                Content.Load<Texture2D>("assets/healer2"),
                Content.Load<Texture2D>("assets/healer3")
            };

            _heartFull = Content.Load<Texture2D>("assets/heart1");
            _heartEmpty = Content.Load<Texture2D>("assets/heart2");
            
            _scaleUpTarget  = new RenderTarget2D(GraphicsDevice, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            _upScaleAmount = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / RENDER_TARGET_WIDTH;
            
            _font80 = Content.Load<SpriteFont>("Font80");


        }

        protected override void Update(GameTime gameTime) {
            var state = Utils.Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                state.IsKeyDown(Keys.Escape))
                Exit();

            switch (_gameState) {
                case GameState.PlayerChoice:
                    if (Utils.Keyboard.IsKeyDown(Keys.Right))
                        _actualChoice++;
                    if (Utils.Keyboard.IsKeyDown(Keys.Left))
                        _actualChoice--;
                    if (_actualChoice > 3)
                        _actualChoice = 1;
                    if (_actualChoice < 1)
                        _actualChoice = 3;
                    if (state.IsKeyDown(Keys.Enter)) {
                        switch (_actualChoice) {
                            case 1:
                                _player = new Healer("Player");
                                break;                
                            case 2:                   
                                _player = new Damager("Player");
                                break;
                            case 3:
                                _player = new Tank("Player");
                                break;
                        }
                        _gameState++;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // TODO: Add your drawing code here
            
            GraphicsDevice.SetRenderTarget(_scaleUpTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            
            // --------------------- DRAW HERE ---------------------

            switch (_gameState) {
                case GameState.PlayerChoice:
                    DrawChoice(gameTime);
                    DrawInterface(Color.White);
                    break;
                
                case GameState.InGame:
                    DrawInGame(gameTime);
                    DrawLife();
                    DrawInterface(Color.White);
                    break;
            }

            // ------------------- STOP DRAWING --------------------
            
            _spriteBatch.End();


            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(
                SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                SamplerState.PointClamp );
            _spriteBatch.Draw(_scaleUpTarget, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, _upScaleAmount, SpriteEffects.None, 0.0f);
            _spriteBatch.End();

            DrawTexts();

            base.Draw(gameTime);
            
        }         

        private void DrawInterface(Color color) {
            const int lineWidth = 3;
            const int offset = 5;
            
            Vector2 topLeft = new (offset, offset);
            Vector2 topRight = new( RENDER_TARGET_WIDTH - offset, offset);
            Vector2 bottomLeft = new (offset, RENDER_TARGET_HEIGHT - offset);
            Vector2 bottomRight = new (RENDER_TARGET_WIDTH - offset, RENDER_TARGET_HEIGHT - offset);

            Utils.Draw.DrawLine(_spriteBatch, topLeft, topRight, color, lineWidth);
            Utils.Draw.DrawLine(_spriteBatch, topRight, bottomRight, color, lineWidth);
            Utils.Draw.DrawLine(_spriteBatch, bottomRight, bottomLeft, color, lineWidth);
            Utils.Draw.DrawLine(_spriteBatch, bottomLeft, topLeft, color, lineWidth);
            
            Utils.Draw.DrawLine(_spriteBatch, 
                new Vector2(bottomLeft.X, bottomLeft.Y - (bottomLeft - topLeft).Y * 0.3f),
                new Vector2(bottomRight.X, bottomRight.Y - (bottomRight - topRight).Y * 0.3f),
                color, lineWidth
                );
        }
        private void DrawChoice(GameTime gameTime) {
            int index = (int)gameTime.TotalGameTime.TotalMilliseconds / 200 % 3;
            
            Texture2D actSprite = _healerSprites[_actualChoice == 1 ? index : 0];
            
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 4 - actSprite.Width / 2, 60), Color.White);

            actSprite = _damagerSprites[_actualChoice == 2 ? index : 0];
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 4 * 2 - actSprite.Width / 2, 60), Color.White);
            
            actSprite = _tankSprites[_actualChoice == 3 ? index : 0];
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 4 * 3 - actSprite.Width / 2, 60), Color.White);
            
            Utils.Draw.DrawAroundSprite(_spriteBatch, actSprite, new Vector2(RENDER_TARGET_WIDTH / 4 * _actualChoice - actSprite.Width / 2, 60));
        }
        
        private void DrawInGame(GameTime gameTime) {
            int index = (int)gameTime.TotalGameTime.TotalMilliseconds / 200 % 3;
            Texture2D playerSprite = _healerSprites[0];
            switch (_player.getClassName()) {
                case "Healer":
                    playerSprite = _healerSprites[index];
                    break;
                case "Damager":
                    playerSprite = _damagerSprites[index];
                    break;
                case "Tank":
                    playerSprite = _tankSprites[index];
                    break;
            }
            
            _spriteBatch.Draw(playerSprite, new Vector2(RENDER_TARGET_WIDTH / 5 - playerSprite.Width / 2, 80), Color.White);
            
            Texture2D npcSprite = _healerSprites[0];
            switch (_npc.getClassName()) {
                case "Healer":
                    npcSprite = _healerSprites[index];
                    break;
                case "Damager":
                    npcSprite = _damagerSprites[index];
                    break;
                case "Tank":
                    npcSprite = _tankSprites[index];
                    break;
            }
            _spriteBatch.Draw(npcSprite, new Rectangle(RENDER_TARGET_WIDTH / 5 * 4 - npcSprite.Width / 2, 30, 64, 64), 
                null, Color.White, 0f,Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            
        }

        private void DrawTexts() {
            string output;
            Vector2 fontOrigin;
            _spriteBatch.Begin();
            switch (_gameState) {
                // ------------- PLAYER CHOICE --------------
                case GameState.PlayerChoice:
                    output = "Healer";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 4 * 1,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);
                    output = "Damager";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 4 * 2,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);
                    output = "Tank";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 4 * 3,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);

                    output = "Choose a Character";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 2 * 1,
                            _graphics.PreferredBackBufferHeight * 0.12f) - fontOrigin, Color.Brown);
                    break;
                
                // ------------------ IN GAME ---------------------
                
                case GameState.InGame:
                    output = _player.getUserName();
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 8 * 3,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);
                    
                    output = _npc.getUserName();
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 10 * 6,
                            _graphics.PreferredBackBufferHeight * 0.1f) - fontOrigin, Color.Black);
                    break;
                    
            }

            _spriteBatch.End();
        }

        private void DrawLife() {
            Vector2 playerPos = new Vector2(RENDER_TARGET_WIDTH * 0.28f, RENDER_TARGET_HEIGHT * 0.48f);
            Vector2 npcPos = new Vector2(RENDER_TARGET_WIDTH * 0.65f, RENDER_TARGET_HEIGHT * 0.14f);

            for (int i = 0; i < _player.getLife(); i++) {
                _spriteBatch.Draw(_heartFull, new Vector2(playerPos.X + i * _heartFull.Width * 0.08f, playerPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }
            
            for (int i = 0; i < _player.getTotalLife() - _player.getLife(); i++) {
                _spriteBatch.Draw(_heartEmpty, new Vector2( _heartFull.Width * 0.08f * _player.getLife() + playerPos.X + i * _heartEmpty.Width * 0.08f, playerPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }
            
            for (int i = 0; i < _npc.getLife(); i++) {
                _spriteBatch.Draw(_heartFull, new Vector2(npcPos.X - i * _heartFull.Width * 0.08f, npcPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }
            
            for (int i = 0; i < _npc.getTotalLife() - _npc.getLife(); i++) {
                _spriteBatch.Draw(_heartEmpty, new Vector2( npcPos.X - _heartFull.Width * 0.08f * _npc.getLife() - i * _heartEmpty.Width * 0.08f, npcPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }
        }
    }
}