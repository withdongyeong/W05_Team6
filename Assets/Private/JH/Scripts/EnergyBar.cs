using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    public Color emptyEnergyColor;
    public Color fullEnergyColor;
    private SpriteRenderer[] _sprites;


    private void Start()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void UpdateEnergyBarUI(int currentEnergy)
    {
        for (int i = 0; i < _sprites.Length; i++)
        {
            _sprites[i].color = 10-i <= currentEnergy ? fullEnergyColor : emptyEnergyColor;
        }

    }
}
