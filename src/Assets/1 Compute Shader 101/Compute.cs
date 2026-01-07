using UnityEngine;

public class Compute : MonoBehaviour
{
    [SerializeField] ComputeShader m_ComputeShader = default!;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int x = 8;
        int y = 8;
        ComputeBuffer computeBuffer = new ComputeBuffer(4 * x * y, sizeof(float));

        int kernelhandle = m_ComputeShader.FindKernel("CSMain");
        m_ComputeShader.SetBuffer(kernelhandle, "Result", computeBuffer);
        m_ComputeShader.Dispatch(kernelhandle, x / 8, y / 8, 1);

        float[] result = new float[4 * x * y];
        computeBuffer.GetData(result);
        computeBuffer.Release();

        for(int i = 0;i<x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float cs_x = result[4 * (i + j * x) + 0];
                float cs_y = result[4 * (i + j * x) + 1]*10.0f;
                float cs_z = result[4 * (i + j * x) + 2]*10.0f;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(cs_x, cs_y, cs_z);
                cube.transform.localScale = new Vector3(0.9f, 0.6f, 0.9f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
