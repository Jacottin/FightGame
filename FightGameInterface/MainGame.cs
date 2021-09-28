using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FightGame.Characters;

namespace FightGameInterface {
    public class MainGame : Game {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random _random = new Random();

        private Texture2D[] _tankSprites;
        private Texture2D[] _damagerSprites;
        private Texture2D[] _healerSprites;
        private Texture2D[] _analystSprites;

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
        private Song _songSelect;
        
        // ------------ In Game -------------
        private Character _npc;
        private InGameState _inGameState;
        private int _inGameChoice;
        private Song _songPlay;

        private void ResetData() {
            MediaPlayer.Play(_songSelect);
            _gameState = GameState.PlayerChoice;
            _inGameState = InGameState.PlayerAction;
            _player = new Healer("Default Player");
            _actualChoice = 1;
            _inGameChoice = 1;
            switch (_random.Next(1, 5)) {
                case 1:
                    _npc = new Damager("NPC");
                    break;
                case 2:
                    _npc = new Healer("NPC");
                    break;
                case 3:
                    _npc = new Tank("NPC");
                    break;
                case 4:
                    _npc = new Analyst("NPC");
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
            _analystSprites = new[] {
                Content.Load<Texture2D>("assets/analystefinancier1"),
                Content.Load<Texture2D>("assets/analystefinancier2"),
                Content.Load<Texture2D>("assets/analystefinancier3")
            };

            _heartFull = Content.Load<Texture2D>("assets/heart1");
            _heartEmpty = Content.Load<Texture2D>("assets/heart2");
            
            _scaleUpTarget  = new RenderTarget2D(GraphicsDevice, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            _upScaleAmount = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / RENDER_TARGET_WIDTH;
            
            _font80 = Content.Load<SpriteFont>("Font80");
            _songSelect = Content.Load<Song>("assets/character_select");
            _songPlay = Content.Load<Song>("assets/futureGrammyAwards");
            MediaPlayer.IsRepeating = true;
            ResetData();
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
                    if (_actualChoice > 4)
                        _actualChoice = 1;
                    if (_actualChoice < 1)
                        _actualChoice = 4;
                    if (Utils.Keyboard.IsKeyDown(Keys.Enter)) {
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
                            case 4:
                                _player = new Analyst("Player");
                                break;
                        }
                        _gameState++;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(_songPlay);
                    }
                    break;
                case GameState.InGame:
                    switch (_inGameState) {
                        case InGameState.PlayerAction:
                            if (Utils.Keyboard.IsKeyDown(Keys.Left))
                                _inGameChoice--;
                            if (Utils.Keyboard.IsKeyDown(Keys.Right))
                                _inGameChoice++;
                            if (_inGameChoice > 3)
                                _inGameChoice = 1;
                            if (_inGameChoice < 1)
                                _inGameChoice = 3;

                            if (Utils.Keyboard.IsKeyDown(Keys.Enter)) {
                                switch (_inGameChoice) {
                                    case 1:
                                        _player.Attack(_npc);
                                        _player.LastAction = AttackType.Attack;
                                        break;
                                    case 2:
                                        _player.Defend(_npc);
                                        _player.LastAction = AttackType.Defend;
                                        break;
                                    case 3:
                                        _player.SpecialCapacity();
                                        _player.LastAction = AttackType.Special;
                                        break;
                                }
                                _inGameState = InGameState.ExecuteAction;

                                switch (_random.Next(1, 4)) {
                                    case 1:
                                        _npc.Attack(_player);
                                        _npc.LastAction = AttackType.Attack;
                                        break;
                                    case 2:
                                        _npc.Defend(_player);
                                        _npc.LastAction = AttackType.Defend;
                                        break;
                                    case 3:
                                        _npc.SpecialCapacity();
                                        _npc.LastAction = AttackType.Special;
                                        break;
                                }
                                _player.Update(_npc);
                                _npc.Update(_player);
                                _player.ComputeDamages();
                                _npc.ComputeDamages();
                                
                                if (_player.getLife() > 0 && _npc.getLife() > 0) {
                                    _inGameState = InGameState.PlayerAction;
                                }
                                else if (_player.getLife() > _npc.getLife()) {
                                    _gameState = GameState.Win;
                                }
                                else if (_player.getLife() == _npc.getLife()) {
                                    _gameState = GameState.Draw;
                                }
                                else {
                                    _gameState = GameState.Defeat;
                                }
                                MediaPlayer.Stop();
                            }
                            break;
                        case InGameState.ExecuteAction:
                            break;
                    }
                    break;
                
                case GameState.Defeat:
                case GameState.Win:
                case GameState.Draw:
                    if (Utils.Keyboard.IsKeyDown(Keys.Enter))
                        ResetData();
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
                
                case GameState.Defeat:
                case GameState.Win:
                case GameState.Draw:
                    DrawEndGame(gameTime);
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
            
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 5 - actSprite.Width / 2, 60), Color.White);

            actSprite = _damagerSprites[_actualChoice == 2 ? index : 0];
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 5 * 2 - actSprite.Width / 2, 60), Color.White);
            
            actSprite = _tankSprites[_actualChoice == 3 ? index : 0];
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 5 * 3 - actSprite.Width / 2, 60), Color.White);
            
            actSprite = _analystSprites[_actualChoice == 4 ? index : 0];
            _spriteBatch.Draw(actSprite, new Vector2(RENDER_TARGET_WIDTH / 5 * 4 - actSprite.Width / 2, 60), Color.White);
            
            Utils.Draw.DrawAroundSprite(_spriteBatch, new Vector2(RENDER_TARGET_WIDTH / 5 * _actualChoice - actSprite.Width / 2, 60), new Vector2(actSprite.Width, actSprite.Height));
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
                case "Analyst":
                    playerSprite = _analystSprites[index];
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
                case "Analyst":
                    npcSprite = _analystSprites[index];
                    break;
            }
            _spriteBatch.Draw(npcSprite, new Rectangle(RENDER_TARGET_WIDTH / 5 * 4 - npcSprite.Width / 2, 30, 64, 64), 
                null, Color.White, 0f,Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            
            if (_inGameState == InGameState.PlayerAction)
                switch (_inGameChoice) {
                    case 1:
                        Utils.Draw.DrawAroundSprite(_spriteBatch, new Vector2(RENDER_TARGET_WIDTH * 0.05f, 
                            RENDER_TARGET_HEIGHT * 0.85f), _font80.MeasureString("1. Attack")/_upScaleAmount, 5);
                        break;
                    case 2:
                        Utils.Draw.DrawAroundSprite(_spriteBatch, new Vector2(RENDER_TARGET_WIDTH * 0.35f, 
                            RENDER_TARGET_HEIGHT * 0.85f), _font80.MeasureString("1. Defend")/_upScaleAmount, 5);
                        break;
                    case 3:
                        Utils.Draw.DrawAroundSprite(_spriteBatch, new Vector2(RENDER_TARGET_WIDTH * 0.65f, 
                            RENDER_TARGET_HEIGHT * 0.85f), _font80.MeasureString("1. Special")/_upScaleAmount, 5);
                        break;
                }
            
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
                        new Vector2(_graphics.PreferredBackBufferWidth / 5 * 1,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);
                    output = "Damager";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 5 * 2,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);
                    output = "Tank";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 5 * 3,
                            _graphics.PreferredBackBufferHeight * 0.62f) - fontOrigin, Color.Black);
                    
                    output = "Analyst";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output,
                        new Vector2(_graphics.PreferredBackBufferWidth / 5 * 4,
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

                    switch (_inGameState) {
                        case InGameState.PlayerAction:
                            _spriteBatch.DrawString(_font80, "Choose an action :", 
                                new Vector2(_graphics.PreferredBackBufferWidth * 0.05f, 
                                    _graphics.PreferredBackBufferHeight * 0.7f), Color.Black);
                            
                            _spriteBatch.DrawString(_font80, "1. Attack", 
                                new Vector2(_graphics.PreferredBackBufferWidth * 0.05f, 
                                    _graphics.PreferredBackBufferHeight * 0.85f), Color.Black);
                            _spriteBatch.DrawString(_font80, "2. Defend", 
                                new Vector2(_graphics.PreferredBackBufferWidth * 0.35f, 
                                    _graphics.PreferredBackBufferHeight * 0.85f), Color.Black);
                            _spriteBatch.DrawString(_font80, "3. Special", 
                                new Vector2(_graphics.PreferredBackBufferWidth * 0.65f, 
                                    _graphics.PreferredBackBufferHeight * 0.85f), Color.Black);
                            break;
                        case InGameState.ExecuteAction:
                            _spriteBatch.DrawString(_font80, _player.getUserName() + " used " + _player.LastAction, 
                                new Vector2(_graphics.PreferredBackBufferWidth * 0.05f,
                                    _graphics.PreferredBackBufferHeight * 0.73f), Color.Black);
                            
                            _spriteBatch.DrawString(_font80, _npc.getUserName() + " used " + _npc.LastAction, 
                                new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                                    _graphics.PreferredBackBufferHeight * 0.85f), Color.Black);
                            break;
                    }
                    
                    break;
                // -------------------- END GAME ------------------------
                case GameState.Win:
                    output = "YOU WIN !";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output, 
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                            _graphics.PreferredBackBufferHeight * 0.25f) - fontOrigin, Color.IndianRed);
                    output = "Continue ?...";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output, 
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                            _graphics.PreferredBackBufferHeight * 0.85f) - fontOrigin, Color.Black);
                    break;
                case GameState.Defeat:
                    output = "YOU LOOSE...";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output, 
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                            _graphics.PreferredBackBufferHeight * 0.25f) - fontOrigin, Color.IndianRed);
                    output = "Continue ?...";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output, 
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                            _graphics.PreferredBackBufferHeight * 0.85f) - fontOrigin, Color.Black);
                    break;
                case GameState.Draw:
                    output = "IT'S A DRAW !";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output, 
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                            _graphics.PreferredBackBufferHeight * 0.25f) - fontOrigin, Color.IndianRed);
                    output = "Continue ?...";
                    fontOrigin = _font80.MeasureString(output) / 2;
                    _spriteBatch.DrawString(_font80, output, 
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.5f,
                            _graphics.PreferredBackBufferHeight * 0.85f) - fontOrigin, Color.Black);
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
            for (int i = 0; i < _player.getTotalLife() - Math.Max(_player.getLife(), 0); i++) {
                _spriteBatch.Draw(_heartEmpty, new Vector2( _heartFull.Width * 0.08f * Math.Max(_player.getLife(), 0) + playerPos.X + i * _heartEmpty.Width * 0.08f, playerPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }

            for (int i = 0; i < _npc.getLife(); i++) {
                _spriteBatch.Draw(_heartFull, new Vector2(npcPos.X - i * _heartFull.Width * 0.08f, npcPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }
            for (int i = 0; i < _npc.getTotalLife() - Math.Max(_npc.getLife(), 0); i++) {
                _spriteBatch.Draw(_heartEmpty, new Vector2( npcPos.X - _heartFull.Width * 0.08f * Math.Max(_npc.getLife(), 0) - i * _heartEmpty.Width * 0.08f, npcPos.Y) , null, Color.White, 0.0f, Vector2.Zero, 0.08f, SpriteEffects.None, 0.0f);
            }
        }

        private void DrawEndGame(GameTime gameTime) {
            int index = (int)gameTime.TotalGameTime.TotalMilliseconds / 200 % 3;
            Texture2D sprite = _healerSprites[0];
            switch (_player.getClassName()) {
                case "Healer":
                    sprite = _healerSprites[index];
                    break;
                case "Damager":
                    sprite = _damagerSprites[index];
                    break;
                case "Tank":
                    sprite = _tankSprites[index];
                    break;
                case "Analyst":
                    sprite = _analystSprites[index];
                    break;
            }
            _spriteBatch.Draw(sprite, new Vector2(RENDER_TARGET_WIDTH / 2 - sprite.Width / 2, 90), Color.White);
        }
    }
}