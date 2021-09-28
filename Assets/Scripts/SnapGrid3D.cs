using System;
using TiltBrush;
using UnityEngine;

[ExecuteInEditMode]
public class SnapGrid3D : MonoBehaviour
{
    public static class ShaderParam
    {
        public static readonly int Color = Shader.PropertyToID("_Color");
        public static readonly int PointerOrigin = Shader.PropertyToID("_PointerOrigin_GS");
        public static readonly int CanvasOrigin = Shader.PropertyToID("_CanvasOrigin_GS");
        public static readonly int GridCount = Shader.PropertyToID("_GridCount");
        public static readonly int GridInterval = Shader.PropertyToID("_GridInterval");
        public static readonly int LineWidth = Shader.PropertyToID("_LineWidth");
        public static readonly int LineLength = Shader.PropertyToID("_LineLength");
        public static readonly int CanvasToWorldMatrix = Shader.PropertyToID("_CanvasMatrix");
        public static readonly int WorldToCanvasMatrix = Shader.PropertyToID("_InverseCanvasMatrix");
        public static readonly int CanvasScale = Shader.PropertyToID("_CanvasScale");
        
    }

    public Material material;
    public Color color;
    public Vector3Int gridCount = Vector3Int.one * 5;
    public float gridInterval = 2f;
    public float lineLength = 0.3f;
    public float lineWidth = 0.01f;
    
    private Transform pointer;
    private Transform canvas;
    private bool initialized;
    
    private void Start()
    {
        Debug.Log($"{PointerManager.m_Instance}");
        if (PointerManager.m_Instance == null) return;
        pointer = PointerManager.m_Instance.MainPointer.transform;
        canvas = transform;  // TODO we're a child of the canvas so this works as long as we're not transformed relative to the canvas
        initialized = true;
    }
    private void OnRenderObject()
    {
        if (!initialized) return;
        if ((Camera.current.cullingMask & (1 << gameObject.layer)) != 0)
        {
            var lineVertexCount = 6 * 2;
            var starVertexCount = lineVertexCount * 3;

            var vertexCount = gridCount.x * gridCount.y * gridCount.z * starVertexCount;

            material.SetMatrix(ShaderParam.CanvasToWorldMatrix, canvas.transform.localToWorldMatrix);
            material.SetMatrix(ShaderParam.WorldToCanvasMatrix, canvas.transform.worldToLocalMatrix);
            
            material.SetVector(ShaderParam.PointerOrigin, pointer.position);
            material.SetVector(ShaderParam.CanvasOrigin, canvas.position);
            material.SetColor(ShaderParam.Color, color);
            material.SetVector(ShaderParam.GridCount, (Vector3)gridCount);
            material.SetFloat(ShaderParam.GridInterval, gridInterval);
            material.SetFloat(ShaderParam.LineWidth, lineWidth);
            material.SetFloat(ShaderParam.LineLength, lineLength);
            material.SetFloat(ShaderParam.CanvasScale, canvas.transform.lossyScale.x);

            material.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, vertexCount);
        }
    }
}
