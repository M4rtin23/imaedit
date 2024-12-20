using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;


namespace imaedit;

public class Game1 : Game{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
	private float scaling = 5;
	public static int height = 50, width = 80;

	public static float rotationX = 0;
	public static float rotationY = 0;
	public static float rotationZ = 0;

	public static Vector2 position;

	public static Vector2 startingPosition;
	public static Vector2 endingPosition;
	public static bool isClicking = false;

	public static bool isPainting = false;
	
	private Microsoft.Xna.Framework.Color selectedColor = Microsoft.Xna.Framework.Color.Pink;

	public static Microsoft.Xna.Framework.Color[] colorList = new Microsoft.Xna.Framework.Color[height*width];
	int swv;

    public Game1(){
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1400;
        _graphics.PreferredBackBufferHeight = 800;
        position = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)/2;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize(){
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				colorList[i*width+j] = Microsoft.Xna.Framework.Color.White;
			}

		}

        base.Initialize();
    }

    protected override void LoadContent(){
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime){
		if(Mouse.GetState().LeftButton == ButtonState.Pressed){
			if(isPainting){
				try{
					int i = (int)((Mouse.GetState().Position.X-position.X)/scaling+width/2);
					int j = (int)((Mouse.GetState().Position.Y-position.Y)/scaling+height/2);
					colorList[j*width+i] = selectedColor;

				}catch{}



			}else{
				if(!isClicking){
					startingPosition = Mouse.GetState().Position.ToVector2() - position;
				}
				isClicking = true;
				if(isClicking){
					endingPosition = Mouse.GetState().Position.ToVector2();
				}
				position = endingPosition - startingPosition;
			}
		}
		if(Mouse.GetState().LeftButton != ButtonState.Pressed){
			isClicking = false;
		}



        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
            Exit();
            
		}

        if (Keyboard.GetState().IsKeyDown(Keys.Enter)){
			CreateImage();
		}

        if (Keyboard.GetState().IsKeyDown(Keys.D1)){
			selectedColor = Microsoft.Xna.Framework.Color.Red;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D2)){
			selectedColor = Microsoft.Xna.Framework.Color.Green;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D3)){
			selectedColor = Microsoft.Xna.Framework.Color.Blue;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D4)){
			selectedColor = Microsoft.Xna.Framework.Color.Pink;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D5)){
			selectedColor = Microsoft.Xna.Framework.Color.Brown;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D9)){
			selectedColor = Microsoft.Xna.Framework.Color.Black;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D0)){
			selectedColor = Microsoft.Xna.Framework.Color.White;
		}

        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus)){
			scaling /= 1.05f;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.Add)){
			scaling *= 1.05f;
		}
        /*if (Keyboard.GetState().IsKeyDown(Keys.Left)){
			rotationZ -= .05f;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.Right)){
			rotationZ +=.05f;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.A)){
			rotationY -= .0005f;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.D)){
			rotationY +=.0005f;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.W)){
			rotationX -= .0005f;
		}
		*/
        if (Keyboard.GetState().IsKeyDown(Keys.S)){
			rotationX +=.0005f;
		}
		isPainting = !Keyboard.GetState().IsKeyDown(Keys.LeftControl);


		if(!isPainting){
			int diff = swv - Mouse.GetState().ScrollWheelValue;
			if(swv - Mouse.GetState().ScrollWheelValue > 0){
				scaling *= (1 -	 diff/20f/120f);
			}
			if(swv - Mouse.GetState().ScrollWheelValue < 0){
				scaling *= (1 - diff/20f/120f);
			}
		}
		swv = Mouse.GetState().ScrollWheelValue;
		base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);


		Texture2D texture = new Texture2D(GraphicsDevice, width, height);
		texture.SetData(colorList);

		_spriteBatch.Begin(transformMatrix: Matrix.CreateScale(scaling)*Matrix.CreateRotationZ(rotationZ)*Matrix.CreateRotationY(rotationY)*Matrix.CreateRotationX(rotationX)*Matrix.CreateTranslation(position.X, position.Y, 0), samplerState:  SamplerState.PointClamp);
		_spriteBatch.Draw(texture, -new Vector2(width, height)/2, Microsoft.Xna.Framework.Color.White);
		_spriteBatch.End();

		_spriteBatch.Begin();
	/*	int size = 30;
		new GameBuilder.Shapes.RectangleF(0, _graphics.PreferredBackBufferHeight/2-size*2, size, size, Microsoft.Xna.Framework.Color.Red).Draw(_spriteBatch);
		new GameBuilder.Shapes.RectangleF(size, _graphics.PreferredBackBufferHeight/2-size*2, size, size, Microsoft.Xna.Framework.Color.Green).Draw(_spriteBatch);
		new GameBuilder.Shapes.RectangleF(0, _graphics.PreferredBackBufferHeight/2-size, size, size, Microsoft.Xna.Framework.Color.Blue).Draw(_spriteBatch);
		new GameBuilder.Shapes.RectangleF(size, _graphics.PreferredBackBufferHeight/2-size, size, size, Microsoft.Xna.Framework.Color.Pink).Draw(_spriteBatch);
*/
		_spriteBatch.End();
        base.Draw(gameTime);
    }
   	public static void CreateImage(){
		Bitmap bitmap = new Bitmap(width, height);
		for (var x = 0; x < bitmap.Width; x++){
			for (var y = 0; y < bitmap.Height; y++){
				bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(colorList[y*width+x].A, colorList[y*width+x].R, colorList[y*width+x].G, colorList[y*width+x].B));
				}
		}
		bitmap.Save("/home/martin/image.png");		
	}
}


