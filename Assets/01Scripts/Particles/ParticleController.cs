using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Pool;

public static class Particles
{
    public static ParticleController particleController;

    #region Scene

    public static ParticleSystem PlayParticle_OnSceneParticles_UI(this RectTransform transform, string name,
        float UI_Z_offset = 10)
    {
        ParticleSystem particle = particleController.GetParticle(name);
        particle.transform.position = CalculateDropIconPosition(transform, UI_Z_offset);
        particle.Play();
        return particle;
    }

    public static void PlayParticle_OnSceneParticles(this Transform target, string name)
    {
        particleController.GetParticle(name).transform.position = target.position;
        particleController.GetParticle(name).Play();
    }

    public static void PlayParticle_OnSceneParticles(this Vector3 target, string name)
    {
        particleController.GetParticle(name).transform.position = target;
        particleController.GetParticle(name).Play();
    }

    #endregion

    #region Pool

    public static void PlayParticle_OnPoolTransform(this Transform target, string name)
    {
        ParticleSystem particle = particleController.OnTakeFromPool(name);

        particle.transform.position = target.position;
        particle.gameObject.AddComponent<ParticleStop>().PoolName = name;
        particle.Play();
    }

    public static ParticleSystem PlayAndGetParticle_OnPoolTransform(this Transform target, string name)
    {
        ParticleSystem particle = particleController.OnTakeFromPool(name);

        particle.transform.position = target.position;
        particle.gameObject.AddComponent<ParticleStop>().PoolName = name;
        particle.Play();
        return particle;
    }

    public static void PlayParticle_OnPoolParent(this Transform parent, string name)
    {
        ParticleSystem particle = particleController.OnTakeFromPool(name);

        particle.transform.SetParent(parent, false);
        particle.gameObject.AddComponent<ParticleStop>().PoolName = name;
        particle.Play();
    }

    public static void PlayParticle_OnPool_UI(this RectTransform transform, string name, float UI_Z_offset = 10)
    {
        ParticleSystem particle = particleController.OnTakeFromPool(name);

        particle.transform.position = CalculateDropIconPosition(transform, UI_Z_offset);

        particle.gameObject.AddComponent<ParticleStop>().PoolName = name;
        particle.Play();
    }

    #endregion

    #region UI Target

    private static Rect UICoordinatesToPixel(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2) transform.position - (size * 0.5f), size);
    }

    private static Vector3 CalculateDropIconPosition(RectTransform UITarget, float UI_Z_offset)
    {
        Vector3 screenPoint = UICoordinatesToPixel(UITarget).center;
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, UI_Z_offset));
    }

    #endregion
}

public class ParticleController : MonoBehaviour
{
    #region Singleton

    public static ParticleController Instance;

    #endregion

    #region Inspector

    [SerializeField] private OnSceneParticles[] particles;
    [SerializeField] private ParticlePool[] particlePool;

    #endregion

    #region Parameters

    private Hashtable particleHash = new Hashtable();

    #endregion

    private void Awake() => Instance ??= this;

    private void Start()
    {
        Particles.particleController = this;
        foreach (OnSceneParticles item in particles)
        {
            particleHash.Add(item.name, item);
        }

        foreach (ParticlePool item in particlePool)
        {
            particleHash.Add(item.name, item);
        }
    }

    #region Public methods

    public void Play(string particleName)
    {
        OnSceneParticles particle = (OnSceneParticles) particleHash[particleName];
        particle.particleObject.Play();
    }

    public ParticleSystem GetParticle(string particleName)
    {
        OnSceneParticles particle = (OnSceneParticles) particleHash[particleName];
        particle.particleObject.gameObject.SetActive(true);
        return particle.particleObject;
    }

    public void OnGameStop()
    {
        for (int i = 0; i < particlePool.Length; i++)
        {
            ParticlePool pool = (ParticlePool) particleHash[particlePool[i].name];
            pool.Pool.Clear();
        }
    }

    #endregion

    #region Pooling Functions

    public void OnReturnedToPool(GameObject targetObject, string poolName)
    {
        ParticlePool pool = (ParticlePool) particleHash[poolName];
        pool.Pool.Release(targetObject);
    }

    public ParticleSystem OnTakeFromPool(string poolName)
    {
        ParticlePool pool = (ParticlePool) particleHash[poolName];
        return pool.Pool.Get().GetComponent<ParticleSystem>();
    }

    public void DeactivateAllPool(string poolName)
    {
        ParticlePool pool = (ParticlePool) particleHash[poolName];
        pool.Pool.Clear();
    }

    #endregion
}

#region Monos

[RequireComponent(typeof(ParticleSystem))]
public class ParticleStop : MonoBehaviour
{
    private string poolName;

    public string PoolName
    {
        get { return poolName; }
        set { poolName = value; }
    }

    private void Start()
    {
        var _ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = _ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void OnParticleSystemStopped()
    {
        ParticleController.Instance.OnReturnedToPool(gameObject, poolName);
        Destroy(this);
    }
}

#endregion

#region Serializables

[System.Serializable]
public struct OnSceneParticles
{
    public string name;
    public ParticleSystem particleObject;
}

[System.Serializable]
public class ParticlePool
{
    #region Inspector

    public string name;
    [SerializeField] private bool collectionChecks = true;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxPoolSize = 10;

    #endregion

    private IObjectPool<GameObject> pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (pool == null)
                pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                    OnDestroyPoolObject, collectionChecks, maxPoolSize);
            return pool;
        }
    }

    private GameObject CreatePooledItem()
    {
        GameObject poolItem = PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity);
        poolItem.SetActive(false);
        return poolItem;
    }

    private void OnReturnedToPool(GameObject targetObject)
    {
        targetObject.transform.SetParent(null);
        targetObject.SetActive(false);
    }

    public void OnTakeFromPool(GameObject targetObject)
    {
        targetObject?.SetActive(true);
    }

    private void OnDestroyPoolObject(GameObject system)
    {
    }

    private void ClearPool()
    {
        pool.Clear();
    }
}

#endregion