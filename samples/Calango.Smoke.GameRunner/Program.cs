using Calango.Smoke.Shapes;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

var game = new Game();
game.Run();

public class Game : IDisposable
{
    private Calango.Smoke.Shader _shader = null!;

    private GL _gl = null!;
    private readonly IWindow _window;
    private IInputContext _inputContext = null!;

    private readonly List<Shape> _shapes = [];

    public Game()
    {
        _window = Window.Create(WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "LearnOpenGL"
        });
    }

    public void Run()
    {
        _window.Load += OnLoad;

        _window.FramebufferResize += FrameBufferResize;

        _window.Render += OnRender;

        _window.Run();
    }

    private void OnRender(double delta)
    {
        _shader.Use();

        _gl.ClearColor(System.Drawing.Color.DarkGreen);
        _gl.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var shape in _shapes)
        {
            shape.Render();
        }
    }

    private void OnLoad()
    {
        _inputContext = _window.CreateInput();
        for (int i = 0; i < _inputContext.Keyboards.Count; i++)
        {
            _inputContext.Keyboards[i].KeyDown += KeyDown;
        }

        _gl = GL.GetApi(_window);

        _shader = new(_gl);

        _shapes.Add(new Triangle(_gl, [
            new(-0.8f, -0.8f, 0.0f),
            new(0.8f, -0.8f, 0.0f),
            new(0.0f, 0.8f, 0.0f)
        ]));

        using var image = Image.Load<Rgba32>(@"C:\Users\sidne\Source\Repos\smoke\samples\Calango.Smoke.GameRunner\Assets\Textures\texture-sample.jpg");

        _shapes.Add(new Rect(_gl, [
            new(0.5f,  0.5f, 0.0f),
            new(0.5f, -0.5f, 0.0f),
            new(-0.5f, -0.5f, 0.0f),
            new(-0.5f,  0.5f, 0.0f)
        ], image));
    }

    private void FrameBufferResize(Vector2D<int> size)
    {
        _gl.Viewport(size);
    }

    private void KeyDown(IKeyboard arg1, Key arg2, int _)
    {
        if (arg2 == Key.Escape)
        {
            _window.Close();
        }
    }

    public void Dispose()
    {
        _window.Dispose();
    }
}