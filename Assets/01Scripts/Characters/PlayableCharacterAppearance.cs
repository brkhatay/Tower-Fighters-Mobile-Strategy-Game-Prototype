using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterAppearance : MonoBehaviour
{
    private List<Material> materials = new List<Material>();

    private void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        List<Renderer> filteredRenderers = new List<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (!renderer.GetComponent<ParticleSystem>())
            {
                filteredRenderers.Add(renderer);
            }
        }

        foreach (Renderer renderer in filteredRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (Utils.ClearInstanceName(mat.name) == Constants.PLAYABLE_CHARACTER_IDENTIFY_MATERIAL_NAME)
                {
                    materials.Add(mat);
                }
            }
        }
    }

    public void SetAppearance(in int sideVal)
    {
        if (sideVal == 0)
            SetAppearanceRed();
        else
            SetAppearanceBlue();
    }

    private void SetAppearanceRed() =>
        materials.ForEach(delegate(Material material)
        {
            material.SetColor("_BaseColor",
                Color.red);
        });

    private void SetAppearanceBlue() =>
        materials.ForEach(delegate(Material material)
        {
            material.SetColor("_BaseColor",
                Color.blue
            );
        });
}