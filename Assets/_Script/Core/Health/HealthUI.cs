using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : NetworkBehaviour
{
    [Header("Reference")]
    [SerializeField] Health _health;
    [SerializeField] Image _healthBarImage;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;
        _health.CurrentHealth.OnValueChanged += HealthChange;
        HealthChange(0, _health.CurrentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        _health.CurrentHealth.OnValueChanged -= HealthChange;
    }

    void HealthChange(int oldHealth, int newHealth)
    {
        _healthBarImage.fillAmount = (float)newHealth / _health.MaxHealth;
    }




}
