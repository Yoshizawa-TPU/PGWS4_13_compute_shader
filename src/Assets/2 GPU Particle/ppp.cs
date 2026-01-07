using UnityEngine;
using System.Runtime.InteropServices;

struct Particle
{
    public Vector3 Position;
    public Vector3 Velocit;
    public float Color;
}
public class ppp : MonoBehaviour
{
    [SerializeField] Material material = default!;
    [SerializeField] ComputeShader compute = default!;

    private int updateKernel;
    private ComputeBuffer buffer;

    private const int THEAD_NUM = 64;
    private const int PARTICLE_NUM = ((65536+THEAD_NUM-1)/THEAD_NUM) * THEAD_NUM;

    private void OnEnable()
    {
        buffer = new ComputeBuffer(PARTICLE_NUM,Marshal.SizeOf(typeof(Particle)),ComputeBufferType.Default);

        int initKernel = compute.FindKernel("CSInitialize");
        compute.SetBuffer(initKernel,"Particles",buffer);
        compute.Dispatch(initKernel, PARTICLE_NUM / THEAD_NUM, 1, 1);

        updateKernel = compute.FindKernel("CSUpdate");
        compute.SetBuffer(updateKernel, "Particlose", buffer);

        material.SetBuffer("Particles", buffer);
    }

    private void OnDisable()
    {
        buffer.Release();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        compute.SetFloat("deltaTime", Time.deltaTime);
        compute.Dispatch(updateKernel,PARTICLE_NUM/ THEAD_NUM,1,1);


    }

    private void OnRenderObject()
    {
        material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points,PARTICLE_NUM);
    }
}
