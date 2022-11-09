using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BackgroundMaterialHolder : MonoBehaviour
{
    [Header("Level 1")]
    [SerializeField] private List<MeshRenderer> M01 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M11 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M21 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M31 = new List<MeshRenderer>();
    [SerializeField] private List<Material> darkenMaterial1 = new List<Material>();
    [Header("Level 2")]
    [SerializeField] private List<MeshRenderer> M02 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M12 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M22 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M32 = new List<MeshRenderer>();
    [SerializeField] private List<Material> darkenMaterial2 = new List<Material>();
    [Header("Level 3")]
    [SerializeField] private List<MeshRenderer> M03 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M13 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M23 = new List<MeshRenderer>();
    [SerializeField] private List<MeshRenderer> M33 = new List<MeshRenderer>();
    [SerializeField] private List<Material> darkenMaterial3 = new List<Material>();


    void Start()
    {
        for(int i = 0; i < M02.Count; i++)
        {
            M02[i].material = darkenMaterial2[0];
        }
        for (int i = 0; i < M12.Count; i++)
        {
            M12[i].material = darkenMaterial2[1];
        }
        for (int i = 0; i < M22.Count; i++)
        {
            M22[i].material = darkenMaterial2[2];
        }
        for (int i = 0; i < M32.Count; i++)
        {
            M32[i].material = darkenMaterial2[3];
        }
    }
}
