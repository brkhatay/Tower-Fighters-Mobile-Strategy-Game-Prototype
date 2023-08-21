using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSideAppearance : MonoBehaviour
{
    #region Inspector

    [SerializeField] private DamageAppearance[] damageAppearance;

    [Header("Particles")]
    [SerializeField] private ParticleSystem smokeParticle;
    [SerializeField] private ParticleSystem fireParticle;

    [Header("Objects")]
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject canon;

    [Header("Materials")]
    [SerializeField] private Material redMain;
    [SerializeField] private Material redFrensel;

    [SerializeField] private Material blueMain;

    [SerializeField] private Material blueFrensel;

    #endregion

    private List<Renderer> renderers = new List<Renderer>();
    private byte damageCount;

    private void Awake()
    {
        Renderer[] rend = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in rend)
        {
            if (!renderer.GetComponent<ParticleSystem>())
                renderers.Add(renderer);
        }
    }

    #region Color Set

    [PunRPC]
    public void SetPlayerColor(int colorCode)
    {
        if (colorCode == 0)
            SetPlayerSideAppearanceRed();
        else
            SetPlayerSideAppearanceBlue();
    }

    public void SetPlayerSideAppearanceRed()
    {
        foreach (Renderer renderer in renderers)
            renderer.sharedMaterials = new[] {redMain, redFrensel};
    }

    public void SetPlayerSideAppearanceBlue()
    {
        foreach (Renderer renderer in renderers)
            renderer.sharedMaterials = new[] {blueMain, blueFrensel};
    }

    #endregion

    #region Damage

    [PunRPC]
    public void OnTakeDamageAppearance()
    {
        if (damageCount >= damageAppearance.Length) return;
        for (int i = 0; i < damageAppearance[damageCount].objects.Length; i++)
        {
            damageAppearance[damageCount].objects[i].SetActive(false);
        }

        damageCount++;

        if (damageCount == 3)
            smokeParticle.Play();
        if (damageCount == 4)
            fireParticle.Play();
    }

    #endregion
}

[System.Serializable]
public class DamageAppearance
{
    public GameObject[] objects;
}