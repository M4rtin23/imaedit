using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;



namespace imaedit;

public class Game1 : Game{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
	private float scaling = 1;
	public static int height = 50, width = 80, size = 5;

	public static float rotationX = 0;
	public static float rotationY = 0;
	public static float rotationZ = 0;

	public static Vector2 position = new Vector2(100,100);

	public static Vector2 startingPosition;
	public static Vector2 endingPosition;
	public static bool isClicking = false;

	public static bool isPainting = false;
	
	private static GameBuilder.Shapes.RectangleF[,] pixels = new GameBuilder.Shapes.RectangleF[width,height];
	private Microsoft.Xna.Framework.Color selectedColor = Microsoft.Xna.Framework.Color.Pink;


    public Game1(){
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1400;
        _graphics.PreferredBackBufferHeight = 800;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize(){
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				pixels[i,j] = new GameBuilder.Shapes.RectangleF(size*i,size*j,size,size);
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
			/*	for(int i = 0; i < width; i++){
					for(int j = 0; j < height; j++){
						if(pixels[i,j].Contains(Mouse.GetState().Position.ToVector2()/scaling-position/scaling)){
							pixels[i,j].SetColor(selectedColor);
						}
					}
				}
				*/
				try{
					pixels[(int)((Mouse.GetState().Position.X-position.X)/size/scaling),(int)((Mouse.GetState().Position.Y-position.Y)/size/scaling)].SetColor(selectedColor);
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
			scaling -= .05f;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.Add)){
			scaling += .05f;
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
        if (Keyboard.GetState().IsKeyDown(Keys.F1)){
			isPainting = true;
		}
        if (Keyboard.GetState().IsKeyDown(Keys.F2)){
			isPainting = false;
		}


        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);


		Texture2D texture = new Texture2D(GraphicsDevice, width, height);

		Microsoft.Xna.Framework.Color[] colorList = new Microsoft.Xna.Framework.Color[height*width];

		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				colorList[i*width+j] = pixels[j,i].GetColor();
			}

		}

		
		texture.SetData(colorList);

/*		_spriteBatch.Begin(transformMatrix: Matrix.CreateScale(scaling)*Matrix.CreateRotationZ(rotationZ)*Matrix.CreateRotationY(rotationY)*Matrix.CreateRotationX(rotationX)*Matrix.CreateTranslation(position.X, position.Y, 0), samplerState:  SamplerState.PointClamp);


		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
				pixels[i,j].Draw(_spriteBatch);
			}
		}
		_spriteBatch.End();*/
		_spriteBatch.Begin(transformMatrix: Matrix.CreateScale(scaling*size)*Matrix.CreateRotationZ(rotationZ)*Matrix.CreateRotationY(rotationY)*Matrix.CreateRotationX(rotationX)*Matrix.CreateTranslation(position.X, position.Y, 0), samplerState:  SamplerState.PointClamp);

		_spriteBatch.Draw(texture, Vector2.Zero, Microsoft.Xna.Framework.Color.White);

//		new GameBuilder.Shapes.RectangleF(Mouse.GetState().Position.ToVector2(), new Vector2(20,20)).Draw(_spriteBatch);
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
				bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pixels[x,y].GetColor().A, pixels[x,y].GetColor().R, pixels[x,y].GetColor().G, pixels[x,y].GetColor().B));
			}
		}
		bitmap.Save("/home/martin/image.png");		
	}
}


