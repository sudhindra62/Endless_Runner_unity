using UnityEngine;

/// <summary>
/// Proxy class ensuring that systems explicitly requesting 'ObjectPool' 
/// are correctly routed to the existing 'ObjectPooler' legacy structure.
/// Solves massive CS0103 / CS0246 errors on pooling architecture.
/// </summary>
public class ObjectPool : ObjectPooler
{
    // Inherits ObjectPooler.Instance access inherently.
}
