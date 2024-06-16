using UnityEngine;

public interface IVFXEntity
{
    public Color CurrentColor { get; }
    public Vector3 ExplosionPosition { get; }
    public Material DamageMaterial => null; 
    public Material AttackMaterial => null;
    public Material NormalMaterial => null;
    public Renderer[] Renderers => null;
}
