using Silk.NET.OpenGL;

namespace Calango.Smoke;

public class Shader
{
    private readonly GL _gl;

    private const string DefaultVertexShaderSource = """
        #version 330 core
        layout (location = 0) in vec3 aPos;
        layout (location = 1) in vec2 aTexCoord;

        out vec2 TexCoord;

        void main()
        {
            gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
            TexCoord = aTexCoord;
        }
        """;

    private const string DefaultFragmentShaderSource = """
        #version 330 core
        out vec4 FragColor;

        in vec2 TexCoord;

        uniform sampler2D ourTexture;

        void main()
        {
            FragColor = texture(ourTexture, TexCoord);
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
