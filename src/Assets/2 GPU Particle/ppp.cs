using UnityEngine;
using System.Runtime.InteropServices;

struct Particle
{
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 Color;
}
public class ppp : MonoBehaviour
{
    [SerializeField] Material material = default!;
    [SerializeField] ComputeShader computeShader = default!;

    private int updateKernel;
    private ComputeBuffer buffer;

    private const int THREAD_NUM = 64;
    private const int PARTICLE_NUM = ((65536 + THREAD_NUM - 1) / THREAD_NUM) * THREAD_NUM;

    private void OnEnable()
    {
        buffer = new ComputeBuffer(
            PARTICLE_NUM,
            Marshal.SizeOf(typeof(Particle)),
            ComputeBufferType.Default);

        int initKernel = computeShader.FindKernel("CSInitialize");
        computeShader.SetBuffer(initKernel, "Particles", buffer);
        computeShader.Dispatch(initKernel, PARTICLE_NUM / THREAD_NUM, 1, 1);

        updateKernel = computeShader.FindKernel("CSUpdate");
        computeShader.SetBuffer(updateKernel, "Particles", buffer);

        material.SetBuffer("Particles", buffer);
    }

    private void OnDisable()
    {
        buffer.Release();
    }

    void Update()
    {
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.Dispatch(updateKernel, PARTICLE_NUM / THREAD_NUM, 1, 1);
    }

    void OnRenderObject()
    {
        material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, PARTICLE_NUM);
    }
}
