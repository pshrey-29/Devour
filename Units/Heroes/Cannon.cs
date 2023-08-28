using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : HeroUnitBase
{
    // [SerializeField] private AudioClip _someSound;
    private GameObject prefab;
    private bool collidedOnce;
    // [SerializeField] AudioClip clip;

    // private void Awake() => GameManager.OnFireTime += Fire;
    // private void OnDestroy() => GameManager.OnFireTime -= Fire;
    private void OnDestroy() => OnFireTime -= Fire;

    void Start()
    {
        // Example usage of a static system
        // AudioSystem.Instance.PlaySound(_someSound);
        maxShots = Hero.maxShots;
        Debug.Log(Hero.name);
        profileSprite = Hero.MenuSprite;
        transform.Find("Canvas").Find("Image").GetComponent<Image>().sprite = profileSprite;
        prefab = Hero.projectilePrefab;
        collidedOnce = false;
        shotsFired = 0;
        _timer = new Timer(timeGapInFire);
    }

    // public override void ExecuteMove() {
    //     // Perform tarodev specific animation, do damage, move etc.
    //     // You'll obviously need to accept the move specifics as an argument to this function. 
    //     // I go into detail in the Grid Game #2 video
    //     base.ExecuteMove(); // Call this to clean up the move
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!collidedOnce && other.transform.CompareTag("Platform"))
        {
            // Debug.Log("collision");
            // GameManager.OnFireTime += Fire;
            OnFireTime += Fire;
            collidedOnce = true;
            // AudioSystem.Instance.canPlayFireSound = true;
        }
    }



    private void Fire()
    {
        float maxDistance = 8.5f - transform.position.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, 0),
            maxDistance, LayerMask.GetMask("Enemy"));
        if (hit)
        {
            Debug.Log(hit.collider.name);
            Vector3 relativePositionToSpawnAt = new Vector3(0.25f, 0.05f, 0);
            float scale = transform.localScale.x;
            Vector3 positionToSpawnAt = transform.position + relativePositionToSpawnAt * scale;
            Quaternion rot;
            if(Hero.name == "Carrot" || Hero.name == "Ice Cream Cone")
            {
                rot = Quaternion.EulerAngles(0,0,90);
            }
            else{
                rot = Quaternion.identity;
            }
            GameObject projectile = Instantiate(prefab, positionToSpawnAt, rot);
            projectile.transform.localScale *= scale;
            projectile.GetComponent<ProjectileUnitBase>().Hero = Hero;
            gameObject.GetComponent<AudioSource>().Play();
            shotsFired++;
        }
    }




}
