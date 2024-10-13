using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Calango.Smoke.Shapes;

public class Rect : Shape
{
    private readonly uint _vbo;
    private readonly uint _vao;
    private readonly uint _ebo;
    private readonly uint _texture;

    private readonly GL _gl;

    private static readonly int[] _indices = [
        0, 1, 3,
        1, 2, 3
    ];

    public Rect(GL gl, Vector3[] vertices, Image<Rgba32> image)
    {
        ReadOnlySpan<float> verticesResult = [
            vertices[0].X, vertices[0].Y, vertices[0].Z, 1.0f, 1.0f,
            vertices[1].X, vertices[1].Y, vertices[1].Z, 1.0f, 0.0f,
            vertices[2].X, vertices[2].Y, vertices[2].Z, 0.0f, 0.0f,
            vertices[3].X, vertices[3].Y, vertices[3].Z, 0.0f, 1.0f,
        ];

        _gl = gl;

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, verticesResult, BufferUsageARB.StaticDraw);

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (ReadOnlySpan<int>)_indices, BufferUsageARB.StaticDraw);

        _texture = _gl.GenTexture();
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        _gl.TextureParameter(_texture, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TextureParameter(_texture, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

        var pixelData = new byte[image.Width * image.Height * 4];

        image.CopyPixelDataTo(pixelData);

        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)image.Width, (uint)image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, (ReadOnlySpan<byte>)pixelData);

        _gl.VertexAttribPointer(
            index: 0,
            size: 3,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: 5 * sizeof(float),
            0);

        _gl.EnableVertexAttribArray(0);

        _gl.VertexAttribPointer(
            index: 1,
            size: 2,
            VertexAttribPointerType.Float,
            normalized: false,
            stride: 5 * sizeof(float),
            3 * sizeof(float));

        _gl.EnableVertexAttribArray(1);
    }

    public unsafe override void Render()
    {
        _gl.BindVertexArray(_vao);

        _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, null);
    }
}
