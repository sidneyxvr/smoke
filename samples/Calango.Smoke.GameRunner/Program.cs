using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

var game = new Game();
game.Run();

public class Game : IDisposable
{
    private Calango.Smoke.Engine.Shader _shader = null!;

    private GL _gl = null!;
    private readonly IWindow _window;
    private IInputContext _inputContext = null!;

    private uint _vao = 0;
    private uint _vbo = 0;
    private uint _ebo = 0;

    private uint _shaderProgram = 0;

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

        ReadOnlySpan<float> vertices = [
             // positions       // colors
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // bottom left
             0.0f,  0.5f, 0.0f, 0.0f, 0.0f, 1.0f  // top
        ];
    }

    private void OnRender(double delta)
    {
        //var greenValue = MathF.Sin(Random.Shared.NextSingle()) / 2.0f + 0.5f;
        //var vertexColorLocation = Gl.GetUniformLocation(shaderProgram, "ourColor");

        //Gl.UseProgram(shaderProgram);
        //Gl.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);

        _shader.Use();

        _gl.ClearColor(System.Drawing.Color.DarkGreen);
        _gl.Clear(ClearBufferMask.ColorBufferBit);

        _gl.BindVertexArray(_vao);


        _gl.DrawArrays(PrimitiveType.Triangles, 0, 18);
    }

    private void OnLoad()
    {
        _inputContext = _window.CreateInput();
        for (int i = 0; i < _inputContext.Keyboards.Count; i++)
        {
            _inputContext.Keyboards[i].KeyDown += KeyDown;
        }

        ReadOnlySpan<float> vertices = [
             // positions       // colors
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // bottom left
             0.0f,  0.5f, 0.0f, 0.0f, 0.0f, 1.0f  // top
        ];

        //ReadOnlySpan<uint> indices = [
        //    0, 1, 3,
        //    1, 2, 3
        //];

        _gl = GL.GetApi(_window);

        _shader = new Calango.Smoke.Engine.Shader(_gl);

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StaticDraw);

        //ebo = Gl.GenBuffer();
        //Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, ebo);
        //Gl.BufferData(BufferTargetARB.ElementArrayBuffer, indices, BufferUsageARB.StaticDraw);

        _gl.VertexAttribPointer(
            index: 0,
            size: 3,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: 6 * sizeof(float),
            IntPtr.Zero);

        _gl.EnableVertexAttribArray(0);

        _gl.VertexAttribPointer(
            index: 1,
            size: 3,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: 6 * sizeof(float),
            3 * sizeof(float));

        _gl.EnableVertexAttribArray(1);
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