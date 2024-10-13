using Silk.NET.OpenGL;
using System.Numerics;

namespace Calango.Smoke.Shapes;

public class Triangle : Shape
{
    private readonly uint _vbo;
    private readonly uint _vao;

    private readonly GL _gl;

    public Triangle(GL gl, Vector3[] vertices)
    {
        _gl = gl;
        
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, (ReadOnlySpan<Vector3>)vertices, BufferUsageARB.StaticDraw);

        _gl.VertexAttribPointer(
            index: 0,
            size: 3,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: 3 * sizeof(float),
            IntPtr.Zero);

        _gl.EnableVertexAttribArray(0);
    }

    public override void Render()
    {
        _gl.BindVertexArray(_vao);

        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}
