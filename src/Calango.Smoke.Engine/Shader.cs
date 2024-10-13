using Silk.NET.OpenGL;

namespace Calango.Smoke.Engine;

public class Shader
{
    private readonly GL _gl;

    private const string DefaultVertexShaderSource = """
        #version 330 core
        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec3 aColor;

        out vec3 ourColor;

        void main()
        {
            gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
            ourColor = aColor;
        }
        """;

    private const string DefaultFragmentShaderSource = """
        #version 330 core
        out vec4 FragColor;

        in vec3 ourColor;

        void main()
        {
            FragColor = vec4(ourColor, 1.0);
        }
        """;

    public uint Id;

    public Shader(GL gl)
    {
        _gl = gl;

        var vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, DefaultVertexShaderSource);
        _gl.CompileShader(vertexShader);

        var fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, DefaultFragmentShaderSource);
        _gl.CompileShader(fragmentShader);

        Id = _gl.CreateProgram();
        _gl.AttachShader(Id, vertexShader);
        _gl.AttachShader(Id, fragmentShader);
        _gl.LinkProgram(Id);

        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        _gl.UseProgram(Id);
    }
}
