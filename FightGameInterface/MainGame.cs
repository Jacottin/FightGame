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

        private RenderTarget2D _scaleUpTarget;
        const int RENDER_TARGET_WIDTH = 420;
        const int RENDER_TARGET_HEIGHT = 236;
        private float _upScaleAmount;

        private GameState _gameState = GameState.PlayerChoice;

        private Character _playerType = new Healer("Foobar");
        
        // ---------- PlayerChoice ----------
        private int _actualChoice = 1;

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
            
            _scaleUpTarget  = new RenderTarget2D(GraphicsDevice, RENDER_TARGET_WIDTH, RENDER_TARGET_HEIGHT);
            _upScaleAmount = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / RENDER_TARGET_WIDTH;


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
                                _playerType = new Healer("Foobar");
                                break;
                            case 2:
                                _playerType = new Damager("Foobar");
                                break;
                            case 3:
                                _playerType = new Tank("Foobar");
                                break;
                        }
                        _gameState++;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            GraphicsDevice.SetRenderTarget(_scaleUpTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend
            );
            
            // --------------------- DRAW HERE ---------------------

            switch (_gameState) {
                case GameState.PlayerChoice:
                    DrawChoice(gameTime);
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
    }
}