using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerProjectileHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _projectilePrefab = null;
    [SerializeField] private Dictionary<PlayerActionProcessor, Coroutine> CoroutinesByPlayer = new Dictionary<PlayerActionProcessor, Coroutine>();

    


    #region Server
    public void AddPlayer(PlayerActionProcessor player)
    {
        CoroutinesByPlayer.Add(player, null);

        player.OnProjectileSpawnRequested += SpawnProjectile;
        player.OnProjectileFireRequested += FireProjectile;
    }

    public void RemovePlayer(PlayerActionProcessor player)
    {
        CoroutinesByPlayer.Remove(player);
        
        player.OnProjectileSpawnRequested -= SpawnProjectile;
        player.OnProjectileFireRequested -= FireProjectile;
    }

    [Server]
    private void SpawnProjectile(PlayerActionProcessor player)
    {
        PlayerIdentity ownerID = player.GetComponent<PlayerIdentity>();

        if (ownerID == null)
        {
            Debug.Log("Could not instantiate projectile because of missing PlayerIdentity.");
            return;
        }


        GameObject projectileInstance = Instantiate(
            _projectilePrefab,
            player.ProjectileSpawn.position, 
            player.ProjectileSpawn.rotation);

        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.SetOwner(ownerID);

        player.GetComponent<MovementSpeedModifier>().AddModifier(projectile.MovementSpeedModifier);

        NetworkServer.Spawn(projectileInstance);

        CoroutinesByPlayer[player] = StartCoroutine(AnchorProjectileToPlayer(player, projectile));
    }

    IEnumerator AnchorProjectileToPlayer(PlayerActionProcessor player, Projectile projectile)
    {
        // catch first call where it's not initialized
        while (CoroutinesByPlayer[player] == null) yield return 0;

        while (CoroutinesByPlayer[player] != null)
        {
            
            projectile.transform.position = player.ProjectileSpawn.position;
            projectile.transform.rotation = player.ProjectileSpawn.rotation;
            yield return 0;
        }

        player.GetComponent<MovementSpeedModifier>().RemoveModifier(projectile.MovementSpeedModifier);
        projectile.Fire(player.ProjectileSpawn.forward);
    }

    [Server]
    private void FireProjectile(PlayerActionProcessor player)
    {
        CoroutinesByPlayer[player] = null;
    }

    #endregion
}
