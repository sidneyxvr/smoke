using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

var vextexShaderSource = """
    #version 330 core
    layout (location = 0) in vec3 aPos;
    
    void main()
    {
        gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
    }
    """;

var fragmentShaderSource = """
    #version 330 core
    out vec4 FragColor;
    
    void main()
    {
        FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
    }
    """;

GL gl = null!;
uint vao = 0;
uint vbo = 0;

uint shaderProgram = 0;

using var window = Window.Create(WindowOptions.Default with
{
    Size = new Vector2D<int>(800, 600),
    Title = "LearnOpenGL"
});

window.Load += () =>
{
    var input = window.CreateInput();
    for (int i = 0; i < input.Keyboards.Count; i++)
    {
        input.Keyboards[i].KeyDown += KeyDown;
    }

    gl = GL.GetApi(window);


    ReadOnlySpan<float> buffer = [
        -0.5f,-0.5f, 0.0f,
        0.5f,-0.5f, 0.0f,
        0.0f, 0.5f, 0.0f
    ];

    vao = gl.GenVertexArray();
    gl.BindVertexArray(vao);

    vbo = gl.GenBuffer();
    gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
    gl.BufferData(BufferTargetARB.ArrayBuffer, buffer, BufferUsageARB.StaticDraw);

    var vexterShader = gl.CreateShader(ShaderType.VertexShader);
    gl.ShaderSource(vexterShader, vextexShaderSource);
    gl.CompileShader(vexterShader);

    var fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
    gl.ShaderSource(fragmentShader, fragmentShaderSource);
    gl.CompileShader(fragmentShader);

    shaderProgram = gl.CreateProgram();
    gl.AttachShader(shaderProgram, vexterShader);
    gl.AttachShader(shaderProgram, fragmentShader);
    gl.LinkProgram(shaderProgram);

    gl.DeleteShader(vexterShader);
    gl.DeleteShader(fragmentShader);

    gl.VertexAttribPointer(
        index: 0,
        size: 3,
        VertexAttribPointerType.Float,
        normalized: false,
        3 * sizeof(float),
        IntPtr.Zero);

    gl.EnableVertexAttribArray(0);
};

window.FramebufferResize += FrameBufferResize;

window.Render += delta =>
{
    gl.ClearColor(System.Drawing.Color.DarkGreen);
    gl.Clear(ClearBufferMask.ColorBufferBit);

    gl.UseProgram(shaderProgram);
    gl.BindVertexArray(vao);

    gl.DrawArrays(PrimitiveType.Triangles, first: 0, count: 3);
};

window.Run();

void FrameBufferResize(Vector2D<int> size)
{
    gl.Viewport(size);
}

void KeyDown(IKeyboard arg1, Key arg2, int _)
{
    if (arg2 == Key.Escape)
    {
        window.Close();
    }
}