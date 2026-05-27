using UnityEngine;

public interface IDamageDealer
{
    int GetDamage(); // Gui damage ma doi tuong gay ra
    LayerMask LayerDamage { get; } // Nhan layer ma doi tuong bi gay sat thuong
}